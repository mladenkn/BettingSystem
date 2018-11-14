using BetingSystem.DAL;
using Microsoft.EntityFrameworkCore;

namespace BetingSystem.Tests
{
    public static class TestServicesFactory
    {
        public static BetingSystemDbContext DbContext()
        {
            var options = new DbContextOptionsBuilder<BetingSystemDbContext>()
                .UseInMemoryDatabase("test-db")
                .Options;
            return new BetingSystemDbContext(options);
        }

        public static UnitOfWork UnitOfWork(BetingSystemDbContext db) => new UnitOfWork(db);
    }
}
