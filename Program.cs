using System;

namespace AdventOfCode2020
{
    class Program
    {
        static void Main()
        {
            var day = new Day2();

            Console.WriteLine($"Part 1: {day.Part1()}");
            Console.WriteLine($"Part 2: {day.Part2()}");

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
