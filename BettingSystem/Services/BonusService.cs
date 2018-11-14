using System;
using System.Linq;
using System.Threading.Tasks;
using BetingSystem.DAL;
using BetingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BetingSystem.Services
{
    public interface IBonusService
    {
        Task ApplyBonuses(Ticket ticket);
    }

    public class BonusService : AbstractService, IBonusService
    {
        private readonly IBonusRepository _bonusRepository;

        public BonusService(IUnitOfWork unitOfWork, IBonusRepository bonusRepository) : base(unitOfWork)
        {
            _bonusRepository = bonusRepository;
        }

        public async Task ApplyBonuses(Ticket ticket)
        {
            var allBonuses = await _bonusRepository.GetAll();

            var numberOfSportsOnTicket = ticket.BetedPairs
                .Select(p => p.BetablePair.Team1.SportId )
                .Distinct()
                .Count();

            if (allBonuses.VariousSportsBonus != null)
            {
                if (numberOfSportsOnTicket >= allBonuses.VariousSportsBonus.RequiredNumberOfDifferentSports)
                    await Apply(ticket, allBonuses.VariousSportsBonus);
            }

            if(allBonuses.AllSportsBonus != null)
            {
                var numberOfSports = await UnitOfWork.Sports.GenericQuery().Select(s => s.Id).Distinct().CountAsync();
                if (numberOfSportsOnTicket >= numberOfSports)
                    await Apply(ticket, allBonuses.AllSportsBonus);
            }
        }

        private async Task Apply(Ticket ticket, IQuotaIncreasingBonus bonus)
        {
            UnitOfWork.AppliedBonuses.Insert(new AppliedBonus { BonusName = bonus.Name, TicketId = ticket.Id });
            ticket.Quota += bonus.IncreasesQuotaByN;
            UnitOfWork.Tickets.Update(ticket);
            await UnitOfWork.SaveChanges();
        }
    }
}
