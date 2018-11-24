using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;

namespace BetingSystem
{
    public interface IUnitOfWork
    {
        void Add(object o);
        void Update(object o);
        void Delete(object o);
        Task SaveChanges();
    }

    public static class UnitOfWorkExtensions
    {
        public static void AddRange(this IUnitOfWork unitOfWork, IEnumerable<object> objects) => objects.ForEach(unitOfWork.Add);
    }
}
