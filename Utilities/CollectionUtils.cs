using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities
{
    public static class CollectionUtils
    {
        public static decimal Product(this IEnumerable<decimal> numbers) => numbers.Aggregate(1m, (x, y) => x * y);

        public static List<T> Generate<T>(Func<T> generateOne, int count)
        {
            return Enumerable.Range(0, count).Select(_ => generateOne()).ToList();
        }

        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
                action(item);
        }
    }
}
