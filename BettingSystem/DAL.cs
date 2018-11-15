using System.Collections.Generic;
using System.Threading.Tasks;
using BetingSystem.DTO;
using BetingSystem.Models;

namespace BetingSystem
{
    public interface ITicketBonusRepository
    {
        Task<IEnumerable<ITicketBonus>> GetAll();
        Task Persist(ITicketBonus bonus);
    }

    public interface IDataProvider
    {
        Task<IEnumerable<TicketDto>> GetUsersTickets(string userId);
    }
}
