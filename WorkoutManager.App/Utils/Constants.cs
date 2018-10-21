using System.Collections.Generic;
using System.Linq;

namespace WorkoutManager.App.Utils
{
    internal static class Constants
    {
        public static IEnumerable<int> RepsValues => Enumerable.Range(1, 100);

        public static IEnumerable<int> WeightValues => Enumerable.Range(0, 200);
    }
}