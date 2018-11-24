using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BetingSystem.Models;
using BetingSystem.Requests;
using Utilities;

namespace BetingSystem.Services
{
    public interface ITicketService
    {
        Task<TicketDto> Handle(CommitTicketRequest request);
        Task<IReadOnlyCollection<TicketDto>> GetUsersTickets();
    }

    public class TicketService : ITicketService
    {
        private readonly IBonusService _bonusService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWalletService _walletService;
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IDataProvider _dataProvider;
        private readonly IMapper _mapper;

        public TicketService(
            IBonusService bonusService, 
            IUnitOfWork unitOfWork,
            IWalletService walletService,
            ICurrentUserAccessor currentUser,
            IDataProvider dataProvider,
            IMapper mapper)
        {
            _bonusService = bonusService;
            _unitOfWork = unitOfWork;
            _walletService = walletService;
            _currentUser = currentUser;
            _dataProvider = dataProvider;
            _mapper = mapper;
        }

        public async Task<TicketDto> Handle(CommitTicketRequest request)
        {
            var pairsToBetIds = request.BetingPairs.Select(p => p.BetedPairId);

            var betablePairs = await _dataProvider.BetablePairs(pairsToBetIds);

            if (request.BetingPairs.Count != betablePairs.Count)
            {
                var betablePairsIds = betablePairs.Select(p => p.Id);
                var difference = pairsToBetIds.Except(betablePairsIds);
                throw new BetablePairsNotFound(difference);
            }

            var betedPairs = CreateBetedPairs(request, betablePairs).ToArray();

            var ticket = new Ticket
            {
                BetedPairs = betedPairs,
                Stake = request.Stake,
                UserId = _currentUser.Id()
            };

            CalculateQuota(ticket);
            await _walletService.SubtractMoney(ticket.Stake, WalletTransaction.WalletTransactionType.TicketCommit);
            _unitOfWork.Add(ticket);
            _unitOfWork.AddRange(ticket.BetedPairs);
            await _unitOfWork.SaveChanges();
            await _bonusService.ApplyBonuses(ticket);

            return _mapper.Map<TicketDto>(ticket);
        }

        public static void CalculateQuota(Ticket ticket)
        {
            ticket.Quota = ticket.BetedPairs.Select(p => p.GetQuota()).Product();
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

        public Task<IReadOnlyCollection<TicketDto>> GetUsersTickets() => _dataProvider.UsersTickets(_currentUser.Id());
    }
}
