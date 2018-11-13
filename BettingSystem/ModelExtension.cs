﻿using System;
using BetingSystem.Models;

namespace BetingSystem
{
    public static class ModelExtension
    {
        public static double QuotaForType(this BetablePair pair, BetingType type)
        {
            switch (type)
            {
                case BetingType.Team1Win: return pair.Team1WinQuota;
                case BetingType.Team2Win: return pair.Team2WinQuota;
                case BetingType.Draw: return pair.DrawQuota;
                default: throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static double Quota(this BetedPair pair) => pair.BetablePair.QuotaForType(pair.BetedType);
    }
}
