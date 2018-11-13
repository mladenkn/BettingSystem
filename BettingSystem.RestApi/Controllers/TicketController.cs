using System.Threading.Tasks;
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
        public async Task Post([FromBody] CommitTicketRequest request)
        {
            var userId = ""; // TODO
            await _ticketService.Handle(request, userId);
        }
    }
}
