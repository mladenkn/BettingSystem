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
}
