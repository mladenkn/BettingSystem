using System;
using System.Threading.Tasks;
using ApplicationKernel;
using BetingSystem.DAL;
using BetingSystem.DAL.Repositories;
using BetingSystem.Models;
using Microsoft.EntityFrameworkCore;
using IUnitOfWork = BetingSystem.DAL.IUnitOfWork;
using UnitOfWork = BetingSystem.DAL.UnitOfWork;

namespace BetingSystem.Tests
{
    public static class TestServicesFactory
    {
        public static IUnitOfWork UnitOfWork()
        {
            var options = new DbContextOptionsBuilder<BetingSystemDbContext>()
                .UseInMemoryDatabase("test-db")
                .Options;
            var context = new BetingSystemDbContext(options);
            return new UnitOfWork(context);
        }
    }
}
