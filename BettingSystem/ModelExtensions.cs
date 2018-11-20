using System;
using System.Linq;
using BetingSystem.Models;
using Utilities;

namespace BetingSystem
{
    public static class ModelExtensions
    {
        public static decimal GetQuotaForType(this BetablePair pair, BetingType type)
        {
            switch (type)
            {
                case BetingType.Team1Win: return pair.Team1WinQuota;
                case BetingType.Team2Win: return pair.Team2WinQuota;
                case BetingType.Draw: return pair.DrawQuota;
                default: throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static decimal GetQuota(this BetedPair pair) => pair.BetablePair.GetQuotaForType(pair.BetedType);

        public static void RefreshQuota(this Ticket ticket) => ticket.Quota = ticket.BetedPairs.Select(p => p.GetQuota()).Product();

        public static string GetName(this ITicketBonus bonus) => bonus.GetType().Name;
    }
}
