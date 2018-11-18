using System.Collections.Generic;
using System.Threading.Tasks;

namespace Utilities
{
    public static class TaskUtils
    {
        public static Task WhenAll(this IEnumerable<Task> tasks) => Task.WhenAll(tasks);
    }
}
