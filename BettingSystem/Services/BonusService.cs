using System;
using System.Linq;
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
        private readonly IBonusApplier _bonusApplier;
        private readonly IDataProvider _dataProvider;

        public BonusService(IBonusApplier bonusApplier, IDataProvider dataProvider)
        {
            _bonusApplier = bonusApplier;
            _dataProvider = dataProvider;
        }

        public async Task ApplyBonuses(Ticket ticket)
        {
            var numberOfSportsOnTicket = ticket.BetedPairs
                .Select(p => p.BetablePair.Team1.SportId)
                .Distinct()
                .Count();

            var bonuses = await _dataProvider.AllActiveBonuses();

            await _bonusApplier
                .Use(ticket)
                .Use(bonuses)
                .ApplyAdditionalFor<IQuotaIncreasingBonus>((t, b) => t.Quota += b.IncreasesQuotaBy)
                .VerifyForBonus<VariousSportsBonus>(b => numberOfSportsOnTicket >= b.RequiredNumberOfDifferentSports)
                .VerifyForBonus<AllSportsBonus>(async b =>
                {
                    var numberOfSports = await _dataProvider.SportsCount();
                    return numberOfSportsOnTicket >= numberOfSports;
                })
                .Apply();
        }
    }
}
