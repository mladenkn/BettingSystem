namespace BetingSystem.Models
{
    public interface ITicketBonus
    {
        bool IsActive { get; }
    }

    public interface IQuotaIncreasingBonus : ITicketBonus
    {
        decimal IncreasesQuotaBy { get; set; }
    }

    public class VariousSportsBonus : IQuotaIncreasingBonus
    {
        public int RequiredNumberOfDifferentSports { get; set; }
        public decimal IncreasesQuotaBy { get; set; }
        public bool IsActive { get; set; }
    }

    public class AllSportsBonus : IQuotaIncreasingBonus
    {
        public decimal IncreasesQuotaBy { get; set; }
        public bool IsActive { get; set; }
    }

    public class AppliedBonus
    {
        public int TicketId { get; set; }
        public string BonusName { get; set; }
    }
}
