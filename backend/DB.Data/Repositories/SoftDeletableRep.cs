using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DB.Data;
using DB.Data.Queries;
using DB.Models.Classes;
using Utils;

namespace Db.Data.Repositories
{
    public class SoftDeletableRep<T> : BaseRep<T>, ISoftDeletableRep<T>
        where T : class, ISoftDeletable, IIdentifiable
    {
        protected ISessionContextService SessionContextService { get; }

        public SoftDeletableRep(DemoDbContext context,
            ISessionContextService sessionContextService) : base(context)
        {
            SessionContextService = sessionContextService;
        }

        protected override async Task<T> FindByIdIgnoringFilters(Guid id)
        {
            return await Queryable.WithTrashed().FindByIdAsync(id);
        }

        public override void RemoveRange(ICollection<T> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public void RemoveRangeFull(ICollection<T> entities)
        {
            base.RemoveRange(entities);
        }

        public override T Delete(T entity)
        {
            entity.DeletedAt = DateTime.Now;
            entity.DeletedByUserId = SessionContextService?.CurrentUserId;

            return entity;
        }

        public async Task<T> DeleteFullByIdAsync(Guid id)
        {
            return await base.DeleteByIdAsync(id);
        }

        public void DeleteFull(T entity)
        {
            base.Delete(entity);
        }

        public void Restore(T entity)
        {
            if (entity.DeletedAt is null)
            {
                throw new ApplicationException("Entity isn't removed yet. Nothing to restore");
            }

            entity.DeletedAt = null;
            entity.DeletedByUserId = null;
        }
    }
}
