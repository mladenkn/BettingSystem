using System.Collections.Generic;

namespace BetingSystem.Models
{
    public interface ITicketBonus
    {
    }

    public interface IQuotaIncreasingBonus : ITicketBonus
    {
        decimal IncreasesQuotaBy { get; set; }
    }

    public class VariousSportsBonus : IQuotaIncreasingBonus
    {
        public int RequiredNumberOfDifferentSports { get; set; }
        public decimal IncreasesQuotaBy { get; set; }
    }

    public class AllSportsBonus : IQuotaIncreasingBonus
    {
        public decimal IncreasesQuotaBy { get; set; }
    }

    public class AppliedBonus
    {
        public int TicketId { get; set; }
        public string BonusName { get; set; }
    }
}
