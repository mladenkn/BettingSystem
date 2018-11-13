using System;
using System.Threading.Tasks;
using BetingSystem.Models;

namespace BetingSystem.Services
{
    public interface IBonusService
    {
        Task ApplyBonuses(Ticket ticket);
    }

    public class BonusService : IBonusService
    {
        public Task ApplyBonuses(Ticket ticket)
        {
            throw new NotImplementedException();
        }
    }
}
