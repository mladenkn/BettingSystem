namespace BetingSystem.Models
{
    public class BetablePair
    {
        public int Id { get; set; }

        public int Team1Id { get; set; }
        public Team Team1 { get; set; }

        public int Team2Id { get; set; }
        public Team Team2 { get; set; }

        public decimal Team1WinQuota { get; set; }
        public decimal Team2WinQuota { get; set; }
        public decimal DrawQuota { get; set; }
    }
}
