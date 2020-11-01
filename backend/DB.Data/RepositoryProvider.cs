using System;
using System.Collections;
using System.Threading.Tasks;
using Db.Data.Repositories;
using DB.Models;
using Utils;

namespace DB.Data
{
    public interface IRepositoryProvider
    {
        DemoDbContext Db { get; }
        ISoftDeletableRep<User> Users { get; }
        void SaveChanges();
        Task SaveChangesAsync();
        Task ReloadAsync<T>(T entry);
        void Dispose();
    }

    public class RepositoryProvider : IDisposable, IRepositoryProvider
    {
        public RepositoryProvider(DemoDbContext context,
            ISessionContextService sessionContextService)
        {
            Db = context;
            SessionContextService = sessionContextService;
        }

        private ISessionContextService SessionContextService { get; }

        public DemoDbContext Db { get; }

        private readonly Hashtable _h = new Hashtable();

        private T GetRepLazy<T>(Func<T> initializer) where T : class
        {
            var typename = typeof(T).FullName;
            if (!_h.ContainsKey(typename))
            {
                _h[typename] = initializer();
            }

            return (T)_h[typename];
        }

        public void SaveChanges()
        {
            Db.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await Db.SaveChangesAsync();
        }

        public ISoftDeletableRep<User> Users =>
            GetRepLazy(() => new SoftDeletableRep<User>(Db, SessionContextService));

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    Db.Dispose();
                }
            }

            this._disposed = true;
        }

        public async Task ReloadAsync<T>(T entry)
        {
            await Db.Entry(entry).ReloadAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}