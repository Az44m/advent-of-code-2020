using System;
using System.Collections.Generic;
using System.Drawing;

namespace AdventOfCode2020
{
    public sealed class Day3 : DayBase<List<string>>
    {
        protected override int Day { get; } = 3;

        public Day3() => Input = ProcessInput(ReadInput());

        protected override List<string> ProcessInput(List<string> rawInput) => rawInput;

        public override int Part1() => CountTrees(new Point(3, 1));

        public override int Part2() => CountTrees(new Point(1, 1), new Point(3, 1), new Point(5, 1), new Point(7, 1), new Point(1, 2));

        private int CountTrees(params Point[] slopes)
        {
            var allTrees = 0;

            foreach (var slope in slopes)
            {
                var numberOfTrees = 0;
                var posX = 0;
                for (var y = 0; y < Input.Count; y += slope.Y)
                {
                    var row = Input[y];
                    numberOfTrees += row[posX] == '#' ? 1 : 0;
                    posX = (posX + slope.X) % row.Length;
                }

                allTrees = Math.Max(allTrees, 1) * Math.Max(numberOfTrees, 1);
            }

            return allTrees;
        }
    }
}
