using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BetingSystem.Models;
using Microsoft.EntityFrameworkCore;
using Utilities;

namespace BetingSystem.Services
{
    public interface IBonusApplier
    {
        Task Apply();
        IBonusApplier ApplyAdditionalFor<TBonus>(Action<Ticket, TBonus> apply);
        IBonusApplier Use(Ticket ticket);
        IBonusApplier VerifyForBonus<TBonus>(Func<TBonus, bool> shouldGrant);
        IBonusApplier VerifyForBonus<TBonus>(Func<TBonus, Task<bool>> shouldApply);
    }

    public class BonusApplier : IBonusApplier
    {
        private readonly DbContext _db;
        private Ticket _ticket;
        private readonly IDictionary<Type, Func<ITicketBonus, Task<bool>>> _verifyers =
            new Dictionary<Type, Func<ITicketBonus, Task<bool>>>();
        private readonly ICollection<Action<ITicketBonus>> _appliers = new List<Action<ITicketBonus>>();
        private readonly ITicketBonusesRepository _bonusesRepo;

        public BonusApplier(ITicketBonusesRepository bonusesRepo, DbContext db)
        {
            _bonusesRepo = bonusesRepo;
            _db = db;
        }

        public IBonusApplier Use(Ticket ticket)
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
            var activeBonuses = await _bonusesRepo.AllActive();

            foreach (var bonus in activeBonuses)
            {
                var shouldGrant = _verifyers[bonus.GetType()];
                if (await shouldGrant(bonus))
                {
                    _db.Add(new AppliedBonus { BonusName = bonus.GetName(), TicketId = _ticket.Id });
                    _appliers.ForEach(a => a(bonus));
                    _db.Update(_ticket);
                    await _db.SaveChangesAsync();
                }
            }
        }
    }
}