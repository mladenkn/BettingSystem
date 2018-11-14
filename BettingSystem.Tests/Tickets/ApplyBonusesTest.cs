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

            var bonuses = new Bonuses
            {
                VariousSportsBonus = new VariousSportsBonus { IncreasesQuotaByN = 4, RequiredNumberOfDifferentSports = 3}
            };

            var bonusesRepository = new Mock<IBonusRepository>();
            bonusesRepository.Setup(r => r.GetAll()).Returns(Task.FromResult(bonuses));
            
            var unitofWork = new Mock<IUnitOfWork>();
            unitofWork.Setup(u => u.AppliedBonuses.Insert(null));
            unitofWork.Setup(u => u.Tickets.Update(null));

            var sportId = 1;
            var betedPairs = CollectionUtils.Generate(() => data.BetedPair(sportId++), 3);

            var ticket = new Ticket { Id = 4, BetedPairs = betedPairs };

            var bonusService = new BonusService(unitofWork.Object, bonusesRepository.Object);

            await bonusService.ApplyBonuses(ticket);

            unitofWork.Verify(u => u.AppliedBonuses.Insert(It.Is<AppliedBonus>(b =>
                b.TicketId == ticket.Id && b.BonusName == bonuses.VariousSportsBonus.Name)), Times.Once());

            ticket.Quota.Should().Be(bonuses.VariousSportsBonus.IncreasesQuotaByN);
        }
    }
}
