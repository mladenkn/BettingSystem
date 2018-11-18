using System;
using System.Linq;
using System.Threading.Tasks;
using BetingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BetingSystem.Services
{
    public interface IBonusService
    {
        Task ApplyBonuses(Ticket ticket);
    }

    public class BonusService : IBonusService
    {
        private readonly IBonusApplier _bonusApplier;
        private readonly DbContext _db;

        public BonusService(IBonusApplier bonusApplier, DbContext db)
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

            await _bonusApplier
                .Use(ticket)
                .ApplyAdditionalFor<IQuotaIncreasingBonus>((t, b) => t.Quota += b.IncreasesQuotaBy)
                .VerifyForBonus<VariousSportsBonus>(b => numberOfSportsOnTicket >= b.RequiredNumberOfDifferentSports)
                .VerifyForBonus<AllSportsBonus>(async b =>
                {
                    var numberOfSports = await _db.Set<Sport>().CountAsync();
                    return numberOfSportsOnTicket >= numberOfSports;
                })
                .Apply();
        }
    }
}
