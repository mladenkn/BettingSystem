using ApplicationKernel;
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

        private IRepository<BetablePair> _betablePairs;
        public IRepository<BetablePair> BetablePairs => 
            _betablePairs ?? (_betablePairs = new Repository<BetablePair>(DbContext.Set<BetablePair>()));

        private ITicketRepository _tickets;
        public ITicketRepository Tickets =>
            _tickets ?? (_tickets = new TicketRepository(DbContext.Set<Ticket>()));

        private IRepository<Sport> _sports;
        public IRepository<Sport> Sports =>
            _sports ?? (_sports = new Repository<Sport>(DbContext.Set<Sport>()));

        private IRepository<AppliedBonus> _appliedBonus;
        public IRepository<AppliedBonus> AppliedBonuses =>
            _appliedBonus ?? (_appliedBonus = new Repository<AppliedBonus>(DbContext.Set<AppliedBonus>()));
    }
}
