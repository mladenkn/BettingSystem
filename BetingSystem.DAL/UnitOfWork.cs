using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetingSystem.DAL.Repositories;
using BetingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BetingSystem.DAL
{
    public class UnitOfWork : ApplicationKernel.UnitOfWork, IUnitOfWork
    {
        public UnitOfWork(DbContext dbContext) : base(dbContext)
        {
        }

        private IBetablePairsRepository _betablePairs;
        public IBetablePairsRepository BetablePairs => 
            _betablePairs ?? (_betablePairs = new BetablePairRepository(DbContext.Set<BetablePair>()));

        private ITicketRepository _tickets;
        public ITicketRepository Tickets =>
            _tickets ?? (_tickets = new TicketRepository(DbContext.Set<Ticket>()));
    }
}
