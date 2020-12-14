using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public sealed class Day13 : DayBase<(int, List<int>)>
    {
        protected override int Day { get; } = 13;

        public Day13() => Input = ProcessInput(ReadInput());

        protected override (int, List<int>) ProcessInput(List<string> rawInput)
        {
            var busIds = rawInput[1]
                .Replace("x", "-1")
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();

            return (int.Parse(rawInput[0]), busIds);
        }

        public override long Part1()
        {
            var input = (timeStamp: Input.Item1, busIds: Input.Item2);

            var earliest = input.busIds
                .Where(x => x > 0)
                .Select(busId => (busId, input.timeStamp - input.timeStamp % busId + busId))
                .OrderBy(x => x.Item2)
                .First();

            return (earliest.Item2 - input.timeStamp) * earliest.busId;
        }

        public override long Part2()
        {
            var modulos = new List<int>();
            var remainders = new List<int>();

            for (int i = 0; i < Input.Item2.Count; i++)
            {
                if (Input.Item2[i] < 0)
                    continue;

                modulos.Add(Input.Item2[i]);
                remainders.Add(Input.Item2[i] - i);
            }

            return Utils.ChineseRemainderTheorem.Solve(modulos.ToArray(), remainders.ToArray());
        }
    }
}