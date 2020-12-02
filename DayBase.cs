using System.IO;

namespace AdventOfCode2020
{
    public abstract class DayBase<T>
    {
        protected T Input { get; set; }
        protected abstract T ProcessInput(string[] rawInput);
        public abstract int Part1();
        public abstract int Part2();
        protected string[] ReadInput() => File.ReadAllLines("input.txt");
    }
}
