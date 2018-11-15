using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetingSystem.Models;

namespace BetingSystem.DAL
{
    public class TicketBonusRepository : ITicketBonusRepository
    {
        public Task<IEnumerable<ITicketBonus>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task Persist(ITicketBonus bonus)
        {
            throw new NotImplementedException();
        }
    }
}
