using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BetingSystem.Models;
using Utilities;

namespace BetingSystem.Services
{
    public interface IBonusApplier
    {
        Task Apply();
        IBonusApplier ApplyAdditionalFor<TBonus>(Action<Ticket, TBonus> apply);
        IBonusApplier Use(Ticket ticket);
        IBonusApplier Use(IEnumerable<ITicketBonus> bonuses);
        IBonusApplier VerifyForBonus<TBonus>(Func<TBonus, bool> shouldGrant);
        IBonusApplier VerifyForBonus<TBonus>(Func<TBonus, Task<bool>> shouldApply);
    }

    public class BonusApplier : IBonusApplier
    {
        private readonly IUnitOfWork _unitOfWork;
        private Ticket _ticket;
        private readonly IDictionary<Type, Func<ITicketBonus, Task<bool>>> _verifyers =
            new Dictionary<Type, Func<ITicketBonus, Task<bool>>>();
        private readonly ICollection<Action<ITicketBonus>> _appliers = new List<Action<ITicketBonus>>();
        private IEnumerable<ITicketBonus> _bonuses;

        public BonusApplier(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IBonusApplier Use(IEnumerable<ITicketBonus> bonuses)
        {
            _bonuses = bonuses;
            return this;
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
            foreach (var bonus in _bonuses)
            {
                var shouldGrant = _verifyers[bonus.GetType()];
                if (await shouldGrant(bonus))
                {
                    _unitOfWork.Add(new AppliedBonus { BonusName = bonus.GetName(), TicketId = _ticket.Id });
                    _appliers.ForEach(a => a(bonus));
                    _unitOfWork.Update(_ticket);
                    await _unitOfWork.SaveChanges();
                }
            }
        }
    }
}