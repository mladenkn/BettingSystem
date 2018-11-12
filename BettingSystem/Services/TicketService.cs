using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BettingSystem.Models;
using BettingSystem.Requests;

namespace BettingSystem.Services
{
    public interface ITicketService
    {
        Task Handle(CommitTicketRequest request);
        Task<IReadOnlyCollection<Ticket>> GetUsersTickets(string userId);
    }

    public class TicketService : ITicketService
    {
        public Task Handle(CommitTicketRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<Ticket>> GetUsersTickets(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
