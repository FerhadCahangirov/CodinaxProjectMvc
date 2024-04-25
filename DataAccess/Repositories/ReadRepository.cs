using CodinaxProjectMvc.Context;
using CodinaxProjectMvc.DataAccess.Models.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;

namespace CodinaxProjectMvc.DataAccess.Repositories
{
    public class ReadRepository<TEntity> : IReadRepository<TEntity> where TEntity : class, IBaseGenericEntity
    {

        private readonly CodinaxDbContext _context;

        public ReadRepository(CodinaxDbContext context)
        {
            _context = context;
        }

        public DbSet<TEntity> Table => _context.Set<TEntity>();


        public IQueryable<TEntity> GetAll(bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();
            return query;
        }

        public IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> method, bool tracking = true)
        {
            var query = Table.Where(method);
            if (!tracking)
                query = query.AsNoTracking();
            return query;
        }

        public async Task<TEntity> GetByIdAsync(string id, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = Table.AsNoTracking();
            return await query.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));
        }

        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> method, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = Table.AsNoTracking();
            return await query.FirstOrDefaultAsync(method);
        }

    }
}
