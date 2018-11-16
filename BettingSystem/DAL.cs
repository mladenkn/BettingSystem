using System.Collections.Generic;
using System.Threading.Tasks;
using BetingSystem.DTO;
using BetingSystem.Models;

namespace BetingSystem
{
    public interface IDataProvider
    {
        Task<IReadOnlyCollection<TicketDto>> GetUsersTickets(string userId);
    }

    public interface ITicketBonusesRepository
    {
        Task<IEnumerable<ITicketBonus>> AllActive();
    }
}
