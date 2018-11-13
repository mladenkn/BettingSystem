using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ApplicationKernel
{
    public interface IDatabase
    {
        Task SaveChanges();
    }

    public class Database : IDatabase
    {
        private readonly DbContext _dbContext;

        public Database(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task SaveChanges() => _dbContext.SaveChangesAsync();
    }
}
