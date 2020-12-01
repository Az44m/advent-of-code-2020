using System;

namespace AdventOfCode2020
{
    class Program
    {
        static void Main()
        {
            var day1 = new Day1();

            Console.WriteLine($"Part 1: {day1.Part1()}");
            Console.WriteLine($"Part 2: {day1.Part2()}");

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
