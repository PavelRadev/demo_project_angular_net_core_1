using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DB.Models.Classes;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace DB.Data.Queries
{
    public static class QueriesExtensions
    {
        public static async Task<ListWithTotals<T>> ToListWithTotalsAsync<T>(
            this IQueryable<T> query,
            int? pageSize = 0,
            int? page = 0, 
            string orderedBy = null, 
            bool orderReverse = false) where T : class
        {
            query = query.AsNoTracking();

            query = query.ApplyCustomOrdering(orderedBy, orderReverse);
            
            var totalCount = query.Count();
            
            var itemsToSkip = ApplyCustomCutting(ref query, pageSize, page);

            var cuttenList = await query.ToArrayAsync();
            return new ListWithTotals<T>()
            {
                List = cuttenList,
                CurrentPage = Convert.ToInt32(page),
                PageSize = Convert.ToInt32(pageSize),
                SkippedItems = (itemsToSkip >= totalCount) ? totalCount : itemsToSkip,
                TotalCount = totalCount
            };
        }

        private static int ApplyCustomCutting<T>(ref IQueryable<T> query, int? pageSize, int? page) where T : class
        {
            var itemsToSkip = pageSize.GetValueOrDefault(0) * page.GetValueOrDefault(0);

            if (itemsToSkip > 0)
            {
                query = query.Skip(itemsToSkip);
            }

            if (pageSize > 0)
                query = query.Take(pageSize.GetValueOrDefault());
            return itemsToSkip;
        }

        public static IQueryable<T> ApplyCustomOrdering<T>(
            this IQueryable<T> query,
            string orderedBy = null,
            bool orderReverse = false) where T : class
        {
            if (!string.IsNullOrWhiteSpace(orderedBy))
            {
                query = query.OrderBy(orderedBy + (orderReverse ? " descending" : ""));
            }
            
            return query;
        }

        public static async Task<T> FindByIdAsync<T>(this IQueryable<T> query, Guid id) where T : class, IIdentifiable
        {
            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }
        
        public static async Task<T> GetByIdAsync<T>(this IQueryable<T> query, Guid id) where T : class, IIdentifiable
        {
            var res = await query.FirstOrDefaultAsync(x => x.Id == id);
            
            if (res == null)
            {
                throw new ApplicationException("Object of type " + typeof(T).Name + " with Id=" + id + " not found!");
            }

            return res;
        }

        public static IQueryable<T> WithTrashed<T>(this IQueryable<T> query) where T : class, ISoftDeletable
        {
            return query.IgnoreQueryFilters();
        }
        
        public static IQueryable<T> WithTrashed<T>(this IQueryable<T> query, bool withTrashed) where T : class, ISoftDeletable
        {
            return withTrashed 
                ? query.IgnoreQueryFilters()
                : query.Where(x => x.DeletedAt == null);
        }
        
        public static IQueryable<T> IncludePropertiesFromString<T>(this IQueryable<T> query, IEnumerable<string> includeProperties = null) where T : class
        {
            return includeProperties == null 
                ? query 
                : includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
    }
}