using System.Collections.Generic;
using N5.Core.Domain.Entities;
using System.Threading.Tasks;
using N5.Core.DTOs;
using N5.Shared.Pagination;

namespace N5.Core.Shared
{
    public interface IPermissionService
    {
        Task<bool> Request(PermissionDto permission);
        Task<bool> Modify(PermissionDto permission);
        Task<PagedResult<PermissionDto>> Get(PaginationParams paginationParams);
        Task<IEnumerable<PermissionTypeDto>> GetTypes();
    }
}