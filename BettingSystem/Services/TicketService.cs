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
        public TicketService(DAL.IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task Handle(CommitTicketRequest request, string userId)
        {
            var pairsToBetIds = request.BetingPairs.Select(p => p.BetedPairId);

            var betablePairs = await UnitOfWork.BetablePairs.GenericQuery()
                .Where(p => pairsToBetIds.Contains(p.Id))
                .ToListAsync();

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
