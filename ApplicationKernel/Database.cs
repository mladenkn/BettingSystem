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

        public DatabaseTransaction(DbContext db)
        {
            _db = db;
        }

        public IDatabaseTransaction Insert(object o)
        {
            _db.Add(o);
            return this;
        }

        public IDatabaseTransaction Update(object o)
        {
            _db.Update(o);
            return this;
        }

        public IDatabaseTransaction Delete(object o)
        {
            _db.Remove(o);
            return this;
        }

        public async Task Commit() => await _db.SaveChangesAsync();
    }
}
