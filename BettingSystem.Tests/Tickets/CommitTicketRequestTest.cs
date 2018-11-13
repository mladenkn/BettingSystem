using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetingSystem.Models;
using BetingSystem.Requests;

namespace BetingSystem.Tests.Tickets
{
    public class CommitTicketRequestTest
    {
        public async Task Test1()
        {
            var request = new CommitTicketRequest
            {
                Stake = 10,

            };
        }
    }
}
