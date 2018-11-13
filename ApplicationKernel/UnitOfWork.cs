using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ApplicationKernel
{
    public interface IUnitOfWork
    {
        Task SaveChanges();
    }

    public class UnitOfWork : IUnitOfWork
    {
        protected readonly DbContext DbContext;

        public UnitOfWork(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        public Task SaveChanges() => DbContext.SaveChangesAsync();
    }
}
