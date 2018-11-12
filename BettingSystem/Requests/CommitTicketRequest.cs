using System.Collections.Generic;
using BettingSystem.Models;

namespace BettingSystem.Requests
{
    public class CommitTicketRequest
    {
        public double Stake { get; set; }
        public IReadOnlyCollection<BettedPair> BettedPairs { get; set; }

        public class BettedPair
        {
            public int BettedPairId { get; set; }
            public BettingType BettedType { get; set; }
        }
    }
}
