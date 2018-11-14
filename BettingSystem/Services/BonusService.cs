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
        private readonly IBonusApplier _bonusApplier;

        public BonusService(IUnitOfWork unitOfWork, IBonusApplier bonusApplier) : base(unitOfWork)
        {
            _bonusApplier = bonusApplier;
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
                    var numberOfSports = await UnitOfWork.Sports.GenericQuery().Select(s => s.Id).Distinct().CountAsync();
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly GetAllTicketBonuses _getAllTicketBonuses;
        private Ticket _ticket;
        private readonly IDictionary<Type, Func<ITicketBonus, Task<bool>>> _verifyers =
            new Dictionary<Type, Func<ITicketBonus, Task<bool>>>();
        private readonly ICollection<Action<ITicketBonus>> _appliers = new List<Action<ITicketBonus>>();

        public BonusApplier(IUnitOfWork unitOfWork, GetAllTicketBonuses getAllTicketBonuses)
        {
            _unitOfWork = unitOfWork;
            _getAllTicketBonuses = getAllTicketBonuses;
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
            var allBonuses = await _getAllTicketBonuses();

            foreach (var bonus in allBonuses)
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
