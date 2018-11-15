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

    public enum BetingType { Team1Win, Team2Win, Draw }
}
