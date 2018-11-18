﻿using System.Collections.Generic;
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
        private readonly ITicketBonusesRepository _ticketBonuses;

        public DataProvider(BetingSystemDbContext db, IConfigurationProvider mapperConfig, ITicketBonusesRepository ticketBonuses)
        {
            _db = db;
            _mapperConfig = mapperConfig;
            _ticketBonuses = ticketBonuses;
        }

        public async Task<IReadOnlyCollection<TicketDto>> GetUsersTickets(string userId)
        {
            // Should maybe write SQL here, or keep tickets in document store DB

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

        public Task<IEnumerable<ITicketBonus>> GetActiveBonuses() => _ticketBonuses.GetActiveBonuses();
    }
}
