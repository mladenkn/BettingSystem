using ApplicationKernel;
using BetingSystem.Models;

namespace BetingSystem.DAL
{
    public interface IUnitOfWork : ApplicationKernel.IUnitOfWork
    {
        IBetablePairsRepository BetablePairs { get; }
        ITicketRepository Tickets { get; }
        IBetedPairRepository BetedPairs { get; }
        IRepository<Sport> Sports { get; }
    }
}
