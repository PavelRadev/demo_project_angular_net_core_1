using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DB.Models.Classes;

namespace Db.Data.Repositories
{
    public interface IBaseRep<T>
        where T : class, IIdentifiable
    {
        IQueryable<T> Queryable { get; }
        Task<int> GetAllItemsCountAsync();
        void Update(T entity);
        bool IsDetached(T entity);
        void Detach(T entity);
        Task AddOrUpdateAsync(T entity);
        Task AddOrUpdateByIdAsync(Guid id, T newValues, IEnumerable<string> excludedProps = null);
        Task AddOrUpdateRangeAsync(IEnumerable<T> entities);
        Task AddOrUpdateRangeByIdAsync(IEnumerable<T> entities);
        Task<T> UpdateByIdAsync(Guid id, T newValues, IEnumerable<string> excludedProps = null);
        Task AddAsync(T entity);
        Task<T> DeleteByIdAsync(Guid id);
        T Delete(T entity);
        void RemoveRange(ICollection<T> entities);
    }
}
