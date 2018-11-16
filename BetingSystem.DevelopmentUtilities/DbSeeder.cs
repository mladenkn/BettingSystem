using System.Threading.Tasks;
using BetingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BetingSystem.DevelopmentUtilities
{
    public static class DbSeeder
    {
        public static async Task Seed(DbContext db, TicketBonuses bonuses)
        {
            bonuses.VariousSportsBonus.IncreasesQuotaBy = 2;
            bonuses.VariousSportsBonus.RequiredNumberOfDifferentSports = 3;
            bonuses.VariousSportsBonus.IsActive = true;

            var ticket = new Ticket
            {
                BetedPairs = new[]
                {
                    new BetedPair
                    {
                        BetablePair = new BetablePair
                        {
                            Team1 = NewTeam("Hajduk", "Nogomet"),
                            Team2 = NewTeam("Barcelona", "Nogomet"),
                            Team1WinQuota = 5,
                            Team2WinQuota = 2,
                            DrawQuota = 2.5m
                        },
                        BetedType = BetingType.Team1Win,
                    },
                    new BetedPair
                    {
                        BetablePair = new BetablePair
                        {
                            Team1 = NewTeam("Dinamo", "Nogomet"),
                            Team2 = NewTeam("Arsenal", "Nogomet"),
                            Team1WinQuota = 4.8m,
                            Team2WinQuota = 2.1m,
                            DrawQuota = 2.5m
                        },
                        BetedType = BetingType.Draw,
                    }
                }
            };

            db.Add(ticket);
            await db.SaveChangesAsync();
        }

        public static Team NewTeam(string name, string sportName)
        {
            return new Team
            {
                Name = name,
                Sport = new Sport
                {
                    Name = sportName
                }
            };
        }
    }
}
