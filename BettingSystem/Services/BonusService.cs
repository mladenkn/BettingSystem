using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BetingSystem.DAL;
using BetingSystem.Models;
using Microsoft.EntityFrameworkCore;
using Utilities;

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
                .Select(p => p.BetablePair.Team1.SportId)
                .Distinct()
                .Count();

            await new BonusApplier(UnitOfWork)
                .UseTicket(ticket)
                .UseBonuses(allBonuses.All.NonNulls())
                .ApplyAdditionalFor<IQuotaIncreasingBonus>((t, b) => t.Quota += b.IncreasesQuotaByN)
                .VerifyForBonus<VariousSportsBonus>(b => numberOfSportsOnTicket >= b.RequiredNumberOfDifferentSports)
                .VerifyForBonus<AllSportsBonus>(async b =>
                {
                    var numberOfSports = await UnitOfWork.Sports.GenericQuery().Select(s => s.Id).Distinct().CountAsync();
                    return numberOfSportsOnTicket >= numberOfSports;
                })
                .Apply();
        }

        private class BonusApplier
        {
            private readonly IUnitOfWork _unitOfWork;
            private Ticket _ticket;
            private IEnumerable<ITicketBonus> _bonuses;
            private readonly IDictionary<Type, Func<ITicketBonus, Task<bool>>> _verifyers =
                    new Dictionary<Type, Func<ITicketBonus, Task<bool>>>();
            private readonly ICollection<Action<ITicketBonus>> _appliers = new List<Action<ITicketBonus>>();

            public BonusApplier(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public BonusApplier UseTicket(Ticket ticket)
            {
                _ticket = ticket;
                return this;
            }

            public BonusApplier UseBonuses(IEnumerable<ITicketBonus> bonuses)
            {
                _bonuses = bonuses;
                return this;
            }

            public BonusApplier VerifyForBonus<TBonusType>(Func<TBonusType, Task<bool>> shouldApply)
            {
                _verifyers[typeof(TBonusType)] = b => shouldApply((TBonusType) b);
                return this;
            }

            public BonusApplier VerifyForBonus<TBonusType>(Func<TBonusType, bool> shouldGrant)
            {
                _verifyers[typeof(TBonusType)] = b => Task.FromResult(shouldGrant((TBonusType)b));
                return this;
            }

            public BonusApplier ApplyAdditionalFor<TBonusType>(Action<Ticket, TBonusType> apply)
            {
                _appliers.Add(b =>
                {
                    if(b is TBonusType wantedBonus)
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
                        _unitOfWork.AppliedBonuses.Insert(new AppliedBonus { BonusName = bonus.Name, TicketId = _ticket.Id });
                        
                        _appliers.ForEach(a => a(bonus));

                        _unitOfWork.Tickets.Update(_ticket);
                        await _unitOfWork.SaveChanges();
                    }
                }
            }
        }
    }
}
