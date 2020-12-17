using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public sealed class Day17 : DayBase<Dictionary<(int, int, int, int), bool>>
    {
        protected override int Day { get; } = 17;

        public Day17() => Input = ProcessInput(ReadInput());

        protected override Dictionary<(int, int, int, int), bool> ProcessInput(List<string> rawInput)
        {
            var map = new Dictionary<(int, int, int, int), bool>();

            for (int x = 0; x < rawInput.Count; x++)
            for (int y = 0; y < rawInput[x].Length; y++)
                map.Add((x, y, 0, 0), rawInput[x][y] == '#');

            return map;
        }

        public override long Part1()
        {
            var map = Input.ToDictionary(cell => cell.Key, cell => cell.Value);

            for (int i = 0; i < 6; i++)
                map = RunCycle(map, false);

            return map.Count(c => c.Value);
        }

        public override long Part2()
        {
            var map = Input.ToDictionary(cell => cell.Key, cell => cell.Value);

            for (int i = 0; i < 6; i++)
                map = RunCycle(map, true);

            return map.Count(c => c.Value);
        }

        private Dictionary<(int, int, int, int), bool> RunCycle(Dictionary<(int, int, int, int), bool> previousMap, bool is4D)
        {
            var currentMap = new Dictionary<(int, int, int, int), bool>();
            var extendedMap = previousMap.SelectMany(cell => ExtendFrom(cell.Key, is4D));
            foreach (var cellPosition in new HashSet<(int, int, int, int)>(extendedMap))
            {
                var neighborCount = GetNeighbors(previousMap, cellPosition, is4D).Count(neighbor => neighbor);

                var isValidCount = neighborCount == 2 || neighborCount == 3;
                if (!isValidCount)
                    continue;

                var value = previousMap.ContainsKey(cellPosition) && previousMap[cellPosition];

                if (value)
                    currentMap[cellPosition] = true;
                else
                    currentMap[cellPosition] = neighborCount == 3;
            }

            return currentMap;
        }

        private IEnumerable<(int x, int y, int z, int w)> ExtendFrom((int x, int y, int z, int w) point, bool is4D)
        {
            for (int x = point.x - 1; x <= point.x + 1; x++)
            for (int y = point.y - 1; y <= point.y + 1; y++)
            for (int z = point.z - 1; z <= point.z + 1; z++)
            {
                if (is4D)
                {
                    for (int w = point.w - 1; w <= point.w + 1; w++)
                    {
                        if ((x, y, z, w) != point)
                            yield return (x, y, z, w);
                    }
                }
                else if ((x, y, z, 0) != point)
                {
                    yield return (x, y, z, 0);
                }
            }
        }

        private IEnumerable<bool> GetNeighbors(Dictionary<(int, int, int, int), bool> map, (int x, int y, int z, int w) point, bool is4D)
        {
            return ExtendFrom(point, is4D).Where(map.ContainsKey).Select(position => map[position]);
        }
    }
}