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
        private readonly DbContext _dbContext;

        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task SaveChanges() => _dbContext.SaveChangesAsync();
    }
}
