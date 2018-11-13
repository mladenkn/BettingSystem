using System;
using System.Linq;
using BetingSystem.Models;
using Utilities;

namespace BetingSystem
{
    public static class ModelExtension
    {
        public static double CalculateQuota(this BetablePair pair, BetingType type)
        {
            switch (type)
            {
                case BetingType.Team1Win: return pair.Team1WinQuota;
                case BetingType.Team2Win: return pair.Team2WinQuota;
                case BetingType.Draw: return pair.DrawQuota;
                default: throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static double CalculateQuota(this BetedPair pair) => pair.BetablePair.CalculateQuota(pair.BetedType);

        public static double CalculateQuota(this Ticket ticket) => ticket.BetedPairs.Select(p => p.CalculateQuota()).Product();
    }
}
