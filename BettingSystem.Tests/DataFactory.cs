using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetingSystem.Models;

namespace BetingSystem.Tests
{
    public class DataFactory
    {
        public BetablePair BetablePair(double team1WinQuota, double team2WinQuota, double drawQuota)
        {
            return new BetablePair
            {
                Team1WinQuota = team1WinQuota,
                Team2WinQuota = team2WinQuota,
                DrawQuota = drawQuota
            };
        }
    }
}
