using System.Collections.Generic;
using System.Threading.Tasks;
using BetingSystem.Models;

namespace BetingSystem
{
    public interface IDataProvider
    {
        Task<IReadOnlyCollection<TicketDto>> UsersTickets(string userId);
        Task<int> SportsCount();
        Task<IEnumerable<ITicketBonus>> AllActiveBonuses();
        Task<UserWallet> UsersWallet(string userId);
        Task<IReadOnlyCollection<BetablePair>> BetablePairs(IEnumerable<int> ids);
    }
}
