using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public sealed class Day15 : DayBase<object>
    {
        protected override int Day { get; } = 15;

        protected override object ProcessInput(List<string> rawInput) => null;

        public override long Part1() => PlayMemoryGame(2020);
        public override long Part2() => PlayMemoryGame(30000000);

        private int PlayMemoryGame(int iterations)
        {
            var spokenNumbers = new List<int> {12, 1, 16, 3, 11, 0};
            var cache = new Dictionary<int, List<int>>();

            for (var index = 0; index < spokenNumbers.Count; index++)
                cache.Add(spokenNumbers[index], new List<int> {index + 1});

            for (int i = spokenNumbers.Count; i < iterations; i++)
            {
                var lastNum = spokenNumbers[i - 1];
                if (cache[lastNum].Count < 2)
                {
                    if (cache.ContainsKey(0))
                        cache[0].Add(i + 1);
                    else
                        cache.Add(0, new List<int> {i + 1});

                    spokenNumbers.Add(0);
                }
                else
                {
                    var num1 = cache[lastNum][cache[lastNum].Count - 1];
                    var num2 = cache[lastNum][cache[lastNum].Count - 2];

                    if (cache.ContainsKey(num1 - num2))
                        cache[num1 - num2].Add(i + 1);
                    else
                        cache.Add(num1 - num2, new List<int> {i + 1});
                    spokenNumbers.Add(num1 - num2);
                }
            }

            return spokenNumbers.Last();
        }
    }
}