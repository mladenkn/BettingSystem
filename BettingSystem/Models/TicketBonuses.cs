namespace BetingSystem.Models
{
    public class TicketBonus
    {
        public int Id { get; set; }
    }

    public class VariousSportsBonus : TicketBonus
    {
        public int IncreasesQuotaByN { get; set; }
        public int TicketMustContainNPairsWithDifferentSports { get; set; }
    }

    public class AllSportsBonus : TicketBonus
    {
        public int IncreasesQuotaByN { get; set; }
    }
}
