using System;
using System.Linq;
using ApplicationKernel;
using BetingSystem.DAL;
using BetingSystem.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BetingSystem.Tests
{
    public static class MockUtils
    {
        public static ICurrentUserAccessor CurrentUserAccessor(string userId)
        {
            var mock = new Mock<ICurrentUserAccessor>();
            mock.Setup(a => a.Id()).Returns(() => userId);
            return mock.Object;
        }

        public static void SetupNewTransaction(this Mock<IDatabase> dbMock, IDatabaseTransaction transaction)
        {
            dbMock.Setup(d => d.NewTransaction()).Returns(transaction);
        }

        public static void SetupGenericQuery<T>(this Mock<IDatabase> dbMock, params T[] data) where T : class
        {
            dbMock.Setup(r => r.GenericQuery<T>()).Returns(Queryable(data));
        }

        public static DbContext DbContext()
        {
            var options = new DbContextOptionsBuilder().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new BetingSystemDbContext(options);
            return db;
        }

        public static IQueryable<T> Queryable<T>(params T[] data) where T : class
        {
            var db = DbContext();
            var set = db.Set<T>();
            set.AddRange(data);
            db.SaveChanges();
            return set;
        }
    }
}
