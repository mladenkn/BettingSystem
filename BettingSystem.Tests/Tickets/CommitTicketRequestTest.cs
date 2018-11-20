using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BetingSystem.Models;
using BetingSystem.Requests;
using BetingSystem.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace BetingSystem.Tests.Tickets
{
    public class CommitTicketRequestTest
    {
        [Fact]
        public async Task Run()
        {
            var data = new DataFactory();
            
            var pair1SelectedQuota = 23;
            var pair2SelectedQuota = 6;
            var pair3SelectedQuota = 2;

            var betablePair1 = data.BetablePair(1, pair1SelectedQuota, -1, -1, new Team(), new Team());
            var betablePair2 = data.BetablePair(2, -1, pair2SelectedQuota, -1, new Team(), new Team());
            var betablePair3 = data.BetablePair(3, -1, -1, pair3SelectedQuota, new Team(), new Team());
            
            var request = new CommitTicketRequest
            {
                Stake = 19,
                BetingPairs = new[]
                {
                    new CommitTicketRequest.BetingPair
                    {
                        BetedPairId = betablePair1.Id,
                        BetingType = BetingType.Team1Win
                    },
                    new CommitTicketRequest.BetingPair
                    {
                        BetedPairId = betablePair2.Id,
                        BetingType = BetingType.Team2Win
                    },
                    new CommitTicketRequest.BetingPair
                    {
                        BetedPairId = betablePair3.Id,
                        BetingType = BetingType.Draw
                    },
                }
            };

            var userId = "a";

            var transactionMock = new TransactionMock();
            var dbMock = new Mock<IDatabase>();
            dbMock.SetupNewTransaction(transactionMock.Transaction);
            dbMock.SetupGenericQuery(betablePair1, betablePair2, betablePair3);

            var currentUserAccessor = MockUtils.CurrentUserAccessor(userId);
            var service = new TicketService(
                Mock.Of<IBonusService>(), dbMock.Object, Mock.Of<IWalletService>(), Mock.Of<IMapper>(), currentUserAccessor);
            await service.Handle(request);

            var insertedData = transactionMock.PendingChanges.Inserted;

            var commitedTicket = insertedData.OfType<Ticket>().SingleOrDefault();
            commitedTicket.Should().NotBeNull();
            commitedTicket.Quota.Should().Be(pair1SelectedQuota * pair2SelectedQuota * pair3SelectedQuota);
            commitedTicket.UserId.Should().Be(userId);
            commitedTicket.Stake.Should().Be(request.Stake);

            var betedPairs = insertedData.OfType<BetedPair>().ToArray();
            betedPairs.Length.Should().Be(3);

            void AssertOnPair(int betablePairId, int quota, BetingType type)
            {
                var pair = betedPairs.Single(p => p.BetablePairId == betablePairId);
                pair.GetQuota().Should().Be(quota);
                pair.BetedType.Should().Be(type);
            }

            AssertOnPair(betablePair1.Id, pair1SelectedQuota, BetingType.Team1Win);
            AssertOnPair(betablePair2.Id, pair2SelectedQuota, BetingType.Team2Win);
            AssertOnPair(betablePair3.Id, pair3SelectedQuota, BetingType.Draw);

            commitedTicket.Quota.Should().Be(pair1SelectedQuota * pair2SelectedQuota * pair3SelectedQuota);
            commitedTicket.UserId.Should().Be(userId);
            commitedTicket.Stake.Should().Be(request.Stake);
        }
    }
}
