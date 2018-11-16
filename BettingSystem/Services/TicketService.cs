using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BetingSystem.DTO;
using BetingSystem.Models;
using BetingSystem.Requests;
using Microsoft.EntityFrameworkCore;
using Utilities;

namespace BetingSystem.Services
{
    public interface ITicketService
    {
        Task Handle(CommitTicketRequest request);
        Task<IReadOnlyCollection<TicketDto>> GetUsersTickets();
    }

    public class TicketService : ITicketService
    {
        private readonly IBonusService _bonusService;
        private readonly DbContext _db;
        private readonly IWalletService _walletService;
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IDataProvider _dataProvider;

        public TicketService(
            IBonusService bonusService, 
            DbContext db,
            IWalletService walletService,
            ICurrentUserAccessor currentUser,
            IDataProvider dataProvider)
        {
            _bonusService = bonusService;
            _db = db;
            _walletService = walletService;
            _currentUser = currentUser;
            _dataProvider = dataProvider;
        }

        public async Task Handle(CommitTicketRequest request)
        {
            var pairsToBetIds = request.BetingPairs.Select(p => p.BetedPairId);

            var betablePairs = await _db.Set<BetablePair>()
                .Include(p => p.Team1)
                .Include(p => p.Team2)
                .Where(p => pairsToBetIds.Contains(p.Id))
                .ToArrayAsync();

            if (request.BetingPairs.Count != betablePairs.Length)
            {
                var betablePairsIds = betablePairs.Select(p => p.Id);
                var difference = pairsToBetIds.Except(betablePairsIds);
                throw new BetablePairsNotFound(difference);
            }

            var betedPairs = CreateBetedPairs(request, betablePairs).ToList();

            var ticket = new Ticket
            {
                BetedPairs = betedPairs,
                Stake = request.Stake,
                UserId = _currentUser.Id()
            };

            CalculateQuota(ticket);
            _db.Add(ticket);
            await _db.SaveChangesAsync();
            await _walletService.SubtractMoney(ticket.Stake, WalletTransaction.WalletTransactionType.TicketCommit);
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

        public Task<IReadOnlyCollection<TicketDto>> GetUsersTickets() => _dataProvider.GetUsersTickets(_currentUser.Id());
    }
}
