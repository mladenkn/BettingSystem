﻿using System;
using System.Linq;
using System.Threading.Tasks;
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
            var db = TestServicesFactory.DbContext();
            
            var pair1SelectedQuota = 23;
            var pair2SelectedQuota = 6;
            var pair3SelectedQuota = 2;

            var betablePair1 = data.BetablePair(pair1SelectedQuota, -1, -1, new Team(), new Team());
            var betablePair2 = data.BetablePair(-1, pair2SelectedQuota, -1, new Team(), new Team());
            var betablePair3 = data.BetablePair(-1, -1, pair3SelectedQuota, new Team(), new Team());

            db.AddRange(betablePair1, betablePair2, betablePair3);
            
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

            db.SaveChanges();

            var currentUserAccessor = TestServicesFactory.CureCurrentUserAccessor(userId);
            var service = new TicketService(Mock.Of<IBonusService>(), db, Mock.Of<IWalletService>(), currentUserAccessor, null);
            await service.Handle(request);

            var commitedTicket = db.Tickets.Single();
            commitedTicket.Should().NotBeNull();

            var betedPairs = db.BetedPairs.ToArray();
            betedPairs.Length.Should().Be(3);

            void AssertOnPair(int betablePairId, Action<BetedPair> assert) =>
                assert(betedPairs.Single(p => p.BetablePairId == betablePairId));

            AssertOnPair(betablePair1.Id, pair =>
            {
                pair.GetQuota().Should().Be(pair1SelectedQuota);
                pair.BetedType.Should().Be(BetingType.Team1Win);
            });

            AssertOnPair(betablePair2.Id, pair =>
            {
                pair.GetQuota().Should().Be(pair2SelectedQuota);
                pair.BetedType.Should().Be(BetingType.Team2Win);
            });

            AssertOnPair(betablePair3.Id, pair =>
            {
                pair.GetQuota().Should().Be(pair3SelectedQuota);
                pair.BetedType.Should().Be(BetingType.Draw);
            });

            commitedTicket.Quota.Should().Be(pair1SelectedQuota * pair2SelectedQuota * pair3SelectedQuota);
            commitedTicket.UserId.Should().Be(userId);
            commitedTicket.Stake.Should().Be(request.Stake);
        }
    }
}
