using CodinaxProjectMvc.DataAccess.Models.Common;
using CodinaxProjectMvc.DataAccess.Models.Identity;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.DataAccess.Abstract.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, IBaseGenericEntity
    {
        DbSet<TEntity> Table { get; }
    }
}
