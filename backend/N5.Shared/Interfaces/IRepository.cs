using N5.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace N5.Shared.Interfaces
{
    public interface IRepository<TEntity, TDto>
        where TEntity : class, IEntity
        where TDto : class
    {
        Task<PagedResult<TDto>> GetAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            PaginationParams paginationParams = null,
            string includeProperties = "");
        Task<TDto> GetByIdAsync(int id);
        Task InsertAsync(TDto dto);
        Task<IEnumerable<TDto>> GetAllAsync();
        void Update(TDto dto);
        void Delete(TDto dto);
    }
}