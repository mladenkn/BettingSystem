using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationKernel
{
    public interface IDatabase
    {
        IDatabaseTransaction NewTransaction();
        IQueryable<T> GenericQuery<T>() where T : class;
    }

    public interface IDatabaseTransaction
    {
        IDatabaseTransaction Insert(object o);
        IDatabaseTransaction Update(object o);
        IDatabaseTransaction Delete(object o);
        Task Commit();
    }

    public static class DatabaseTransactionExtensions
    {
        public static IDatabaseTransaction InsertRange(this IDatabaseTransaction transaction, IEnumerable<object> data)
        {
            foreach (var o in data)
                transaction.Insert(o);
            return transaction;
        }
        public static IDatabaseTransaction InsertRange(this IDatabaseTransaction transaction, params object[] data)
        {
            return InsertRange(transaction, (IEnumerable<object>)data);
        }
    }
}
