using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationKernel;
using BetingSystem.Models;
using BetingSystem.Requests;
using Microsoft.EntityFrameworkCore;
using Utilities;

namespace BetingSystem.Services
{
    public interface ITicketService
    {
        Task Handle(CommitTicketRequest request, string userId);
        Task<IReadOnlyCollection<Ticket>> GetUsersTickets(string userId);
    }

    public class TicketService : AbstractService, ITicketService
    {
        private readonly IBonusService _bonusService;
        public TicketService(DAL.IUnitOfWork unitOfWork, IBonusService bonusService) : base(unitOfWork)
        {
            _bonusService = bonusService;
        }

        public async Task Handle(CommitTicketRequest request, string userId)
        {
            var pairsToBetIds = request.BetingPairs.Select(p => p.BetedPairId);

            var betablePairs = await UnitOfWork.BetablePairs.GenericQuery()
                .Include(p => p.Team1)
                .Include(p => p.Team2)
                .Where(p => pairsToBetIds.Contains(p.Id))
                .ToArrayAsync();

            var betedPairs = CreateBetedPairs(request, betablePairs).ToList();

            var ticket = new Ticket
            {
                BetedPairs = betedPairs,
                Stake = request.Stake,
                UserId = userId
            };

            CalculateQuota(ticket);
            UnitOfWork.Tickets.Insert(ticket);
            await UnitOfWork.SaveChanges();
            await _bonusService.ApplyBonuses(ticket);
        }

        public static void CalculateQuota(Ticket ticket)
        {
            ticket.Quota = ticket.BetedPairs.Select(p => p.Quota()).Product();
        }

        private static IEnumerable<BetedPair> CreateBetedPairs(CommitTicketRequest request, IEnumerable<BetablePair> betablePairs)
        {
            BetingType GetSelectedTypeOfPair(int pairId) =>
                request.BetingPairs.First(p => p.BetedPairId == pairId).BetingType;

            return betablePairs.Select(p => new BetedPair
            {
                BetedType = GetSelectedTypeOfPair(p.Id),
                BetablePair = p,
                BetablePairId = p.Id
            });
        }

        public Task<IReadOnlyCollection<Ticket>> GetUsersTickets(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
