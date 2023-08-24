using N5.Core.Domain.Entities;
using N5.Core.Shared;
using N5.Shared.Interfaces;
using System.Threading.Tasks;
using N5.Core.DTOs;

namespace N5.Infrastructure.Data
{
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
