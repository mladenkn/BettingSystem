using ApplicationKernel;
using BetingSystem.Models;

namespace BetingSystem.DAL
{
    public interface IUnitOfWork : ApplicationKernel.IUnitOfWork
    {
        IRepository<BetablePair> BetablePairs { get; }
        ITicketRepository Tickets { get; }
        IRepository<Sport> Sports { get; }
        IRepository<AppliedBonus> AppliedBonuses { get; }
    }
}
