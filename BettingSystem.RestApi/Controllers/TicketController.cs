using System;
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
        public async Task<IActionResult> Post([FromBody] CommitTicketRequest request)
        {
            try
            {
                await _ticketService.Handle(request);
            }
            catch (BetablePairsNotFound e)
            {
                var message = $"Pairs with the ids: {e.NotFoundPairsIds} have not been found";
                return BadRequest(message);
            }
            return Ok();
        }

        [HttpGet]
        public Task<IReadOnlyCollection<TicketDto>> Get() => _ticketService.GetUsersTickets();
    }
}
