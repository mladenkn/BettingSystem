using System.Collections.Generic;

namespace BetingSystem.Models
{
    public interface ITicketBonus
    {
        string Name { get; set; }
    }

    public interface IQuotaIncreasingBonus : ITicketBonus
    {
        double IncreasesQuotaByN { get; set; }
    }

    public class VariousSportsBonus : IQuotaIncreasingBonus
    {
        public int RequiredNumberOfDifferentSports { get; set; }
        public double IncreasesQuotaByN { get; set; }
        public string Name { get; set; }
    }

    public class AllSportsBonus : IQuotaIncreasingBonus
    {
        public double IncreasesQuotaByN { get; set; }
        public string Name { get; set; }
    }

    public class AppliedBonus
    {
        public int TicketId { get; set; }
        public string BonusName { get; set; }
    }
}
