using System.Collections.Generic;
using System.Linq;
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

            var variousSportsBonus = new VariousSportsBonus {IncreasesQuotaBy = 4, RequiredNumberOfDifferentSports = 3, IsActive = true};
            IEnumerable<ITicketBonus> bonuses = new[] {variousSportsBonus };
            var dataProvider = new Mock<IDataProvider>();
            dataProvider.Setup(r => r.AllActiveBonuses()).Returns(Task.FromResult(bonuses));

            var sportId = 1;
            var betedPairs = CollectionUtils.Generate(() => data.BetedPair(sportId++), 3);

            var quotaWithoutBonus = 2;
            var ticket = new Ticket { Quota = quotaWithoutBonus, Id = 4, BetedPairs = betedPairs };
            db.Add(ticket);
            await db.SaveChangesAsync();

            var bonusService = new BonusService(new BonusApplier(new UnitOfWork(db)), dataProvider.Object);

            await bonusService.ApplyBonuses(ticket);

            var appliedBonus = db.AppliedBonuses.Single();
            appliedBonus.TicketId.Should().Be(ticket.Id);
            appliedBonus.BonusName.Should().Be(variousSportsBonus.GetName());

            ticket.Quota.Should().Be(variousSportsBonus.IncreasesQuotaBy + quotaWithoutBonus);
        }
    }
}
