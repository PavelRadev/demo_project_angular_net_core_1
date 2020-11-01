using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DB.Data;
using DB.Data.Queries;
using DB.Models.Classes;
using DB.Models.Extensions;

namespace Db.Data.Repositories
{
    public class BaseRep<T> : IBaseRep<T>
        where T : class, IIdentifiable
    {
        protected DemoDbContext Db { get; }
        protected DbSet<T> DbSet { get; }

        public BaseRep(DemoDbContext dbContext)
        {
            Db = dbContext;
            DbSet = Db.Set<T>();
        }

        public IQueryable<T> Queryable => DbSet;

        public async Task<int> GetAllItemsCountAsync()
        {
            return await Queryable.CountAsync();
        }

        public virtual void Update(T entity)
        {
            EntityEntry entry = Db.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        public bool IsDetached(T entity)
        {
            EntityEntry entry = Db.Entry(entity);
            return entry.State == EntityState.Detached;
        }

        public void Detach(T entity)
        {
            EntityEntry entry = Db.Entry(entity);
            entry.State = EntityState.Detached;
        }

        public virtual async Task AddOrUpdateAsync(T entity)
        {
            if (IsDetached(entity))
            {
                await AddAsync(entity);
            }
            else
            {
                Update(entity);
            }
        }

        protected virtual async Task<T> FindByIdIgnoringFilters(Guid id)
        {
            return await Queryable.FindByIdAsync(id);
        }

        public virtual async Task AddOrUpdateByIdAsync(Guid id, T newValues, IEnumerable<string> excludedProps = null)
        {
            var entity = await FindByIdIgnoringFilters(id);

            if (entity is null)
            {
                newValues.Id = id;
                await AddAsync(newValues);
            }
            else
            {
                await UpdateByIdAsync(id, newValues, excludedProps);
            }
        }

        public async Task AddOrUpdateRangeAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                await AddOrUpdateAsync(entity);
            }
        }

        public async Task AddOrUpdateRangeByIdAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                await AddOrUpdateByIdAsync(entity.Id, entity);
            }
        }

        public async Task<T> UpdateByIdAsync(Guid id, T newValues, IEnumerable<string> excludedProps = null)
        {
            var entity = await FindByIdIgnoringFilters(id);

            entity.CopyValues(newValues, excludedProps);

            await AddOrUpdateAsync(entity);

            return entity;
        }

        public virtual async Task AddAsync(T entity)
        {
            var entry = Db.Entry(entity);
            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
            else
            {
                await DbSet.AddAsync(entity);
            }
        }

        public async Task<T> DeleteByIdAsync(Guid id)
        {
            var entity = await FindByIdIgnoringFilters(id);

            Delete(entity);

            return entity;
        }

        public virtual T Delete(T entity)
        {
            EntityEntry entry = Db.Entry(entity);
            if (entry.State != EntityState.Deleted)
            {
                entry.State = EntityState.Deleted;
            }
            else
            {
                DbSet.Attach(entity);
                DbSet.Remove(entity);
            }

            return entity;
        }

        public virtual void RemoveRange(ICollection<T> entities)
        {
            DbSet.RemoveRange(entities);
        }
    }
}
