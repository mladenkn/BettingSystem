namespace BetingSystem.Models
{
    public class BetingPair
    {
        public int Id { get; set; }

        public int Team1Id { get; set; }
        public Team Team1 { get; set; }

        public int Team2Id { get; set; }
        public Team Team2 { get; set; }

        public double Team1WinQuota { get; set; }
        public double Team2WinQuota { get; set; }
        public double DrawQuota { get; set; }
    }
}
