using Azure.Storage.Blobs.Models;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.DataAccess.Models.Common;
using CodinaxProjectMvc.DataAccess.Models.Identity;
using System.Linq.Expressions;

namespace CodinaxProjectMvc
{
    public static class QueryFilters
    {
        public static Func<Course, bool> CoursesLayoutFilter()
        {
            return x => !x.IsDeleted && !x.IsArchived && x.Showcase;
        }

        public static Expression<Func<Course, bool>> CoursesLayoutFilter(Guid id)
        {
            return x => x.Id == id && !x.IsDeleted && !x.IsArchived && x.Showcase;
        }
    }


    public static class EntityQueryFilters<TEntity> where TEntity : BaseEntity
    {
        public static Func<TEntity, bool> LayoutFilter
        {
            get
            {
                return x => !x.IsDeleted && !x.IsArchived;
            }
        
        }
    }

    public static class UserQueryFilters<TEntity> where TEntity : AppUser
    {
        public static Func<TEntity, bool> GeneralFilter
        {
            get
            {
                return x => !x.IsDeleted && !x.IsBanned && x.IsApproved && x.EmailConfirmed;
            }
        }
    }

    public static class OrderFilters<TSource> where TSource : BaseEntity
    {
        public static Func<TSource, DateTime> ByCreatedDate
        {
            get
            {
                return x => x.CreatedDate;
            }
        }
    }

    public static class OrderFilters
    {
        public static Func<Course, string?> ByTitle
        {
            get
            {
                return x => x.Title;
            }
        }
    }
}
