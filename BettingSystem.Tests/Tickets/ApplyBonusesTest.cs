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

            var variousSporstsBonus = new VariousSportsBonus {IncreasesQuotaBy = 4, RequiredNumberOfDifferentSports = 3, Name = "a"};
            IEnumerable<ITicketBonus> allBonues = new[] {variousSporstsBonus};

            var unitofWork = new Mock<IUnitOfWork>();
            unitofWork.Setup(u => u.AppliedBonuses.Insert(null));
            unitofWork.Setup(u => u.Tickets.Update(null));

            var sportId = 1;
            var betedPairs = CollectionUtils.Generate(() => data.BetedPair(sportId++), 3);

            var quotaWithoutBonus = 2;
            var ticket = new Ticket { Quota = quotaWithoutBonus, Id = 4, BetedPairs = betedPairs };
            db.Add(ticket);
            await db.SaveChangesAsync();

            var bonusService = new BonusService(unitofWork.Object, new BonusApplier(unitofWork.Object, () => Task.FromResult(allBonues), db));

            await bonusService.ApplyBonuses(ticket);

            var appliedBonus = db.AppliedBonuses.Single();
            appliedBonus.TicketId.Should().Be(ticket.Id);
            appliedBonus.BonusName.Should().Be(variousSporstsBonus.Name);

            ticket.Quota.Should().Be(variousSporstsBonus.IncreasesQuotaBy + quotaWithoutBonus);
        }
    }
}
