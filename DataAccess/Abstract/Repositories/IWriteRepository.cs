using CodinaxProjectMvc.DataAccess.Models.Common;

namespace CodinaxProjectMvc.DataAccess.Abstract.Repositories
{
    public interface IWriteRepository<TEntity> : IRepository<TEntity> where TEntity : class, IBaseGenericEntity
    {
        Task<bool> AddAsync(TEntity model);
        Task<bool> AddRangeAsync(List<TEntity> models);
        bool Remove(TEntity model);
        bool RemoveRange(List<TEntity> models);
        Task<bool> RemoveAsync(string id);
        bool Update(TEntity model);
        Task<int> SaveAsync();

    }
}
