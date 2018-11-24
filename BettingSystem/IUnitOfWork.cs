using System.Threading.Tasks;

namespace BetingSystem
{
    public interface IUnitOfWork
    {
        void Add(object o);
        void Update(object o);
        void Delete(object o);
        Task SaveChanges();
    }
}
