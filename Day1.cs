using System.IO;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day1
    {
        private readonly int[] _numbers;

        public Day1()
        {
            var input = File.ReadAllLines("input.txt");
            var numbers = input.Select(int.Parse).ToArray();
            _numbers = numbers;
        }

        public int Part1()
        {
            foreach (var x in _numbers)
                if (_numbers.Contains(2020 - x))
                    return x * (2020 - x);

            return -1;
        }

        public int Part2()
        {
            foreach (var x in _numbers)
            foreach (var y in _numbers)
                if (_numbers.Contains(2020 - x - y))
                    return x * y * (2020 - x - y);

            return -1;
        }
    }
}
