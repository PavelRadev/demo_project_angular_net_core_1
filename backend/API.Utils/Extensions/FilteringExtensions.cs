using System.Linq;
using System.Linq.Dynamic.Core;
using API.Utils.Models;
using DB.Data.Queries;
using DB.Models;
using DB.Models.Classes;

namespace API.Utils.Extensions
{
    public static class FilteringExtensions
    {
        public static IQueryable<T> ApplyBaseFilter<T>(
            this IQueryable<T> query,
            BaseFilterModel filters) where T : class
        {
            return query.IncludePropertiesFromString(filters.IncludeProperties);
        }

        public static IQueryable<T> ApplySoftDeletableFilter<T>(
            this IQueryable<T> query,
            SoftDeletableListFilterModel filters) where T : class, ISoftDeletable
        {
            query = query.ApplyBaseFilter(filters);

            query = query.WithTrashed(filters.WithTrashed);

            if (filters.DeletedBy != null && filters.DeletedBy.Any())
            {
                query = query.Where(x => x.DeletedByUserId != null
                                         && filters.DeletedBy.Any(y => y == x.DeletedByUserId));
            }

            if (filters.DeletedFrom != null)
            {
                query = query.Where(x => x.DeletedAt >= filters.DeletedFrom);
            }

            if (filters.DeletedTo != null)
            {
                query = query.Where(x => x.DeletedAt <= filters.DeletedTo);
            }

            return query;
        }
    }
}
