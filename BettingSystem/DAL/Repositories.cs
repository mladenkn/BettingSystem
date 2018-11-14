using System.Threading.Tasks;
using ApplicationKernel;
using BetingSystem.Models;

namespace BetingSystem.DAL
{
    public interface ITicketRepository : IRepository<Ticket>
    {
    }

    public interface IBetablePairsRepository : IRepository<BetablePair>
    {
    }

    public interface IBetedPairRepository : IRepository<BetedPair>
    {
    }
}
