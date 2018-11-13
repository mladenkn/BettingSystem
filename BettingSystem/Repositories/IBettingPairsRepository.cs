using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationKernel;
using BetingSystem.Models;

namespace BetingSystem.Repositories
{
    public interface IBettingPairsRepository : IRepository<BetablePair>
    {
        
    }
}
