using System;
using BetingSystem.DAL;
using Microsoft.EntityFrameworkCore;

namespace BetingSystem.Tests
{
    public static class TestServicesFactory
    {
        public static BetingSystemDbContext DbContext()
        {
            var options = new DbContextOptionsBuilder<BetingSystemDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new BetingSystemDbContext(options);
        }
    }
}
