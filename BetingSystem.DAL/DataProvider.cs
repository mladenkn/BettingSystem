using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BetingSystem.DTO;
using BetingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BetingSystem.DAL
{
    public class DataProvider : IDataProvider
    {
        private readonly BetingSystemDbContext _db;

        public DataProvider(BetingSystemDbContext db)
        {
            _db = db;
        }

        public async Task<IReadOnlyCollection<TicketDto>> GetUsersTickets(string userId)
        {
            // Should maybe write SQL here, or keep tickets in document store DB

            var tickets = await _db.Set<Ticket>()
                .Include(t => t.BetedPairs)
                    .ThenInclude(t => t.BetablePair)
                        .ThenInclude(p => p.Team1)
                .Include(t => t.BetedPairs)
                    .ThenInclude(t => t.BetablePair)
                         .ThenInclude(p => p.Team2)
                .ProjectTo<TicketDto>()
                .ToListAsync();
            return tickets;
        }
    }
}
