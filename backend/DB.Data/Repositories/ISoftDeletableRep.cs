using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DB.Data;
using DB.Models.Classes;
using Utils;

namespace Db.Data.Repositories
{
    public interface ISoftDeletableRep<T> : IBaseRep<T>
        where T : class, ISoftDeletable, IIdentifiable
    {
        void RemoveRangeFull(ICollection<T> entities);
        Task<T> DeleteFullByIdAsync(Guid id);
        void DeleteFull(T entity);
        void Restore(T entity);
    }
}
