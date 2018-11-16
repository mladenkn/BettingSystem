using System.Linq;
using System.Threading.Tasks;
using BetingSystem.Models;
using Microsoft.EntityFrameworkCore;
using Utilities;

namespace BetingSystem.DevelopmentUtilities
{
    public static class DbSeeder
    {
        public static async Task Seed(DbContext db, ITicketBonusesRepository bonusesRepo)
        {
            var ticketId = 1;

            var ticket = new Ticket
            {
                Id = ticketId,
                UserId = "mladen",
                BetedPairs = new[]
                {
                    new BetedPair
                    {
                        TicketId = ticketId,
                        BetablePairId = 1,
                        BetablePair = new BetablePair
                        {
                            Id = 1,
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
                        TicketId = ticketId,
                        BetablePairId = 2,
                        BetablePair = new BetablePair
                        {
                            Id = 2,
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

            ticket.Quota = ticket.BetedPairs.Select(p => p.Quota()).Product();

            db.Add(ticket);
            db.AddRange(ticket.BetedPairs.Select(p => p.BetablePair));
            db.AddRange(ticket.BetedPairs);

            await db.SaveChangesAsync();
        }

        public static Team NewTeam(string name, string sportName)
        {
            return new Team
            {
                Name = name,
                Sport = new Sport { Name = sportName }
            };
        }
    }
}
