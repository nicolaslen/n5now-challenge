using System;
using System.Threading.Tasks;
using N5.Core.Domain.Entities;
using N5.Core.DTOs;
using N5.Shared.Interfaces;

namespace N5.Core.Shared;

public interface IUnitOfWork
{
    IRepository<Permission, PermissionDto> PermissionRepository { get; }
    IRepository<PermissionType, PermissionTypeDto> PermissionTypeRepository { get; }

    int Commit();
    void Rollback();
    Task<int> CommitAsync();
    Task RollbackAsync();
}