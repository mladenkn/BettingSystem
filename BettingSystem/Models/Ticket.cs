using System.Collections.Generic;

namespace BettingSystem.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public IReadOnlyCollection<BettedPair> BettedPairs { get; set; }
        public double Stake { get; set; }
        public IReadOnlyCollection<TicketBonus> Bonuses { get; set; }
    }

    public class BettedPair
    {
        public int Id { get; set; }
        public BettingPair BettingPair { get; set; }
        public int BettingPairId { get; set; }
        public BettingType BettedType { get; set; }
    }
}
