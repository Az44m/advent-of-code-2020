using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public sealed class Day23 : DayBase<IEnumerable<int>>
    {
        protected override int Day { get; } = 23;

        public Day23() => Input = ProcessInput(ReadInput());

        protected override IEnumerable<int> ProcessInput(List<string> rawInput) => rawInput[0].Select(x => int.Parse($"{x}"));

        public override long Part1()
        {
            var cup1 = RunGame(new LinkedList<int>(Input), 100);

            var labels = string.Empty;
            cup1 = GetNextOrFirst(cup1);

            for (int i = 0; i < 8; i++)
            {
                labels += cup1.Value.ToString();
                cup1 = GetNextOrFirst(cup1);
            }

            return long.Parse(labels);
        }

        public override long Part2()
        {
            var cups = new List<int>(Input);
            cups = cups.Concat(Enumerable.Range(cups.Max() + 1, 1000000 - cups.Count)).ToList();

            var cup1 = RunGame(new LinkedList<int>(cups), 10000000);
            cup1 = GetNextOrFirst(cup1);

            return (long)cup1.Value * GetNextOrFirst(cup1).Value;
        }

        private LinkedListNode<int> RunGame(LinkedList<int> cups, int iterations)
        {
            var cupIndexes = new Dictionary<int, LinkedListNode<int>>();
            var cupIndex = cups.First;
            while (cupIndex != null)
            {
                cupIndexes.Add(cupIndex.Value, cupIndex);
                cupIndex = cupIndex.Next;
            }

            var currentCup = cups.First;

            for (int i = 0; i < iterations; i++)
            {
                var pickup = new List<LinkedListNode<int>>();
                var cup1 = GetNextOrFirst(currentCup);
                pickup.Add(cup1);
                var cup2 = GetNextOrFirst(cup1);
                pickup.Add(cup2);
                var cup3 = GetNextOrFirst(cup2);
                pickup.Add(cup3);

                foreach (var cup in pickup)
                    cups.Remove(cup);

                var destinationCupLabel = currentCup.Value - 1;
                while (destinationCupLabel < 1 || destinationCupLabel == currentCup.Value || pickup.Any(cup => cup.Value == destinationCupLabel))
                {
                    destinationCupLabel--;
                    if (destinationCupLabel >= 1)
                        continue;
                    destinationCupLabel = cupIndexes.Count();
                }

                currentCup = GetNextOrFirst(currentCup);

                var destinationCup = cupIndexes[destinationCupLabel];

                foreach (var cup in pickup)
                {
                    cups.AddAfter(destinationCup, cup);
                    destinationCup = GetNextOrFirst(destinationCup);
                }
            }

            return cupIndexes[1];
        }

        private LinkedListNode<int> GetNextOrFirst(LinkedListNode<int> current) => current.Next ?? current.List?.First;
    }
}