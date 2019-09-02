using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Creative.System.Core
{
    public static class LinqExtensions
    {
        public static bool HasSameItemsAs<T>(this IEnumerable<T> a, IEnumerable<T> b)
        {
            if (a == null) return false;
            if (b == null) return false;

            var dictionary = a.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
            foreach (var item in b)
            {
                if (!dictionary.TryGetValue(item, out int value))
                {
                    return false;
                }
                if (value == 0)
                {
                    return false;
                }
                dictionary[item] -= 1;
            }
            return dictionary.All(x => x.Value == 0);
        }

        public static IEnumerable<T> Flatten<T>(
            this IEnumerable<T> tree,
            Func<T, IEnumerable<T>> childNodes)
        {
            // ReSharper disable PossibleMultipleEnumeration
            return tree.SelectMany(c => childNodes(c).Flatten(childNodes)).Concat(tree);
            // ReSharper restore PossibleMultipleEnumeration
        }


        /// <summary>
        /// Transform the set into keyed groups containing the count of the number of members within the group
        /// </summary>
        /// <typeparam name="TItem">The type of </typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="set"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IQueryable<DistributionKeyItem<TKey>> Distributions<TItem, TKey>(
            this IQueryable<TItem> set,
            Func<TItem, TKey> keySelector)
            where TKey : IComparable<TKey>
        {
            // Using the keySelector forces work as an enumerable because the query provider may not know how to use it.
            return set.AsEnumerable().Distributions(keySelector).AsQueryable();
        }

        /// <summary>
        /// Transform the set into keyed groups containing the count of the number of members within the group
        /// </summary>
        /// <typeparam name="TItem">The type of </typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="set"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<DistributionKeyItem<TKey>> Distributions<TItem, TKey>(
            this IEnumerable<TItem> set,
            Func<TItem, TKey> keySelector)
            where TKey : IComparable<TKey>
        {
            return set.GroupBy(keySelector, item => item, (key, items) => new DistributionKeyItem<TKey>
            {
                Key = key,
                Count = items.Count()
            });
        }

        /// <summary>
        /// Group the set into percentage-based groups containing the count of the number of members within the group
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="set"></param>
        /// <param name="valueSelector"></param>
        /// <param name="doubleConverter"></param>
        /// <param name="chunks"></param>
        /// <returns></returns>
        public static IEnumerable<DistributionRangeItem<int, TValue>> Distributions<TItem, TValue>(
            this IEnumerable<TItem> set,
            Func<TItem, TValue> valueSelector,
            Func<TValue, double> doubleConverter,
            int chunks)
            where TValue : IComparable<TValue>
        {
            return set.AsQueryable().Distributions(valueSelector, doubleConverter, chunks);
        }

        /// <summary>
        /// Transform the set into keyed groups containing the count of the number of members within the group
        /// </summary>
        /// <typeparam name="TItem">The type of </typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="set"></param>
        /// <param name="valueSelector"></param>
        /// <param name="doubleConverter"></param>
        /// <param name="chunks"></param>
        /// <returns></returns>
        public static IQueryable<DistributionRangeItem<int, TValue>> Distributions<TItem, TValue>(
            this IQueryable<TItem> set,
            Func<TItem, TValue> valueSelector,
            Func<TValue, double> doubleConverter,
            int chunks)
            where TValue : IComparable<TValue>
        {
            // This might work okay with a fraction, but not needed at this time.
            if (100 % chunks != 0)
                throw new ArgumentException(@"The value of chunks must be an even divisor of 100", nameof(chunks));

            var chunkSizeInPct = 100 / chunks;

            // Assign a grouping factor that is a fractional value. As the index progresses through the set,
            // the fractional value is incremented and will cross integer boundaries. The integral value defines 
            // the boundary of the group within the distribution.
            var setCount = set.Count();
            var groupInc = (double)chunks / setCount;
            var groups = set
                .OrderBy(item => valueSelector(item))
                .Select((item, i) => new
                {
                    GroupingFactor = (i + 1) * groupInc,
                    Item = item
                });

            // Use Math.Ceiling on the fractional grouping factors to group the set into chunks
            var distributionGroups = groups
                .GroupBy(item => (int)Math.Ceiling(item.GroupingFactor));

            // Project into a set of distribution items.
            var distributions = distributionGroups
                .Select(grp => new DistributionRangeItem<int, TValue>
                {
                    // The range is expressed as percentiles of size (100 / chunk)
                    RangeMinimumInclusive = grp.Key * chunkSizeInPct - chunkSizeInPct,  
                    RangeMaximumExclusive = grp.Key * chunkSizeInPct,
                    Count = grp.Count(),
                    Mean = grp.Average(item => doubleConverter(valueSelector(item.Item))),
                    Median = grp.Select(item => valueSelector(item.Item)).Median(),
                    Mode = grp.GroupBy(item => valueSelector(item.Item))
                        .OrderByDescending(modeGrp => modeGrp.Count()).ThenBy(modeGrp => modeGrp.Key)
                        .Select(modeGrp => modeGrp.Key)
                        .FirstOrDefault(),
                    MinimumValue = grp.Min(item => valueSelector(item.Item)),
                    MaximumValue = grp.Max(item => valueSelector(item.Item))
                });

            return distributions;
        }

        /// <summary>
        /// Partitions the given list around a pivot element such that all elements on left of pivot are &lt;= pivot
        /// and the ones at thr right are > pivot. This method can be used for sorting, N-order statistics such as
        /// as median finding algorithms.
        /// Pivot is selected randomly if random number generator is supplied else its selected as last element in the list.
        /// Reference: Introduction to Algorithms 3rd Edition, Corman et al, pp 171
        /// </summary>
        static int Partition<T>(this IList<T> list, int start, int end, Random rnd = null) where T : IComparable<T>
        {
            if (rnd != null)
                list.Swap(end, rnd.Next(start, end + 1));

            var pivot = list[end];
            var lastLow = start - 1;
            for (var i = start; i < end; i++)
            {
                if (list[i].CompareTo(pivot) <= 0)
                    list.Swap(i, ++lastLow);
            }
            list.Swap(end, ++lastLow);
            return lastLow;
        }

        /// <summary>
        /// Returns Nth smallest element from the list. Here n starts from 0 so that n=0 returns minimum, n=1 returns 
        /// 2nd smallest element etc.
        /// Note: specified list would be mutated in the process.
        /// Reference: Introduction to Algorithms 3rd Edition, Corman et al, pp 216
        /// </summary>
        public static T NthOrderStatistic<T>(this IList<T> list, int n, Random rnd = null) where T : IComparable<T>
        {
            return NthOrderStatistic(list, n, 0, list.Count - 1, rnd);
        }

        static T NthOrderStatistic<T>(this IList<T> list, int n, int start, int end, Random rnd) where T : IComparable<T>
        {
            while (true)
            {
                var pivotIndex = list.Partition(start, end, rnd);
                if (pivotIndex == n)
                    return list[pivotIndex];

                if (n < pivotIndex)
                    end = pivotIndex - 1;
                else
                    start = pivotIndex + 1;
            }
        }

        public static void Swap<T>(this IList<T> list, int i, int j)
        {
            if (i == j)   //This check is not required but Partition function may make many calls so its for perf reason
                return;
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        /// <summary>
        /// Note: specified list would be mutated in the process.
        /// </summary>
        public static T Median<T>(this IList<T> list) where T : IComparable<T>
        {
            return list.NthOrderStatistic((list.Count - 1) / 2);
        }

        public static T Median<T>(this IEnumerable<T> sequence)
            where T : IComparable<T>
        {
            var list = sequence.ToList();
            var mid = (list.Count - 1) / 2;
            return list.NthOrderStatistic(mid);
        }

        /// <summary>
            /// Enumerates over the set in parallel and executes the aync code in <paramref name="body"/>.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="source"></param>
            /// <param name="body"></param>
            /// <param name="maxDegreeOfParallelism">The default value is <seealso cref="DataflowBlockOptions.Unbounded"/>.</param>
            /// <param name="scheduler">If no scheduler is specified, the default scheduler will be used.</param>
            /// <returns></returns>
            public static Task AsyncParallelForEach<T>(
            this IEnumerable<T> source, Func<T, Task> body,
            int maxDegreeOfParallelism = DataflowBlockOptions.Unbounded,
            TaskScheduler scheduler = null)
        {
            var options = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = maxDegreeOfParallelism
            };

            if (scheduler == null)
            {
                // If there is no SyncContext for this thread (e.g. we are in a unit test
                // or console scenario instead of running in an app), then just use the
                // default scheduler because there is no UI thread to sync with.
                var syncContextScheduler = SynchronizationContext.Current != null
                    ? TaskScheduler.FromCurrentSynchronizationContext()
                    : TaskScheduler.Current;

                if (syncContextScheduler != null)
                    options.TaskScheduler = syncContextScheduler;
            }
            else options.TaskScheduler = scheduler;

            var block = new ActionBlock<T>(body, options);

            foreach (var item in source)
                block.Post(item);

            block.Complete();
            return block.Completion;
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> expr)
        {
            foreach (var item in source)
                expr(item);
        }
    }
}