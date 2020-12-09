using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public sealed class Day9 : DayBase<List<long>>
    {
        protected override int Day { get; } = 9;

        public Day9() => Input = ProcessInput(ReadInput());

        protected override List<long> ProcessInput(List<string> rawInput) => rawInput.Select(long.Parse).ToList();

        public override long Part1() => FindWeakness().weakness;

        public override long Part2()
        {
            (long weakness, int weekSpot) = FindWeakness();

            for (var i = 2; i < Input.Count; i++)
            for (var j = 0; j < weekSpot; j++)
            {
                var innerSequence = Input.GetRange(j, i);
                if (innerSequence.Sum() == weakness)
                    return innerSequence.Max() + innerSequence.Min();
            }

            return -1;
        }

        private (long weakness, int weekSpot) FindWeakness()
        {
            var blockLength = 25;

            for (int i = blockLength; i < Input.Count; i++)
            {
                var previousBlock = Input.GetRange(i - blockLength, blockLength);

                var current = Input[i];
                var contains = false;
                foreach (var j in previousBlock)
                foreach (var k in previousBlock)
                    if (current == j + k)
                        contains = true;

                if (!contains)
                    return (current, i);
            }

            return (-1, -1);
        }
    }
}