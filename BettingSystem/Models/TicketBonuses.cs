namespace BettingSystem.Models
{
    public abstract class TicketBonus
    {
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
