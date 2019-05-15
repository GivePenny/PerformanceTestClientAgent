using System;
using System.Collections.Generic;
using System.Linq;

namespace PerformanceTestClientAgent
{
    public static class ListExtensions
    {
        static readonly Random randomNumberGenerator = new Random();

        public static T PickOneRandomly<T>(this List<T> list)
        {
            if (list.Count == 0)
            {
                throw new ArgumentException("List is empty.");
            }

            var randomIndex = randomNumberGenerator.Next(list.Count);
            return list[randomIndex];
        }

        public static IEnumerable<T> PickSomeRandomly<T>(this List<T> list, int minimumNumberOfItemsToPick)
        {
            if (list.Count == 0)
            {
                throw new ArgumentException("List is empty.");
            }

            if (minimumNumberOfItemsToPick <= 0)
            {
                throw new ArgumentException($"{nameof(minimumNumberOfItemsToPick)} must be greater than zero.");
            }

            if (minimumNumberOfItemsToPick > list.Count)
            {
                throw new ArgumentException($"Asked to pick at least {minimumNumberOfItemsToPick} elements but the list only contains {list.Count}.");
            }

            var indexValuesAvailableToPickFrom = Enumerable.Range(0, list.Count).ToList();
            var numberOfItemsToPick = randomNumberGenerator.Next(minimumNumberOfItemsToPick, list.Count + 1);

            for (var pickedItems = 0; pickedItems < numberOfItemsToPick; pickedItems++)
            {
                var randomIndex = randomNumberGenerator.Next(indexValuesAvailableToPickFrom.Count);
                var randomIndexValue = indexValuesAvailableToPickFrom[randomIndex];

                indexValuesAvailableToPickFrom.RemoveAt(randomIndex);

                yield return list[randomIndexValue];
            }
        }
    }
}
