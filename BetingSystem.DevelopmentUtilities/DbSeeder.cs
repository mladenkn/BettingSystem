using System.Linq;
using System.Threading.Tasks;
using BetingSystem.Models;
using Microsoft.EntityFrameworkCore;
using Utilities;

namespace BetingSystem.DevelopmentUtilities
{
    public static class DbSeeder
    {
        public static async Task Seed(DbContext db)
        {
            var user = new User {UserId = "mladen"};
            var wallet = new UserWallet {Currency = "HRK", MoneyAmmount = 500, UserId = user.UserId};

            db.AddRange(user, wallet);

            var hajduk = NewFootballTeam("Hajduk");
            var dinamo = NewFootballTeam("Dinamo");
            var barca = NewFootballTeam("Barcelona");
            var arsenal = NewFootballTeam("Arsenal");

            db.AddRange(hajduk, dinamo, barca, arsenal);

            var hajdukVsDinamo = new BetablePair
            {
                Id = 1,
                Team1 = hajduk,
                Team2 = dinamo,
                Team1WinQuota = 2,
                Team2WinQuota = 3,
                DrawQuota = 2.5m
            };

            var barcaVsArsenal = new BetablePair
            {
                Id = 2,
                Team1 = barca,
                Team2 = arsenal,
                Team1WinQuota = 2,
                Team2WinQuota = 3.1m,
                DrawQuota = 2.4m
            };

            db.AddRange(hajdukVsDinamo, barcaVsArsenal);

            var ticket = new Ticket
            {
                UserId = user.UserId,
                BetedPairs = new []
                {
                    new BetedPair
                    {
                        BetablePair = hajdukVsDinamo,
                        BetablePairId = hajdukVsDinamo.Id,
                        BetedType = BetingType.Draw,
                    },
                    new BetedPair
                    {
                        BetablePair = barcaVsArsenal,
                        BetablePairId = barcaVsArsenal.Id,
                        BetedType = BetingType.Team1Win,
                    }
                }
            };

            ticket.Quota = ticket.BetedPairs.Select(p => p.GetQuota()).Product();

            db.Add(ticket);

            await db.SaveChangesAsync();
        }

        public static Team NewFootballTeam(string name)
        {
            return new Team
            {
                Name = name,
                Sport = new Sport { Name = "Football" }
            };
        }
    }
}
