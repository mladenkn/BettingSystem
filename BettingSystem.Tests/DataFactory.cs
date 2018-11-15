using BetingSystem.Models;

namespace BetingSystem.Tests
{
    public class DataFactory
    {
        public BetablePair BetablePair(decimal team1WinQuota, decimal team2WinQuota, decimal drawQuota, Team team1, Team team2)
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

        public BetedPair BetedPair(BetablePair betablePair, BetingType type)
        {
            return new BetedPair
            {
                BetablePair = betablePair,
                BetablePairId = betablePair.Id,
                BetedType = type
            };
        }

        public BetedPair BetedPair(int sportId)
        {
            return new BetedPair
            {
                BetablePair = new BetablePair
                {
                    Team1 = new Team { SportId = sportId },
                    Team2 = new Team { SportId = sportId },
                }
            };
        }
    }
}
