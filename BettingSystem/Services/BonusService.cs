using System.Linq;
using System.Threading.Tasks;
using ApplicationKernel;
using BetingSystem.Models;
using Microsoft.EntityFrameworkCore;
using Utilities;

namespace BetingSystem.Services
{
    public interface IBonusService
    {
        Task ApplyBonuses(Ticket ticket);
    }

    public class BonusService : IBonusService
    {
        private readonly IBonusApplier _bonusApplier;
        private readonly IDatabase _db;

        public BonusService(IBonusApplier bonusApplier, IDatabase db)
        {
            _bonusApplier = bonusApplier;
            _db = db;
        }

        public async Task ApplyBonuses(Ticket ticket)
        {
            var numberOfSportsOnTicket = ticket.BetedPairs
                .Select(p => p.BetablePair.Team1.SportId)
                .Distinct()
                .Count();

            var bonuses = await _db.DataProvider.GetActiveBonuses();

            var appliedBonuses = await _bonusApplier
                .Use(ticket)
                .Use(bonuses)
                .ApplyAdditionalFor<IQuotaIncreasingBonus>((t, b) => t.Quota += b.IncreasesQuotaBy)
                .VerifyForBonus<VariousSportsBonus>(b => numberOfSportsOnTicket >= b.RequiredNumberOfDifferentSports)
                .VerifyForBonus<AllSportsBonus>(async b =>
                {
                    var numberOfSports = await _db.GenericQuery<Sport>().CountAsync();
                    return numberOfSportsOnTicket >= numberOfSports;
                })
                .Apply();

            await _db
                .NewTransaction()
                .InsertRange(appliedBonuses)
                .Update(ticket)
                .Commit();
        }
    }
}
