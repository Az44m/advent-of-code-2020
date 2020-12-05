using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public sealed class Day5 : DayBase<List<string>>
    {
        protected override int Day { get; } = 5;
        public Day5() => Input = ProcessInput(ReadInput());
        protected override List<string> ProcessInput(List<string> rawInput) => rawInput;

        public override int Part1() => CalculateSeatIds().Max();

        public override int Part2()
        {
            var orderedIds = CalculateSeatIds().OrderBy(id => id).ToArray();
            for (var i = 0; i < orderedIds.Length; i++)
            {
                var id = orderedIds[i];
                if (id != i + orderedIds[0])
                    return id - 1;
            }

            return -1;
        }

        private IEnumerable<int> CalculateSeatIds()
        {
            return Input.Select(row =>
            {
                var rowInBinary = row
                    .Replace('B', '1')
                    .Replace('F', '0')
                    .Replace('R', '1')
                    .Replace('L', '0');
                return Convert.ToInt32(rowInBinary, 2);
            });
        }
    }
}
