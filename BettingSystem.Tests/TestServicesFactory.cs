using System;
using BetingSystem.DAL;
using Microsoft.EntityFrameworkCore;
using Moq;

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

        public static ICurrentUserAccessor CureCurrentUserAccessor(string userId)
        {
            var mock = new Mock<ICurrentUserAccessor>();
            mock.Setup(a => a.Id()).Returns(() => userId);
            return mock.Object;
        }
    }
}
