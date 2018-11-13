using System.Collections.Generic;
using BetingSystem.Models;

namespace BetingSystem.Requests
{
    public class CommitTicketRequest
    {
        public double Stake { get; set; }
        public IReadOnlyCollection<BetedPair> BetedPairs { get; set; }

        public class BetedPair
        {
            public int BetedPairId { get; set; }
            public BetingType BetedType { get; set; }
        }
    }
}
