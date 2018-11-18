using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ApplicationKernel
{
    public class Database : IDatabase
    {
        private readonly DbContext _db;

        public Database(DbContext db)
        {
            _db = db;
        }

        public IDatabaseTransaction NewTransaction() => new DatabaseTransaction(_db);

        public IQueryable<T> GenericQuery<T>() where T : class => _db.Set<T>();
    }

    public class DatabaseTransaction : IDatabaseTransaction
    {
        private readonly DbContext _db;
        private readonly ICollection<Action<DbContext>> _modifiers = new List<Action<DbContext>>();

        public DatabaseTransaction(DbContext db)
        {
            _db = db;
        }

        public IDatabaseTransaction Insert(object o)
        {
            _modifiers.Add(db => db.Add(o));
            return this;
        }

        public IDatabaseTransaction Update(object o)
        {
            _modifiers.Add(db => db.Update(o));
            return this;
        }

        public IDatabaseTransaction Delete(object o)
        {
            _modifiers.Add(db => db.Remove(o));
            return this;
        }

        public async Task Commit()
        {
            foreach (var modifier in _modifiers)
                modifier(_db);
            await _db.SaveChangesAsync();
        }
    }
}
