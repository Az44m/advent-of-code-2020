using System.Collections.Generic;
using System.Linq;

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
                    var row0 = GetRow(fragment.imageFragment, 0);
                    var rowX = GetRow(fragment.imageFragment, fragment.imageFragment.GetLength(0) - 1);
                    var col0 = GetColumn(fragment.imageFragment, 0);
                    var colX = GetColumn(fragment.imageFragment, fragment.imageFragment.GetLength(1) - 1);
                    var edges = new[]
                    {
                        new string(row0),
                        new string(rowX),
                        new string(col0),
                        new string(colX),
                        new string(row0.Reverse().ToArray()),
                        new string(rowX.Reverse().ToArray()),
                        new string(col0.Reverse().ToArray()),
                        new string(colX.Reverse().ToArray())
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
            return -1;
        }

        public char[] GetColumn(char[,] matrix, int columnNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(0))
                .Select(x => matrix[x, columnNumber])
                .ToArray();
        }

        public char[] GetRow(char[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1))
                .Select(x => matrix[rowNumber, x])
                .ToArray();
        }
    }
}