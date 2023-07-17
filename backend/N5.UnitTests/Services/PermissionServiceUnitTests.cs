using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using N5.Core.Domain.Entities;
using N5.Core.DTOs;
using N5.Core.Mapping;
using N5.Core.Messaging;
using N5.Core.Services;
using N5.Infrastructure.Data;
using N5.Shared.Pagination;
//using Nest;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace N5.UnitTests.Services
{
    public class PermissionServiceUnitTests
    {
        private static DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase(databaseName: "TestDb")
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        private readonly IMapper _mapper;
        private readonly DateTime _expectedDate = DateTime.Now;

        public PermissionServiceUnitTests()
        {
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()));
        }

        private class Dependencies
        {
            public Dependencies(PermissionService permissionService, UnitOfWork unitOfWork)
            {
                PermissionService = permissionService;
                UnitOfWork = unitOfWork;
            }

            public UnitOfWork UnitOfWork { get; }
            public PermissionService PermissionService { get; }
        }

        private async Task<Dependencies> GetDependencies(ApplicationDbContext dbContext)
        {
            await ApplicationDbContext.SeedAsync(dbContext);
            var unitOfWork = new UnitOfWork(dbContext,
                new GenericRepository<Permission, PermissionDto>(dbContext, _mapper),
                new GenericRepository<PermissionType, PermissionTypeDto>(dbContext, _mapper));
            /*var settings = new ConnectionSettings()
                .DefaultIndex("permissions");

            var elasticClient = new ElasticClient(settings);
            var kafkaProducer = new Mock<IKafkaProducer>();*/
            var permissionService = new PermissionService(unitOfWork/*, elasticClient, kafkaProducer.Object*/);
            return new Dependencies(permissionService: permissionService, unitOfWork: unitOfWork);
        }

        [Fact]
        public async Task Request_ShouldInsertPermissionAndCommitUnitOfWork()
        {
            // Arrange
            using (var dbContext = new ApplicationDbContext(CreateNewContextOptions()))
            {
                var services = await GetDependencies(dbContext);

                var permissionDto = new PermissionDto
                {
                    NombreEmpleado = "John",
                    ApellidoEmpleado = "Doe",
                    TipoPermiso = 1
                };
                var expectedPermissionDto = new PermissionDto
                {
                    Id = 1,
                    NombreEmpleado = permissionDto.NombreEmpleado,
                    ApellidoEmpleado = permissionDto.ApellidoEmpleado,
                    TipoPermiso = permissionDto.TipoPermiso
                };

                // Act
                var result = await services.PermissionService.Request(permissionDto);
                var createdPermission = await dbContext.Permissions.FirstAsync();

                // Assert
                Assert.True(result);
                Assert.NotNull(createdPermission);
                Assert.Single(dbContext.Permissions);
                Assert.Equal(expectedPermissionDto.Id, createdPermission.Id);
                Assert.Equal(expectedPermissionDto.NombreEmpleado, createdPermission.NombreEmpleado);
                Assert.Equal(expectedPermissionDto.ApellidoEmpleado, createdPermission.ApellidoEmpleado);
                Assert.Equal(expectedPermissionDto.TipoPermiso, createdPermission.TipoPermiso);
                Assert.Equal(_expectedDate.Date, createdPermission.FechaPermiso.Date);
            }
        }

        [Fact]
        public async Task Modify_ShouldUpdatePermissionAndCommitUnitOfWork()
        {
            // Arrange
            using (var dbContext = new ApplicationDbContext(CreateNewContextOptions()))
            {
                var services = await GetDependencies(dbContext);

                var oldPermissionDto = new PermissionDto
                {
                    NombreEmpleado = "John",
                    ApellidoEmpleado = "Doe",
                    TipoPermiso = 1
                };
                await services.UnitOfWork.PermissionRepository.InsertAsync(oldPermissionDto);
                await services.UnitOfWork.CommitAsync();
                dbContext.ChangeTracker.Entries().ElementAt(0).State = EntityState.Detached;

                var permissionDto = new PermissionDto
                {
                    Id = 1,
                    NombreEmpleado = "Johnson",
                    ApellidoEmpleado = "Doety",
                    TipoPermiso = 2
                };

                // Act
                var result = await services.PermissionService.Modify(permissionDto);
                var modifiedPermission = await dbContext.Permissions.FirstAsync(x => x.Id == permissionDto.Id);

                // Assert
                Assert.True(result);
                Assert.NotNull(modifiedPermission);
                Assert.Single(dbContext.Permissions);
                Assert.Equal(permissionDto.Id, modifiedPermission.Id);
                Assert.Equal(permissionDto.NombreEmpleado, modifiedPermission.NombreEmpleado);
                Assert.Equal(permissionDto.ApellidoEmpleado, modifiedPermission.ApellidoEmpleado);
                Assert.Equal(permissionDto.TipoPermiso, modifiedPermission.TipoPermiso);
                Assert.Equal(_expectedDate.Date, modifiedPermission.FechaPermiso.Date);
            }
        }

        [Fact]
        public async Task Get_ShouldReturnPagedResultOfPermissions()
        {
            // Arrange
            using (var dbContext = new ApplicationDbContext(CreateNewContextOptions()))
            {
                var services = await GetDependencies(dbContext);

                var paginationParams = new PaginationParams
                {
                    PageNumber = 0,
                    PageSize = 5
                };

                const int quantity = 20;
                var permissions = Enumerable.Range(1, quantity).Select(i => new PermissionDto
                {
                    Id = i,
                    NombreEmpleado = "John",
                    ApellidoEmpleado = "Doe",
                    TipoPermiso = 1
                });

                foreach (var permission in permissions)
                {
                    await services.UnitOfWork.PermissionRepository.InsertAsync(permission);
                }

                await services.UnitOfWork.CommitAsync();

                // Act
                var result = await services.PermissionService.Get(paginationParams);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(paginationParams.PageSize, result.Items.Count());
                Assert.Equal(paginationParams.PageNumber, result.PageNumber);
                Assert.Equal(paginationParams.PageSize, result.PageSize);
                Assert.Equal(quantity, result.TotalCount);
            }
        }
    }
}