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

    public class ModelNotFound : ApplicationException
    {
        public ModelNotFound(Type modelType) : base($"Model of type {modelType} with the id of has not been found.")
        {
            WantedObjectType = modelType;
        }

        public Type WantedObjectType { get; }
    }
}
