using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BetingSystem.DAL
{
    public interface IUnitOfWork : ApplicationKernel.IUnitOfWork
    {
        IBetablePairsRepository BetablePairs { get; }
        ITicketRepository Tickets { get; }
    }

    public class UnitOfWork : ApplicationKernel.UnitOfWork, IUnitOfWork
    {
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWork(DbContext dbContext, IServiceProvider serviceProvider) : base(dbContext)
        {
            _serviceProvider = serviceProvider;
        }

        public IBetablePairsRepository BetablePairs => _serviceProvider.GetService<IBetablePairsRepository>();
        public ITicketRepository Tickets => _serviceProvider.GetService<ITicketRepository>();
    }
}
