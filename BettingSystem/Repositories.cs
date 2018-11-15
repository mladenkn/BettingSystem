using System.Collections.Generic;
using System.Threading.Tasks;
using BetingSystem.Models;

namespace BetingSystem
{
    public interface ITicketBonusRepository
    {
        Task<IEnumerable<ITicketBonus>> GetAll();
        Task Persist(ITicketBonus bonus);
    }
}
