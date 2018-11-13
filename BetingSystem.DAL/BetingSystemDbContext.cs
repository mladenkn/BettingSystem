using Microsoft.EntityFrameworkCore;

namespace BetingSystem.DAL
{
    public class BetingSystemDbContext : DbContext
    {
        public BetingSystemDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
