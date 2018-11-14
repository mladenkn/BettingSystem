using BetingSystem.Models;

namespace BetingSystem.Tests
{
    public class DataFactory
    {
        public BetablePair BetablePair(double team1WinQuota, double team2WinQuota, double drawQuota, Team team1, Team team2)
        {
            return new BetablePair
            {
                Team1 = team1,
                Team2 = team2,
                Team1WinQuota = team1WinQuota,
                Team2WinQuota = team2WinQuota,
                DrawQuota = drawQuota
            };
        }
    }
}
