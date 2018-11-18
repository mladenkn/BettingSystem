using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ApplicationKernel
{
    public class Database : IDatabase
    {
        protected readonly DbContext Db;

        public Database(DbContext db)
        {
            Db = db;
        }

        public virtual IDatabaseTransaction NewTransaction() => new DatabaseTransaction(Db);

        public IQueryable<T> GenericQuery<T>() where T : class => Db.Set<T>();
    }

    public class DatabaseTransaction : IDatabaseTransaction
    {
        private readonly DbContext _db;
        private readonly ICollection<Action> _modifiers = new List<Action>();

        public DatabaseTransaction(DbContext db)
        {
            _db = db;
        }

        public virtual IDatabaseTransaction Insert(object o)
        {
            _modifiers.Add(() => _db.Add(o));
            return this;
        }

        public virtual IDatabaseTransaction Update(object o)
        {
            _modifiers.Add(() => _db.Update(o));
            return this;
        }

        public virtual IDatabaseTransaction Delete(object o)
        {
            _modifiers.Add(() => _db.Remove(o));
            return this;
        }

        public virtual async Task Commit()
        {
            foreach (var modifier in _modifiers)
                modifier();
            await _db.SaveChangesAsync();
        }
    }
}
