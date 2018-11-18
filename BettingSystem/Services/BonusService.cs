using System.Linq;
using System.Threading.Tasks;
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
        private readonly DbContext _db;
        private readonly ITicketBonusesRepository _bonuses;

        public BonusService(IBonusApplier bonusApplier, DbContext db, ITicketBonusesRepository bonuses)
        {
            _bonusApplier = bonusApplier;
            _db = db;
            _bonuses = bonuses;
        }

        public async Task ApplyBonuses(Ticket ticket)
        {
            var numberOfSportsOnTicket = ticket.BetedPairs
                .Select(p => p.BetablePair.Team1.SportId)
                .Distinct()
                .Count();

            var bonuses = await _bonuses.AllActive();

            var appliedBonuses = await _bonusApplier
                .Use(ticket)
                .Use(bonuses)
                .ApplyAdditionalFor<IQuotaIncreasingBonus>((t, b) => t.Quota += b.IncreasesQuotaBy)
                .VerifyForBonus<VariousSportsBonus>(b => numberOfSportsOnTicket >= b.RequiredNumberOfDifferentSports)
                .VerifyForBonus<AllSportsBonus>(async b =>
                {
                    var numberOfSports = await _db.Set<Sport>().CountAsync();
                    return numberOfSportsOnTicket >= numberOfSports;
                })
                .Apply();

            appliedBonuses.ForEach(b => _db.Add(b));
            _db.Update(ticket);
            await _db.SaveChangesAsync();
        }
    }
}
