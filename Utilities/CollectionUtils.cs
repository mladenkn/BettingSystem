using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities
{
    public static class CollectionUtils
    {
        public static double Product(this IEnumerable<double> numbers) => numbers.Aggregate(1.0, (x, y) => x * y);
    }
}
