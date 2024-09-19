using System;
using System.Collections.Generic;

namespace Game.Utility
{
    public static class ListExtensions
    {
        private static Random _random = new();

        /// <summary>
        /// Implementation of the Fisher-Yates shuffle.
        /// </summary>
        /// <param name="list">The list to shuffle.</param>
        public static void Shuffle<T>(this List<T> list)
        {
            int iteration = list.Count;
            while (iteration > 1)
            {
                iteration--;

                int random = _random.Next(iteration + 1);
                T value = list[random];
                list[random] = list[iteration];
                list[iteration] = value;
            }
        }
    }
}