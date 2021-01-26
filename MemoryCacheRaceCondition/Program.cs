using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace MemoryCacheRaceCondition
{
    internal class Program
    {
        private static void Main()
        {
            // demo of race condition of GetOrCreate when cache is null.
            var cache = new MemoryCache(new MemoryCacheOptions());
            var rand = new Random();
            var tasks = Enumerable.Range(0, 10).Select(i => Task.Run(() =>
            {
                var ret = cache.GetOrCreate("key", _ => rand.Next());
                Console.WriteLine($"Task {i,2}: {ret}");
            })).ToArray();
            Task.WaitAll(tasks);

            Console.WriteLine();

            // another demo from https://blog.novanet.no/asp-net-core-memory-cache-is-get-or-create-thread-safe/
            var counter = 0;
            Parallel.ForEach(Enumerable.Range(1, 10), _ =>
            {
                var item = cache.GetOrCreate("test-key", _ => Interlocked.Increment(ref counter));
                Console.Write($"{item} ");
            });

            Console.WriteLine();
        }
    }
}
