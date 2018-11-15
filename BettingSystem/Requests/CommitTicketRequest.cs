using System.Collections.Generic;
using BetingSystem.Models;

namespace BetingSystem.Requests
{
    public class CommitTicketRequest
    {
        public decimal Stake { get; set; }
        public IReadOnlyCollection<BetingPair> BetingPairs { get; set; }

        public class BetingPair
        {
            public int BetedPairId { get; set; }
            public BetingType BetingType { get; set; }
        }
    }
}
