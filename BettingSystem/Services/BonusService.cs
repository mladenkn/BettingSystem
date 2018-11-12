using System;
using System.Collections.Generic;
using BettingSystem.Models;

namespace BettingSystem.Services
{
    public interface IBonusService
    {
        IReadOnlyCollection<TicketBonus> ApplyBonuses(Ticket ticket);
    }

    public class BonusService : IBonusService
    {
        public IReadOnlyCollection<TicketBonus> ApplyBonuses(Ticket ticket)
        {
            throw new NotImplementedException();
        }
    }
}
