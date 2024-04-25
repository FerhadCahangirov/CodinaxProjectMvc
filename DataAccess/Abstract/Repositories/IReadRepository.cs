using CodinaxProjectMvc.DataAccess.Models.Common;
using CodinaxProjectMvc.DataAccess.Models.Identity;
using System.Linq.Expressions;

namespace CodinaxProjectMvc.DataAccess.Abstract.Repositories
{
    public interface IReadRepository<TEntity> : IRepository<TEntity> where TEntity : class, IBaseGenericEntity
    {
        IQueryable<TEntity> GetAll(bool tracking = true);
        IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> method, bool tracking = true);
        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> method, bool tracking = true);
        Task<TEntity> GetByIdAsync(string id, bool tracking = true);
    }
}
