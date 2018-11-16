using System;
using System.Collections.Generic;
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
                .UseTicket(ticket)
                .ApplyAdditionalFor<IQuotaIncreasingBonus>((t, b) => t.Quota += b.IncreasesQuotaBy)
                .VerifyForBonus<VariousSportsBonus>(b => numberOfSportsOnTicket >= b.RequiredNumberOfDifferentSports)
                .VerifyForBonus<AllSportsBonus>(async b =>
                {
                    var numberOfSports = await _db.Set<Sport>().Select(s => s.Id).Distinct().CountAsync();
                    return numberOfSportsOnTicket >= numberOfSports;
                })
                .Apply();
        }
    }

    public interface IBonusApplier
    {
        Task Apply();
        IBonusApplier ApplyAdditionalFor<TBonus>(Action<Ticket, TBonus> apply);
        IBonusApplier UseTicket(Ticket ticket);
        IBonusApplier VerifyForBonus<TBonus>(Func<TBonus, bool> shouldGrant);
        IBonusApplier VerifyForBonus<TBonus>(Func<TBonus, Task<bool>> shouldApply);
    }

    public delegate Task<IEnumerable<ITicketBonus>> GetAllTicketBonuses();

    public class BonusApplier : IBonusApplier
    {
        private readonly ITicketBonusRepository _bonusRepository;
        private readonly DbContext _db;
        private Ticket _ticket;
        private readonly IDictionary<Type, Func<ITicketBonus, Task<bool>>> _verifyers =
            new Dictionary<Type, Func<ITicketBonus, Task<bool>>>();
        private readonly ICollection<Action<ITicketBonus>> _appliers = new List<Action<ITicketBonus>>();

        public BonusApplier(ITicketBonusRepository bonusRepository, DbContext db)
        {
            _bonusRepository = bonusRepository;
            _db = db;
        }

        public IBonusApplier UseTicket(Ticket ticket)
        {
            _ticket = ticket;
            return this;
        }

        public IBonusApplier VerifyForBonus<TBonus>(Func<TBonus, Task<bool>> shouldApply)
        {
            _verifyers[typeof(TBonus)] = b => shouldApply((TBonus)b);
            return this;
        }

        public IBonusApplier VerifyForBonus<TBonus>(Func<TBonus, bool> shouldGrant)
        {
            return VerifyForBonus<TBonus>(b => Task.FromResult(shouldGrant(b)));
        }

        public IBonusApplier ApplyAdditionalFor<TBonusType>(Action<Ticket, TBonusType> apply)
        {
            _appliers.Add(b =>
            {
                if (b is TBonusType wantedBonus)
                    apply(_ticket, wantedBonus);
            });
            return this;
        }

        public async Task Apply()
        {
            var allBonuses = await _bonusRepository.GetAll();

            foreach (var bonus in allBonuses)
            {
                var shouldGrant = _verifyers[bonus.GetType()];
                if (await shouldGrant(bonus))
                {
                    _db.Add(new AppliedBonus { BonusName = bonus.Name(), TicketId = _ticket.Id });
                    _appliers.ForEach(a => a(bonus));
                    _db.Update(_ticket);
                    await _db.SaveChangesAsync();
                }
            }
        }
    }
}
