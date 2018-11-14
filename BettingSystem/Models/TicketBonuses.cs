namespace BetingSystem.Models
{
    public class TicketBonus
    {
        public string Name { get; set; }
    }

    public class VariousSportsBonus : TicketBonus
    {
        public int IncreasesQuotaByN { get; set; }
        public int RequiredNumberOfDifferentSports { get; set; }
    }

    public class AllSportsBonus : TicketBonus
    {
        public int IncreasesQuotaByN { get; set; }
    }

    public class AppliedBonus
    {
        public int TicketId { get; set; }
        public string BonusName { get; set; }
    }

    public class Bonuses
    {
        public VariousSportsBonus VariousSportsBonus { get; set; }
        public AllSportsBonus AllSportsBonus { get; set; }
    }
}
