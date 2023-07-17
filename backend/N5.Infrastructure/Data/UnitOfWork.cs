using N5.Core.Domain.Entities;
using N5.Core.Shared;
using N5.Shared.Interfaces;
using System.Threading.Tasks;
using N5.Core.DTOs;

namespace N5.Infrastructure.Data
{
    //https://github.com/matthewrenze/clean-architecture-demo/blob/repo-and-uow/Application/Application.csproj OJO .NET (no net core)
    //https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
    //https://medium.com/@mxcmxc/unit-of-work-with-repository-pattern-boilerplate-518726e4beb7

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext,
            IRepository<Permission, PermissionDto> permissionRepository,
            IRepository<PermissionType, PermissionTypeDto> permissionTypeRepository)
        {
            _dbContext = dbContext;
            PermissionRepository = permissionRepository;
            PermissionTypeRepository = permissionTypeRepository;
        }

        public IRepository<Permission, PermissionDto> PermissionRepository { get; }
        public IRepository<PermissionType, PermissionTypeDto> PermissionTypeRepository { get; }

        public int Commit()
            => _dbContext.SaveChanges();
        
        public async Task<int> CommitAsync()
            => await _dbContext.SaveChangesAsync();

        public void Rollback()
            => _dbContext.Dispose();

        public async Task RollbackAsync()
            => await _dbContext.DisposeAsync();
    }
}
