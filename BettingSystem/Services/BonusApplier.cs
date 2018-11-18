using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BetingSystem.Models;
using Utilities;

namespace BetingSystem.Services
{
    public interface IBonusApplier
    {
        IBonusApplier ApplyAdditionalFor<TBonus>(Action<Ticket, TBonus> apply);
        IBonusApplier Use(Ticket ticket);
        IBonusApplier Use(IEnumerable<ITicketBonus> bonuses);
        IBonusApplier VerifyForBonus<TBonus>(Func<TBonus, bool> shouldGrant);
        IBonusApplier VerifyForBonus<TBonus>(Func<TBonus, Task<bool>> shouldApply);
        Task<IEnumerable<AppliedBonus>> Apply();
    }

    public class BonusApplier : IBonusApplier
    {
        private Ticket _ticket;
        private readonly IDictionary<Type, Func<ITicketBonus, Task<bool>>> _verifyers =
            new Dictionary<Type, Func<ITicketBonus, Task<bool>>>();
        private readonly ICollection<Action<Ticket, ITicketBonus>> _appliers = new List<Action<Ticket, ITicketBonus>>();
        private IEnumerable<ITicketBonus> _bonuses;

        public IBonusApplier Use(Ticket ticket)
        {
            _ticket = ticket;
            return this;
        }

        public IBonusApplier Use(IEnumerable<ITicketBonus> bonuses)
        {
            _bonuses = bonuses;
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
            _appliers.Add((t, b) =>
            {
                if (b is TBonusType wantedBonus)
                    apply(t, wantedBonus);
            });
            return this;
        }

        public async Task<IEnumerable<AppliedBonus>> Apply()
        {
            var bonuses = new List<AppliedBonus>();
            foreach (var bonus in _bonuses)
            {
                var shouldGrant = _verifyers[bonus.GetType()];
                if (await shouldGrant(bonus))
                {
                    bonuses.Add(new AppliedBonus { BonusName = bonus.GetName(), TicketId = _ticket.Id });
                    _appliers.ForEach(a => a(_ticket, bonus));
                }
            }
            return bonuses;
        }
    }
}