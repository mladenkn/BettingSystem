using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationKernel;
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

            var variousSportsBonus = new VariousSportsBonus {IncreasesQuotaBy = 4, RequiredNumberOfDifferentSports = 3, IsActive = true};
            IEnumerable<ITicketBonus> bonuses = new[] {variousSportsBonus};

            var transactionMock = new TransactionMock();
            var dbMock = new Mock<IDatabase>();
            dbMock.SetupNewTransaction(transactionMock.Transaction);
            dbMock.Setup(r => r.DataProvider.GetActiveBonuses()).Returns(Task.FromResult(bonuses));

            var sportId = 1;
            var betedPairs = CollectionUtils.Generate(() => data.BetedPair(sportId++), 3);

            var quotaWithoutBonus = 2;
            var ticket = new Ticket { Quota = quotaWithoutBonus, Id = 4, BetedPairs = betedPairs }; 

            var bonusService = new BonusService(new BonusApplier(), dbMock.Object);

            await bonusService.ApplyBonuses(ticket);

            var pendingChanges = transactionMock.PendingChanges;

            pendingChanges.Updated.Count.Should().Be(1);
            pendingChanges.Updated.First().Should().Match<Ticket>(t => t.Quota == variousSportsBonus.IncreasesQuotaBy + quotaWithoutBonus);

            pendingChanges.Inserted.Count.Should().Be(1);
            pendingChanges.Inserted.First().Should().Match<AppliedBonus>(b => b.TicketId == ticket.Id && b.BonusName == variousSportsBonus.GetName());

            pendingChanges.Commited.Should().Be(true);
        }
    }
}
