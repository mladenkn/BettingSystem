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
                .SelectMany(p => new []{ p.BetablePair.Team1.SportId, p.BetablePair.Team2.SportId })
                .Distinct()
                .Count();

            if (allBonuses.VariousSportsBonus != null)
            {
                if(numberOfSportsOnTicket >= allBonuses.VariousSportsBonus.RequiredNumberOfDifferentSports)
                {
                    UnitOfWork.AppliedBonuses.Insert(new AppliedBonus{BonusName = allBonuses.VariousSportsBonus.Name, TicketId = ticket.Id});
                    await UnitOfWork.SaveChanges();
                }
            }

            if(allBonuses.AllSportsBonus != null)
            {
                var numberOfSports = await UnitOfWork.Sports.GenericQuery().Select(s => s.Id).Distinct().CountAsync();
                if (numberOfSportsOnTicket >= numberOfSports)
                {
                    UnitOfWork.AppliedBonuses.Insert(new AppliedBonus { BonusName = allBonuses.AllSportsBonus.Name, TicketId = ticket.Id });
                    await UnitOfWork.SaveChanges();
                }
            }
        }
    }
}
