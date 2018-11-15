using System.Collections.Generic;
using BetingSystem.Models;

namespace BetingSystem.DTO
{
    public class TicketDto
    {
        public int Id { get; set; }
        public double Stake { get; set; }
        public double Quota { get; set; }
        public IReadOnlyCollection<Pair> Pairs { get; set; }

        public class Pair
        {
            public string Team1 { get; set; }
            public string Team2 { get; set; }
            public BetingType BetedType { get; set; }
        }
    }
}
