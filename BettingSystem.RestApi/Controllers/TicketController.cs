using System.Collections.Generic;
using System.Threading.Tasks;
using BetingSystem.Models;
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
        private readonly ISafeRunner _safeRunner;

        public TicketController(ITicketService ticketService, ISafeRunner safeRunner)
        {
            _ticketService = ticketService;
            _safeRunner = safeRunner;
        }

        [HttpPost]
        public Task<IActionResult> Post([FromBody] CommitTicketRequest request) => _safeRunner.Run(() => _ticketService.Handle(request), Ok);

        [HttpGet]
        public Task<IActionResult> Get() => _safeRunner.Run(() => _ticketService.GetUsersTickets(), Ok);
    }
}
