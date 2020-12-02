using System.Linq;

namespace AdventOfCode2020
{
    public sealed class Day1 : DayBase<int[]>
    {
        protected override int[] ProcessInput(string[] rawInput) => rawInput.Select(int.Parse).ToArray();

        public Day1() => Input = ProcessInput(ReadInput());

        public override int Part1()
        {
            foreach (var x in Input)
                if (Input.Contains(2020 - x))
                    return x * (2020 - x);

            return -1;
        }

        public override int Part2()
        {
            foreach (var x in Input)
            foreach (var y in Input)
                if (Input.Contains(2020 - x - y))
                    return x * y * (2020 - x - y);

            return -1;
        }
    }
}
