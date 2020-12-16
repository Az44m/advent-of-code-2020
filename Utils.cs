using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public static class Utils
    {
        public static List<string> ConcatGroupOfLines<T>(T input, string groupItemSeparator, string groupSeparator,
            bool insertSeparatorAfterLastLine = true) where T : ICollection<string>
        {
            var concatenatedGroups = new List<string>();
            var stringBuilder = new StringBuilder();

            if (insertSeparatorAfterLastLine)
                input.Add(groupSeparator);

            foreach (var row in input)
            {
                if (row != groupSeparator)
                {
                    stringBuilder.Append($"{groupItemSeparator}{row}");
                }
                else
                {
                    concatenatedGroups.Add(stringBuilder.ToString().Trim());
                    stringBuilder.Clear();
                }
            }

            return concatenatedGroups;
        }

        public static char[,] ToCharMatrix<T>(T input) where T : IList<string>
        {
            var matrix = new char[input.Count, input[0].Length];
            for (var x = 0; x < input.Count; x++)
            for (var y = 0; y < input[x].Length; y++)
                matrix[x, y] = input[x][y];

            return matrix;
        }

        private static readonly (int x, int y)[] NeighborDirections4 = {(0, 1), (1, 0), (0, -1), (-1, 0)};
        private static readonly (int x, int y)[] NeighborDirections8 = {(-1, 1), (0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0)};

        private static bool IsValid(int x, int y, int width, int height) => x >= 0 && x < width && y >= 0 && y < height;

        public static IEnumerable<T> GetDirect4Neighbors<T>(T[,] matrix, int x, int y) => GetDirectNeighbors(matrix, x, y, NeighborDirections4);

        public static IEnumerable<T> GetDirect8Neighbors<T>(T[,] matrix, int x, int y) => GetDirectNeighbors(matrix, x, y, NeighborDirections8);

        private static IEnumerable<T> GetDirectNeighbors<T>(T[,] matrix, int x, int y, (int x, int y)[] neighborDirections)
        {
            return neighborDirections
                .Where(dir => IsValid(dir.x + x, dir.y + y, matrix.GetLength(0), matrix.GetLength(1)))
                .Select(pos => matrix[pos.x + x, pos.y + y]);
        }

        public static Dictionary<(int x, int y), T> GetDirect4NeighborsWithCoordinates<T>(T[,] matrix, int x, int y)
        {
            return GetDirectNeighborsWithCoordinates(matrix, x, y, NeighborDirections4);
        }

        public static Dictionary<(int x, int y), T> GetDirect8NeighborsWithCoordinates<T>(T[,] matrix, int x, int y)
        {
            return GetDirectNeighborsWithCoordinates(matrix, x, y, NeighborDirections8);
        }

        private static Dictionary<(int x, int y), T> GetDirectNeighborsWithCoordinates<T>(T[,] matrix, int x, int y,
            (int x, int y)[] neighborDirections)
        {
            return neighborDirections
                .Where(dir => IsValid(dir.x + x, dir.y + y, matrix.GetLength(0), matrix.GetLength(1)))
                .ToDictionary(pos => (pos.x, pos.y), pos => matrix[pos.x + x, pos.y + y]);
        }

        public static IEnumerable<T> GetInDirect4Neighbors<T>(T[,] matrix, int x, int y, T[] nonObstacles, T[] obstacles)
        {
            return GetInDirectNeighbors(matrix, x, y, nonObstacles, obstacles, NeighborDirections4);
        }

        public static IEnumerable<T> GetInDirect8Neighbors<T>(T[,] matrix, int x, int y, T[] nonObstacles, T[] obstacles)
        {
            return GetInDirectNeighbors(matrix, x, y, nonObstacles, obstacles, NeighborDirections8);
        }

        private static List<T> GetInDirectNeighbors<T>(T[,] matrix, int x, int y, T[] nonObstacles, T[] obstacles, (int x, int y)[] neighborDirs)
        {
            var inDirectNeighbors = new List<T>();

            foreach ((int x, int y) direction in neighborDirs)
            {
                var x1 = x;
                var y1 = y;

                while (true)
                {
                    x1 += direction.x;
                    y1 += direction.y;

                    if (!IsValid(x1, y1, matrix.GetLength(0), matrix.GetLength(1)))
                        break;

                    var seat = matrix[x1, y1];

                    if (nonObstacles.Contains(seat))
                        continue;

                    if (obstacles.Contains(seat))
                        inDirectNeighbors.Add(seat);

                    break;
                }
            }

            return inDirectNeighbors;
        }

        public static bool MapsEquals<T>(T[,] map1, T[,] map2)
        {
            for (var y = 0; y < map1.GetLength(1); y++)
            for (var x = 0; x < map1.GetLength(0); x++)
                if (!EqualityComparer<T>.Default.Equals(map1[x, y], map2[x, y]))
                    return false;

            return true;
        }

        public static int CountInMatrix<T>(T[,] matrix, T itemToCount)
        {
            var count = 0;
            for (var x = 0; x < matrix.GetLength(0); x++)
            for (var y = 0; y < matrix.GetLength(1); y++)
                if (EqualityComparer<T>.Default.Equals(matrix[x, y], itemToCount))
                    count++;

            return count;
        }

        public static (int, int) RotatePoint((int x, int y) point, float degrees)
        {
            var degreeInRadian = degrees * (Math.PI / 180);
            var cos = Math.Cos(degreeInRadian);
            var sin = Math.Sin(degreeInRadian);
            return ((int)Math.Round(point.x * cos - point.y * sin), (int)Math.Round(point.x * sin + point.y * cos));
        }

        public static List<string> GeneratePermutations(char[] input, int permutationLength)
        {
            return GenerateSubPermutations(input, "", input.Length, permutationLength, new List<string>());
        }

        private static List<string> GenerateSubPermutations(char[] input, String prefix, int inputLength, int permutationLength,
            List<string> permutations)
        {
            if (permutationLength == 0)
            {
                permutations.Add(prefix);
                return permutations;
            }

            for (var i = 0; i < inputLength; ++i)
            {
                var newPrefix = prefix + input[i];
                permutations.AddRange(GenerateSubPermutations(input, newPrefix, inputLength, permutationLength - 1, new List<string>()));
            }

            return permutations;
        }

        public static void Print<T>(T item) where T : struct => Console.WriteLine($"{item}");

        public static void Print<T>(IEnumerable<T> list, string separator = ", ") => Console.WriteLine(string.Join(separator, list));

        public static void Print<T>(T[,] matrix)
        {
            for (var x = 0; x < matrix.GetLength(0); x++)
                Print(Enumerable.Range(0, matrix.GetLength(1)).Select(y => matrix[x, y]).ToArray(), "");
        }

        public static void Print<T1, T2>(Dictionary<T1, T2> dictionary)
        {
            foreach ((T1 key, T2 value) in dictionary)
                Console.WriteLine($"{key}, {value}");
        }

        public static void PrintDeepDictionary<T1, T2>(Dictionary<T1, T2> dictionary, string separator = ", ") where T2 : IEnumerable<object>
        {
            foreach ((T1 key, T2 value) in dictionary)
                Console.WriteLine($"{key}, {string.Join(separator, value)}");
        }

        //https://rosettacode.org/wiki/Chinese_remainder_theorem#C.23
        public static class ChineseRemainderTheorem
        {
            public static long Solve(int[] n, int[] a)
            {
                var prod = n.Aggregate((long)1, (i, j) => i * j);
                long sm = 0;

                for (var i = 0; i < n.Length; i++)
                {
                    var p = prod / n[i];
                    sm += a[i] * ModularMultiplicativeInverse(p, n[i]) * p;
                }

                return sm % prod;
            }

            private static long ModularMultiplicativeInverse(long a, long mod)
            {
                var b = a % mod;
                for (var x = 1; x < mod; x++)
                    if ((b * x) % mod == 1)
                        return x;

                return 1;
            }
        }
    }
}