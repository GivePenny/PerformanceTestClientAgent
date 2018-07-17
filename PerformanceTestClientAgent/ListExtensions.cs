using System;
using System.Collections.Generic;

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

            var randomIndex = randomNumberGenerator.Next(list.Count - 1);
            return list[randomIndex];
        }
    }
}
