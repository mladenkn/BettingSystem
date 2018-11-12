using System.Collections.Generic;

namespace BettingSystem.Models
{
    public class BettingPair
    {
        public int Id { get; set; }
        public Team Team1 { get; set; }
        public Team Team2 { get; set; }
        public IReadOnlyCollection<TypeAndQuota> TypesAndQuotas { get; set; }

        public class TypeAndQuota
        {
            public int PairId { get; set; }
            public BettingType Type { get; set; }
            public double Quota { get; set; }
        }
    }

    public enum BettingType { One, Two, X }
}
