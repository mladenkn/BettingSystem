using System.Collections.Generic;

namespace BetingSystem.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public IReadOnlyCollection<BetedPair> BetedPairs { get; set; }

        public decimal Quota { get; set; }

        public decimal Stake { get; set; }

        public IReadOnlyCollection<AppliedBonus> Bonuses { get; set; }
    }

    public class BetedPair
    {
        public int Id { get; set; }

        public int TicketId { get; set; }

        public int BetablePairId { get; set; }
        public BetablePair BetablePair { get; set; }

        public BetingType BetedType { get; set; }
    }

    public enum BetingType { Team1Win = 1, Team2Win = 2, Draw = 3 }

    public class TicketDto
    {
        public int Id { get; set; }
        public decimal Stake { get; set; }
        public decimal Quota { get; set; }
        public IReadOnlyCollection<Pair> Pairs { get; set; }

        public class Pair
        {
            public string Team1 { get; set; }
            public string Team2 { get; set; }
            public BetingType BetedType { get; set; }
        }
    }
}
