using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public sealed class Day10 : DayBase<List<int>>
    {
        protected override int Day { get; } = 10;

        public Day10() => Input = ProcessInput(ReadInput());

        protected override List<int> ProcessInput(List<string> rawInput) => rawInput.Select(int.Parse).ToList();

        public override long Part1()
        {
            (List<int> jolts1, List<int> jolts3) = GetValidJolts();
            return jolts1.Count * jolts3.Count;
        }

        public override long Part2()
        {
            (List<int> jolts1, List<int> jolts3) = GetValidJolts();

            var allJolts = jolts1.Concat(jolts3).OrderBy(x => x).ToList();

            var memory = new Dictionary<int, long>();
            long CalculateTotalValidSequencesFromIndex(int index)
            {
                if (index == allJolts.Count - 1)
                    return 1;
                if (memory.ContainsKey(index))
                    return memory[index];

                long total = 0;
                var nextIndex = index + 1;
                while (nextIndex < allJolts.Count && allJolts[nextIndex] - allJolts[index] <= 3)
                {
                    total += CalculateTotalValidSequencesFromIndex(nextIndex);
                    nextIndex++;
                }

                memory[index] = total;
                return total;
            }

            return CalculateTotalValidSequencesFromIndex(0);
        }

        private (List<int> jolts1, List<int> jolts3) GetValidJolts()
        {
            var input = Input.ToList();
            var maxJolt = input.Max();
            var jolts1 = new List<int>();
            var jolts3 = new List<int> {0};

            for (int i = 0; i < maxJolt; i++)
            {
                var choices = input.Where(x => x <= i + 3).ToList();
                if (!choices.Any())
                    continue;

                int current = choices.Min();
                if (current - i == 1)
                {
                    jolts1.Add(current);
                }
                else if (current - i == 3)
                {
                    jolts3.Add(current);
                    i += 2;
                }

                input.Remove(current);
            }

            return (jolts1, jolts3);
        }
    }
}