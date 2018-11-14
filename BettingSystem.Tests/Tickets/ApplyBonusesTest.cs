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

            var variousSporstsBonus = new VariousSportsBonus {IncreasesQuotaByN = 4, RequiredNumberOfDifferentSports = 3};
            IEnumerable<ITicketBonus> allBonues = new[] {variousSporstsBonus};

            var unitofWork = new Mock<IUnitOfWork>();
            unitofWork.Setup(u => u.AppliedBonuses.Insert(null));
            unitofWork.Setup(u => u.Tickets.Update(null));

            var sportId = 1;
            var betedPairs = CollectionUtils.Generate(() => data.BetedPair(sportId++), 3);

            var ticket = new Ticket { Id = 4, BetedPairs = betedPairs };

            var bonusService = new BonusService(unitofWork.Object, new BonusApplier(unitofWork.Object, () => Task.FromResult(allBonues)));

            await bonusService.ApplyBonuses(ticket);

            unitofWork.Verify(u => u.AppliedBonuses.Insert(It.Is<AppliedBonus>(b =>
                b.TicketId == ticket.Id && b.BonusName == variousSporstsBonus.Name)), Times.Once());

            ticket.Quota.Should().Be(variousSporstsBonus.IncreasesQuotaByN);
        }
    }
}
