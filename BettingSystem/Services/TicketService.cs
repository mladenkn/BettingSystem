using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationKernel;
using BetingSystem.Models;
using BetingSystem.Repositories;
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

    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _tickets;
        private readonly IBettingPairsRepository _bettingPairs;
        private readonly IDatabase _db;

        public TicketService(ITicketRepository tickets, IBettingPairsRepository bettingPairs, IDatabase db)
        {
            _tickets = tickets;
            _bettingPairs = bettingPairs;
            _db = db;
        }

        public async Task Handle(CommitTicketRequest request, string userId)
        {
            var pairsToBetIds = request.BetedPairs.Select(p => p.BetedPairId);

            var betablePairs = await _bettingPairs.GenericQuery()
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

            _tickets.Insert(ticket);
            await _db.SaveChanges();
        }

        public static void CalculateQuota(Ticket ticket)
        {
            ticket.Quota = ticket.BetedPairs.Select(p => p.Quota()).Product();
        }

        private static IEnumerable<BetedPair> CreateBetedPairs(CommitTicketRequest request, IEnumerable<BetablePair> betablePairs)
        {
            BetingType GetSelectedTypeOfPair(int pairId) =>
                request.BetedPairs.First(p => p.BetedPairId == pairId).BetedType;

            return betablePairs.Select(p => new BetedPair
            {
                BetedType = GetSelectedTypeOfPair(p.Id),
                BetablePair = p,
                BetingPairId = p.Id
            });
        }

        public Task<IReadOnlyCollection<Ticket>> GetUsersTickets(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
