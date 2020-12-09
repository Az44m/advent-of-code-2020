using System;

namespace AdventOfCode2020
{
    class Program
    {
        static void Main()
        {
            var day = new Day9();

            var part1 = day.Part1();
            var part2 = day.Part2();

            Console.WriteLine($"Part 1: {part1}");
            Console.WriteLine($"Part 2: {part2}");

            Console.WriteLine("Press \'1\' to submit Part 1, \'2\' to Part 2...");
            var key = Console.ReadKey();
            var value = (int) char.GetNumericValue(key.KeyChar);
            var answer = value == 1 ? part1 : part2;
            day.SubmitAnswer(value, answer);

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
