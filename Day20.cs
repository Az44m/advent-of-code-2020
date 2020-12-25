using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    public sealed class Day20 : DayBase<IEnumerable<(int, char[,])>>
    {
        protected override int Day { get; } = 20;

        public Day20() => Input = ProcessInput(ReadInput());

        protected override IEnumerable<(int, char[,])> ProcessInput(List<string> rawInput)
        {
            var flattenedImageFragments = Utils.ConcatGroupOfLines(rawInput, "\n", string.Empty);

            foreach (var line in flattenedImageFragments)
            {
                if (line == string.Empty)
                    continue;

                var imageFragmentWithTitle = line.Split('\n');
                var tile = imageFragmentWithTitle[0];
                var id = tile.Replace("Tile ", "").Replace(":", "");
                yield return (int.Parse(id), Utils.ToCharMatrix(imageFragmentWithTitle.Skip(1).ToList()));
            }
        }

        public override long Part1()
        {
            List<(int id, char[,] imageFragment)> imageFragments = Input.ToList();

            var fragmentIdsWithEdges = imageFragments
                .Select(fragment =>
                {
                    var row0 = GetMatrixRow(fragment.imageFragment, 0);
                    var rowX = GetMatrixRow(fragment.imageFragment, fragment.imageFragment.GetLength(0) - 1);
                    var col0 = GetMatrixColumn(fragment.imageFragment, 0);
                    var colX = GetMatrixColumn(fragment.imageFragment, fragment.imageFragment.GetLength(1) - 1);
                    var edges = new[]
                    {
                        new string(row0), new string(rowX), new string(col0), new string(colX), new string(row0.Reverse().ToArray()),
                        new string(rowX.Reverse().ToArray()), new string(col0.Reverse().ToArray()), new string(colX.Reverse().ToArray())
                    };
                    return (fragment.id, edges);
                }).ToList();


            var fragmentIdsWithSharedEdgeCount = fragmentIdsWithEdges
                .Select(fragmentIdWithEdges1 =>
                {
                    var sharedEdgeCount = fragmentIdsWithEdges
                        .Where(fragmentIdWithEdges2 => fragmentIdWithEdges2.id != fragmentIdWithEdges1.id)
                        .Sum(fragmentIdWithEdges2 => fragmentIdWithEdges1.edges.Intersect(fragmentIdWithEdges2.edges).Count());

                    return (fragmentIdWithEdges1.id, sharedEdgeCount);
                });

            return fragmentIdsWithSharedEdgeCount
                .Where(fragment => fragment.sharedEdgeCount == 4)
                .Select(fragment => (long)fragment.id)
                .Aggregate((id1, id2) => id1 * id2);
        }


        public override long Part2()
        {
            var bigImage = ConstructBigImage();
            var trimmedImage = TrimBigImage(bigImage);

            var seaMonster = new[]
            {
                "^.?.?.?.?.?.?.?.?.?.?.?.?.?.?.?.?.?.?#.?$",
                "^#.?.?.?.?##.?.?.?.?##.?.?.?.?###$",
                "^.?#.?.?#.?.?#.?.?#.?.?#.?.?#.?.?.?$"
            };

            (int seaMonsterWidth, int seaMonsterHeight) = (20, 3);

            var bigImageTransformations = GetTransformations(trimmedImage);

            foreach (var transformedBigImage in bigImageTransformations)
            {
                var seaMonsterCount = 0;
                for (var x = 0; x < transformedBigImage.GetLength(0) - seaMonsterHeight; x++)
                {
                    if (x > 1 && seaMonsterCount == 0)
                    {
                        Console.WriteLine($"No sea monster in the {x}. row. Go to next transformation.");
                        break;
                    }

                    var maxY = transformedBigImage.GetLength(1);
                    for (var y = 0; y < maxY - seaMonsterWidth; y++)
                    {
                        var seaMonster1 = new string(GetMatrixRow(transformedBigImage, x))[y..^(maxY - seaMonsterWidth - y)];
                        var seaMonster2 = new string(GetMatrixRow(transformedBigImage, x + 1))[y..^(maxY - seaMonsterWidth - y)];
                        var seaMonster3 = new string(GetMatrixRow(transformedBigImage, x + 2))[y..^(maxY - seaMonsterWidth - y)];

                        if (Regex.IsMatch(seaMonster1, seaMonster[0], RegexOptions.Compiled) &&
                            Regex.IsMatch(seaMonster2, seaMonster[1], RegexOptions.Compiled) &&
                            Regex.IsMatch(seaMonster3, seaMonster[2], RegexOptions.Compiled))
                        {
                            Console.WriteLine($"Found a sea monster at {(x, y)}");
                            y += seaMonsterWidth;
                            seaMonsterCount++;
                        }
                    }
                }

                if (seaMonsterCount != 0)
                {
                    var roughCount = Utils.CountInMatrix(transformedBigImage, '#');
                    return roughCount - seaMonsterCount * 15;
                }
            }

            return -1;
        }

        private Dictionary<(int x, int y), char> ConstructBigImage()
        {
            var bigImage = new Dictionary<(int x, int y), char>();
            var fragmentsToCheck = new Queue<((int x, int y)origin, (int id, char[,]imageFragment) fragment)>();
            var bigImageFragments = new HashSet<int>();
            var imageFragments = new Queue<(int id, char[,] imageFragment)>(Input);
            var firstFragment = imageFragments.Dequeue();

            AddFragmentToBigImage((0, 0), firstFragment.imageFragment, bigImage);
            bigImageFragments.Add(firstFragment.id);
            fragmentsToCheck.Enqueue(((0, 0), firstFragment));

            while (fragmentsToCheck.Count > 0)
            {
                ((int x, int y) currentOrigin, (int id, char[,] imageFragment) currentFragment) = fragmentsToCheck.Dequeue();

                for (var i = 0; i < 4; i++)
                {
                    var index = -1;
                    var possibleAdjacentFragments = imageFragments.ToArray();
                    while (++index < possibleAdjacentFragments.Length)
                    {
                        string currentImageEdge;
                        (int x, int y) pos;
                        var adjacentFragment = possibleAdjacentFragments[index];

                        if (i == 0)
                        {
                            pos = (0, 1); // right
                            currentImageEdge =
                                new string(GetMatrixColumn(currentFragment.imageFragment, currentFragment.imageFragment.GetUpperBound(0)));
                        }
                        else if (i == 1)
                        {
                            pos = (0, -1); // left
                            currentImageEdge = new string(GetMatrixColumn(currentFragment.imageFragment, 0));
                        }
                        else if (i == 2)
                        {
                            pos = (-1, 0); // top
                            currentImageEdge = new string(GetMatrixRow(currentFragment.imageFragment, 0));
                        }
                        else
                        {
                            pos = (1, 0); // bottom
                            currentImageEdge =
                                new string(GetMatrixRow(currentFragment.imageFragment, currentFragment.imageFragment.GetUpperBound(1)));
                        }

                        var fragmentPos = (currentOrigin.x + pos.x, currentOrigin.y + pos.y);

                        if (bigImageFragments.Contains(adjacentFragment.id))
                            continue;

                        var transformations = GetTransformations(adjacentFragment);
                        foreach (var transformation in transformations)
                        {
                            string transformedImageEdge;

                            if (i == 0)
                                transformedImageEdge = new string(GetMatrixColumn(transformation, 0));
                            else if (i == 1)
                                transformedImageEdge = new string(GetMatrixColumn(transformation, transformation.GetUpperBound(0)));
                            else if (i == 2)
                                transformedImageEdge = new string(GetMatrixRow(transformation, transformation.GetUpperBound(1)));
                            else
                                transformedImageEdge = new string(GetMatrixRow(transformation, 0));

                            if (string.Equals(currentImageEdge, transformedImageEdge))
                            {
                                AddFragmentToBigImage(fragmentPos, transformation, bigImage);
                                bigImageFragments.Add(adjacentFragment.id);
                                fragmentsToCheck.Enqueue((fragmentPos, (adjacentFragment.id, transformation)));
                                break;
                            }
                        }
                    }
                }
            }

            return bigImage;
        }

        private void AddFragmentToBigImage((int x, int y) offset, char[,] imageFragment, Dictionary<(int x, int y), char> bigImage)
        {
            for (int x = 0; x < imageFragment.GetLength(0); x++)
            for (int y = 0; y < imageFragment.GetLength(1); y++)
                bigImage.Add((x + offset.x * 10, +y + offset.y * 10), imageFragment[x, y]);
        }

        private readonly Dictionary<int, List<char[,]>> _imageFragmentCache = new Dictionary<int, List<char[,]>>();

        private List<char[,]> GetTransformations((int id, char[,] imageFragment) fragment)
        {
            if (_imageFragmentCache.ContainsKey(fragment.id))
                return _imageFragmentCache[fragment.id];

            var transformations = GetTransformations(fragment.imageFragment);
            _imageFragmentCache[fragment.id] = transformations;

            return transformations;
        }

        private List<char[,]> GetTransformations(char[,] fragment)
        {
            List<char[,]> transformations = new List<char[,]>();
            transformations.Add(fragment);
            transformations.Add(FlipMatrixVertically(fragment));
            transformations.Add(FlipMatrixHorizontally(fragment));
            var rotated = RotateMatrix(fragment);
            transformations.Add(rotated);
            var rotated2 = RotateMatrix(rotated);
            transformations.Add(rotated2);
            var rotated3 = RotateMatrix(rotated2);
            transformations.Add(rotated3);
            transformations.Add(FlipMatrixHorizontally(rotated));
            transformations.Add(FlipMatrixHorizontally(rotated2));
            transformations.Add(FlipMatrixHorizontally(rotated3));

            return transformations;
        }

        private char[,] RotateMatrix(char[,] matrix)
        {
            var maxY = matrix.GetLength(0);
            var maxX = matrix.GetLength(1);
            var newMatrix = new char[maxX, maxY];

            for (var x = 0; x < maxX; x++)
            for (var y = 0; y < maxY; y++)
                newMatrix[x, y] = matrix[y, maxX - x - 1];

            return newMatrix;
        }

        private char[,] FlipMatrixVertically(char[,] matrix)
        {
            var maxY = matrix.GetLength(1);
            var maxX = matrix.GetLength(0);
            var newMatrix = new char[maxX, maxY];

            for (var x = 0; x < maxX; x++)
            for (var y = 0; y < maxY; y++)
                newMatrix[x, y] = matrix[x, maxY - y - 1];

            return newMatrix;
        }

        private char[,] FlipMatrixHorizontally(char[,] matrix)
        {
            var maxY = matrix.GetLength(1);
            var maxX = matrix.GetLength(0);
            var newMatrix = new char[maxX, maxY];

            for (var x = 0; x < maxX; x++)
            for (var y = 0; y < maxY; y++)
                newMatrix[x, y] = matrix[maxX - x - 1, y];

            return newMatrix;
        }

        public char[] GetMatrixColumn(char[,] matrix, int columnNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(0))
                .Select(x => matrix[x, columnNumber])
                .ToArray();
        }

        public char[] GetMatrixRow(char[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1))
                .Select(x => matrix[rowNumber, x])
                .ToArray();
        }

        private char[,] TrimBigImage(Dictionary<(int x, int y), char> bigImage)
        {
            var minX = bigImage.Min(f => f.Key.x);
            var maxX = bigImage.Max(f => f.Key.x);
            var minY = bigImage.Min(f => f.Key.y);
            var maxY = bigImage.Max(f => f.Key.y);

            var bigImageWidth = maxX - minX + 1;
            var bigImageHeight = maxY - minY + 1;
            var skippedCount = 10 / 2;
            var trimmedImage = new char[bigImageWidth - (bigImageWidth / skippedCount), bigImageHeight - (bigImageHeight / skippedCount)];

            var x1 = 0;

            for (var x = minX; x < maxX; x++)
            {
                if ((x - minX) % 10 == 0 || (x - minX + 1) % 10 == 0)
                    continue;

                var y1 = 0;

                for (var y = minY; y < maxY; y++)
                {
                    if ((y - minY + 1) % 10 == 0 || (y - minY) % 10 == 0)
                        continue;

                    trimmedImage[x1, y1] = bigImage[(x, y)];
                    y1++;
                }

                x1++;
            }

            return trimmedImage;
        }
    }
}