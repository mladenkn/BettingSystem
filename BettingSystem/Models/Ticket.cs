using System.Collections.Generic;

namespace BetingSystem.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public IReadOnlyCollection<BetedPair> BetedPairs { get; set; }

        public double Stake { get; set; }

        public IReadOnlyCollection<TicketBonus> Bonuses { get; set; }
    }

    public class BetedPair
    {
        public int Id { get; set; }

        public int BetingPairId { get; set; }
        public BetingPair BetingPair { get; set; }

        public BetingType BetedType { get; set; }
    }

    public enum BetingType { Team1Win, Team2Win, Draw }
}
