using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetingSystem.DAL;
using BetingSystem.Models;
using BetingSystem.Services;
using FluentAssertions;
using Moq;
using Utilities;
using Xunit;

namespace BetingSystem.Tests.Tickets
{
    public class ApplyBonusesTest
    {
        [Fact]
        public async Task Run()
        {
            var data = new DataFactory();
            var db = TestServicesFactory.DbContext();
            
            var bonuses = new TicketBonuses
            {
                VariousSportsBonus = new VariousSportsBonus { IncreasesQuotaBy = 4, RequiredNumberOfDifferentSports = 3, IsActive = true },
                AllSportsBonus = new AllSportsBonus()
            };
            var bonusRepo = new Mock<ITicketBonusesAccessor>();
            bonusRepo.Setup(r => r.Value).Returns(bonuses);

            var sportId = 1;
            var betedPairs = CollectionUtils.Generate(() => data.BetedPair(sportId++), 3);

            var quotaWithoutBonus = 2;
            var ticket = new Ticket { Quota = quotaWithoutBonus, Id = 4, BetedPairs = betedPairs };
            db.Add(ticket);
            await db.SaveChangesAsync();

            var bonusService = new BonusService(new BonusApplier(bonusRepo.Object, db), db);

            await bonusService.ApplyBonuses(ticket);

            var appliedBonus = db.AppliedBonuses.Single();
            appliedBonus.TicketId.Should().Be(ticket.Id);
            appliedBonus.BonusName.Should().Be(bonuses.VariousSportsBonus.Name());

            ticket.Quota.Should().Be(bonuses.VariousSportsBonus.IncreasesQuotaBy + quotaWithoutBonus);
        }
    }
}
