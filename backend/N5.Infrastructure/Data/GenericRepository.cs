using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using N5.Shared.Interfaces;
using N5.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace N5.Infrastructure.Data
{
    public class GenericRepository<TEntity, TDto> : IRepository<TEntity, TDto> 
        where TEntity : class, IEntity
        where TDto : class
    {
        protected readonly ApplicationDbContext DbContext;
        private readonly IMapper _mapper;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            DbContext = dbContext;
            _mapper = mapper;
            _dbSet = dbContext.Set<TEntity>();
        }

        public virtual async Task<PagedResult<TDto>> GetAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            PaginationParams paginationParams = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            var totalCount = await query.CountAsync();

            if (paginationParams != null)
            {
                var skip = paginationParams.PageNumber * paginationParams.PageSize;
                query = query.Skip(skip).Take(paginationParams.PageSize);
            }

            var items = await query.ProjectTo<TDto>(_mapper.ConfigurationProvider).ToListAsync();

            return new PagedResult<TDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = paginationParams?.PageNumber ?? 0,
                PageSize = paginationParams?.PageSize ?? items.Count
            };
        }

        public virtual async Task<TDto> GetByIdAsync(int id)
        {
            var entity = await _dbSet.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<TDto>(entity);
        }

        public virtual async Task InsertAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task<IEnumerable<TDto>> GetAllAsync()
        {
            return await _dbSet.ProjectTo<TDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public virtual void Update(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(TDto dto)
        {
            var entity = _mapper.Map<TDto, TEntity>(dto);
            _dbSet.Attach(entity);
            _dbSet.Remove(entity);
        }
    }
}

