using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public sealed class Day8 : DayBase<List<string>>
    {
        protected override int Day { get; } = 8;
        public Day8() => Input = ProcessInput(ReadInput());
        protected override List<string> ProcessInput(List<string> rawInput) => rawInput;

        public override int Part1() => RunInstructions(out _);

        public override int Part2()
        {
            var visitedIndexes = new HashSet<int>();
            for (var i = 0; i < Input.ToList().Count; i++)
            {
                var counter = RunInstructions(out var isInfinite);
                if (!isInfinite)
                    return counter;

                var instruction = Input[i].Split(' ');
                if (instruction[0] == "nop")
                    Input[i] = "jmp " + instruction[1];
                else if (instruction[0] == "jmp")
                    Input[i] = "nop " + instruction[1];

                if (!visitedIndexes.Contains(i))
                {
                    visitedIndexes.Add(i);
                    i--;
                }
            }

            return -1;
        }

        private int RunInstructions(out bool isInfinite)
        {
            isInfinite = false;
            var counter = 0;
            var visitedIndexes = new HashSet<int>();
            for (int i = 0; i < Input.Count; i++)
            {
                if (visitedIndexes.Contains(i))
                {
                    isInfinite = true;
                    break;
                }

                var instruction = Input[i].Split(' ');
                if (instruction[0] == "nop")
                    continue;
                if (instruction[0] == "acc")
                    counter += int.Parse(instruction[1]);
                else
                    i += int.Parse(instruction[1]) - 1;

                visitedIndexes.Add(i);
            }

            return counter;
        }
    }
}