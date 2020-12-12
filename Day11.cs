using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public sealed class Day11 : DayBase<char[,]>
    {
        protected override int Day { get; } = 11;

        public Day11() => Input = ProcessInput(ReadInput());

        protected override char[,] ProcessInput(List<string> rawInput) => Utils.ToCharMatrix(rawInput);

        public override long Part1() => CountSeats(4, Utils.GetDirect8Neighbors);

        public override long Part2() => CountSeats(5, (matrix, x, y) => Utils.GetInDirect8Neighbors(matrix, x, y, new[] {'.'}, new[] {'L', '#'}));

        private int CountSeats(int switchDistance, Func<char[,], int, int, IEnumerable<char>> visibleSeatsFunc)
        {
            var currentMap = (char[,])Input.Clone();
            var previousMap = new char[currentMap.GetLength(0), currentMap.GetLength(1)];

            while (!Utils.MapsEquals(currentMap, previousMap))
            {
                previousMap = currentMap;
                currentMap = (char[,])previousMap.Clone();

                var width = previousMap.GetLength(0);
                var height = previousMap.GetLength(1);
                for (var x = 0; x < width; x++)
                for (var y = 0; y < height; y++)
                {
                    var seat = previousMap[x, y];
                    var visibleSeats = visibleSeatsFunc(previousMap, x, y).ToList();
                    if (seat == 'L' && visibleSeats.All(s => s != '#'))
                        currentMap[x, y] = '#';
                    else if (seat == '#' && visibleSeats.Count(s => s == '#') >= switchDistance)
                        currentMap[x, y] = 'L';
                }
            }

            return Utils.CountInMatrix(currentMap, '#');
        }
    }
}