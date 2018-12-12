using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BetingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BetingSystem.DAL
{
    public class DataProvider : IDataProvider
    {
        private readonly BetingSystemDbContext _db;
        private readonly IConfigurationProvider _mapperConfig;
        private readonly ITicketBonusesRepository _ticketBonusesRepository;

        public DataProvider(BetingSystemDbContext db, IConfigurationProvider mapperConfig, ITicketBonusesRepository ticketBonusesRepository)
        {
            _db = db;
            _mapperConfig = mapperConfig;
            _ticketBonusesRepository = ticketBonusesRepository;
        }

        public async Task<IReadOnlyCollection<TicketDto>> UsersTickets(string userId)
        {
            // Should maybe write SQL here, or keep data in document store DB
            // Should use query specification pattern

            var tickets = await _db.Set<Ticket>()
                .Where(t => t.UserId == userId)
                .Include(t => t.BetedPairs)
                    .ThenInclude(t => t.BetablePair)
                        .ThenInclude(p => p.Team1)
                .Include(t => t.BetedPairs)
                    .ThenInclude(t => t.BetablePair)
                         .ThenInclude(p => p.Team2)
                .ProjectTo<TicketDto>(_mapperConfig)
                .ToArrayAsync();

            return tickets;
        }

        public Task<int> SportsCount() => _db.Sports.CountAsync();

        public Task<IEnumerable<ITicketBonus>> AllActiveBonuses() => _ticketBonusesRepository.AllActive();

        public Task<UserWallet> UsersWallet(string userId) => _db.UserWallets.FirstOrDefaultAsync(w => w.UserId == userId);

        public async Task<IReadOnlyCollection<BetablePair>> BetablePairs(IEnumerable<int> ids)
        {
            // TODO: use query specs pattern
            return await _db.Set<BetablePair>()
                .Include(p => p.Team1)
                .Include(p => p.Team2)
                .Where(p => ids.Contains(p.Id))
                .ToArrayAsync();;
        }
    }
}
