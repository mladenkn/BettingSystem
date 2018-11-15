using System.Collections.Generic;
using System.Threading.Tasks;
using BetingSystem.DTO;
using BetingSystem.Requests;
using BetingSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace BetingSystem.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost]
        public Task Post([FromBody] CommitTicketRequest request) => _ticketService.Handle(request);

        public Task<IReadOnlyCollection<TicketDto>> Get(string userId) => _ticketService.GetUsersTickets(userId);
    }
}
