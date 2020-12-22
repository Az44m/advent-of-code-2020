using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public sealed class Day22 : DayBase<(List<int>, List<int>)>
    {
        protected override int Day { get; } = 22;

        public Day22() => Input = ProcessInput(ReadInput());

        protected override (List<int>, List<int>) ProcessInput(List<string> rawInput)
        {
            var player1Deck = new List<int>();
            var player2Deck = new List<int>();

            var parsePlayer2 = false;
            foreach (var line in rawInput)
            {
                if (line == "")
                    continue;

                if (line.StartsWith("Player 1"))
                    continue;

                if (line.StartsWith("Player 2"))
                {
                    parsePlayer2 = true;
                    continue;
                }

                if (parsePlayer2)
                    player2Deck.Add(int.Parse(line));
                else
                    player1Deck.Add(int.Parse(line));
            }

            return (player1Deck, player2Deck);
        }

        public override long Part1()
        {
            var player1Deck = new Queue<int>(Input.Item1);
            var player2Deck = new Queue<int>(Input.Item2);

            while (player1Deck.Count > 0 && player2Deck.Count > 0)
            {
                var player1Card = player1Deck.Dequeue();
                var player2Card = player2Deck.Dequeue();

                if (player1Card > player2Card)
                {
                    player1Deck.Enqueue(player1Card);
                    player1Deck.Enqueue(player2Card);
                }
                else
                {
                    player2Deck.Enqueue(player2Card);
                    player2Deck.Enqueue(player1Card);
                }
            }

            return player1Deck.Any()
                ? player1Deck.Reverse().Select((card, i) => card * (i + 1)).Sum()
                : player2Deck.Reverse().Select((card, i) => card * (i + 1)).Sum();
        }

        public override long Part2()
        {
            var player1Deck = new Queue<int>(Input.Item1);
            var player2Deck = new Queue<int>(Input.Item2);

            var winner = PlayGame(player1Deck, player2Deck);

            return winner
                ? player1Deck.Reverse().Select((card, i) => card * (i + 1)).Sum()
                : player2Deck.Reverse().Select((card, i) => card * (i + 1)).Sum();
        }

        private bool PlayGame(Queue<int> player1Deck, Queue<int> player2Deck)
        {
            var previousRounds = new HashSet<string>();

            while (player1Deck.Count > 0 && player2Deck.Count > 0)
            {
                var currentRound = $"{string.Join(",", player1Deck)}-{string.Join(",", player2Deck)}";

                if (previousRounds.Contains(currentRound))
                    return true;

                previousRounds.Add(currentRound);

                var player1Card = player1Deck.Dequeue();
                var player2Card = player2Deck.Dequeue();

                if (player1Deck.Count >= player1Card && player2Deck.Count >= player2Card)
                {
                    var player1RecursiveCombatDeck = new Queue<int>(player1Deck.Take(player1Card));
                    var player2RecursiveCombatDeck = new Queue<int>(player2Deck.Take(player2Card));
                    var recursiveCombatWinner = PlayGame(player1RecursiveCombatDeck, player2RecursiveCombatDeck);
                    if (recursiveCombatWinner)
                    {
                        player1Deck.Enqueue(player1Card);
                        player1Deck.Enqueue(player2Card);
                    }
                    else
                    {
                        player2Deck.Enqueue(player2Card);
                        player2Deck.Enqueue(player1Card);
                    }
                }
                else
                {
                    if (player1Card > player2Card)
                    {
                        player1Deck.Enqueue(player1Card);
                        player1Deck.Enqueue(player2Card);
                    }
                    else
                    {
                        player2Deck.Enqueue(player2Card);
                        player2Deck.Enqueue(player1Card);
                    }
                }
            }

            return player1Deck.Count > 0;
        }
    }
}