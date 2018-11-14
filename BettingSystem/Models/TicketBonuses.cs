using System.Collections.Generic;

namespace BetingSystem.Models
{
    public interface ITicketBonus
    {
        string Name { get; set; }
    }

    public interface IQuotaIncreasingBonus : ITicketBonus
    {
        double IncreasesQuotaBy { get; set; }
    }

    public class VariousSportsBonus : IQuotaIncreasingBonus
    {
        public int RequiredNumberOfDifferentSports { get; set; }
        public double IncreasesQuotaBy { get; set; }
        public string Name { get; set; }
    }

    public class AllSportsBonus : IQuotaIncreasingBonus
    {
        public double IncreasesQuotaBy { get; set; }
        public string Name { get; set; }
    }

    public class AppliedBonus
    {
        public int TicketId { get; set; }
        public string BonusName { get; set; }
    }
}
