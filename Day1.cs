using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public sealed class Day1 : DayBase<int[]>
    {
        protected override int Day { get; } = 1;
        public Day1() => Input = ProcessInput(ReadInput());

        protected override int[] ProcessInput(List<string> rawInput) => rawInput.Select(int.Parse).ToArray();

        public override long Part1()
        {
            foreach (var x in Input)
                if (Input.Contains(2020 - x))
                    return x * (2020 - x);

            return -1;
        }

        public override long Part2()
        {
            foreach (var x in Input)
            foreach (var y in Input)
                if (Input.Contains(2020 - x - y))
                    return x * y * (2020 - x - y);

            return -1;
        }
    }
}
