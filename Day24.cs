using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public sealed class Day24 : DayBase<List<string>>
    {
        protected override int Day { get; } = 24;

        public Day24() => Input = ProcessInput(ReadInput());

        protected override List<string> ProcessInput(List<string> rawInput) => rawInput;

        public override long Part1() => GetBlackTiles().Count;

        public override long Part2()
        {
            var previousMap = new HashSet<(int x, int y, int z)>(GetBlackTiles());

            for (var i = 0; i < 100; i++)
            {
                var currentMap = new HashSet<(int x, int y, int z)>();
                var extendedMap = previousMap
                    .SelectMany(tile => GetNeighbors((tile.x, tile.y, tile.z)))
                    .ToHashSet();

                foreach (var tile in extendedMap)
                {
                    var neighborCount = GetNeighbors(tile).Count(previousMap.Contains);

                    if (!(neighborCount == 2 || neighborCount == 1))
                        continue;

                    var isBlack = previousMap.Contains(tile);

                    if (isBlack || neighborCount == 2)
                        currentMap.Add(tile);
                }

                previousMap = currentMap;
            }

            return previousMap.Count;
        }

        private IEnumerable<(int x, int y, int z)> GetNeighbors((int x, int y, int z) point)
        {
            var directions = new[] {(1, -1, 0), (0, -1, 1), (-1, 0, 1), (-1, 1, 0), (0, 1, -1), (1, 0, -1)};
            foreach ((int dx, int dy, int dz) in directions)
                yield return (point.x + dx, point.y + dy, point.z + dz);
        }

        private List<(int, int, int)> GetBlackTiles()
        {
            var blackTiles = new List<(int, int, int)>();

            foreach (var line in Input)
            {
                var x = 0;
                var y = 0;
                var z = 0;

                var lineCopy = line;
                while (lineCopy != "")
                {
                    if (lineCopy.StartsWith("e"))
                    {
                        x += 1;
                        y -= 1;
                        lineCopy = lineCopy[1..];
                    }
                    else if (lineCopy.StartsWith("se"))
                    {
                        y -= 1;
                        z += 1;
                        lineCopy = lineCopy[2..];
                    }
                    else if (lineCopy.StartsWith("sw"))
                    {
                        x -= 1;
                        z += 1;
                        lineCopy = lineCopy[2..];
                    }
                    else if (lineCopy.StartsWith("w"))
                    {
                        x -= 1;
                        y += 1;
                        lineCopy = lineCopy[1..];
                    }
                    else if (lineCopy.StartsWith("nw"))
                    {
                        z -= 1;
                        y += 1;
                        lineCopy = lineCopy[2..];
                    }
                    else if (lineCopy.StartsWith("ne"))
                    {
                        x += 1;
                        z -= 1;
                        lineCopy = lineCopy[2..];
                    }
                }

                if (blackTiles.Contains((x, y, z)))
                    blackTiles.Remove((x, y, z));
                else
                    blackTiles.Add((x, y, z));
            }

            return blackTiles;
        }
    }
}