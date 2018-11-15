using System;
using System.Collections.Generic;

namespace BetingSystem
{
    public class BetablePairsNotFound : ApplicationException
    {
        public BetablePairsNotFound(IEnumerable<int> notFoundPairsIds)
        {
            NotFoundPairsIds = notFoundPairsIds;
        }

        public IEnumerable<int> NotFoundPairsIds { get; }
    }
}
