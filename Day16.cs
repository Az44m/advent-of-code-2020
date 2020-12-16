using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public sealed class Day16 : DayBase<(List<(int, int, int, int)>, int[], List<int[]>)>
    {
        protected override int Day { get; } = 16;

        public Day16() => Input = ProcessInput(ReadInput());

        protected override (List<(int, int, int, int)>, int[], List<int[]>) ProcessInput(List<string> rawInput)
        {
            var rules = new List<(int, int, int, int)>();
            int[] myTicket = null;
            var nearbyTickets = new List<int[]>();

            var readNearbyTickets = false;

            for (var i = 0; i < rawInput.Count; i++)
            {
                var line = rawInput[i];
                var ruleLine = line.Split(": ");

                if (readNearbyTickets)
                {
                    nearbyTickets.Add(line.Split(',').Select(int.Parse).ToArray());
                }

                if (ruleLine.Length == 2)
                {
                    var currentRules = ruleLine[1]
                        .Split(" or ")
                        .Select(rule => rule.Split('-'))
                        .SelectMany(r => r.Select(int.Parse)).ToArray();
                    rules.Add((currentRules[0], currentRules[1], currentRules[2], currentRules[3]));
                }

                if (line.StartsWith("your ticket:"))
                {
                    myTicket = rawInput[i + 1].Split(',').Select(int.Parse).ToArray();
                    i++;
                    continue;
                }

                if (line.StartsWith("nearby tickets:"))
                    readNearbyTickets = true;
            }

            return (rules, myTicket, nearbyTickets);
        }

        public override long Part1()
        {
            (List<(int, int, int, int)> rules, int[] myTicket, List<int[]> nearbyTickets) = Input;
            (int count, _) = ProcessTickets(rules, myTicket, nearbyTickets);

            return count;
        }

        public override long Part2()
        {
            (List<(int, int, int, int)> rules, int[] myTicket, List<int[]> nearbyTickets) = Input;
            (int _, bool[,] validTicketFieldMap) = ProcessTickets(rules, myTicket, nearbyTickets);

            var correctIndexes = Enumerable.Repeat(-1, myTicket.Length).ToArray();
            var takenIndexes = new bool[myTicket.Length];

            while (correctIndexes.Any(index => index < 0))
            {
                for (var i = 0; i < myTicket.Length; i++)
                {
                    var validIndexes = new List<int>();
                    for (var j = 0; j < myTicket.Length; j++)
                    {
                        var isValid = validTicketFieldMap[i, j] && !takenIndexes[j];
                        if (isValid)
                            validIndexes.Add(j);
                    }

                    if (validIndexes.Count == 1)
                    {
                        var validIndex = validIndexes[0];
                        correctIndexes[i] = validIndex;
                        takenIndexes[validIndex] = true;
                    }
                }
            }

            return correctIndexes
                .Select(index => correctIndexes[index] < 6 ? (long)myTicket[index] : (long)1)
                .ToList()
                .Aggregate((x, y) => x * y);
        }

        private (int, bool[,]) ProcessTickets(List<(int, int, int, int)> rules, int[] myTicket, List<int[]> nearbyTickets)
        {
            var count = 0;
            
            var validTicketFieldMap = new bool[myTicket.Length, myTicket.Length];
            for (var i = 0; i < validTicketFieldMap.GetLength(0); i++)
            for (var j = 0; j < validTicketFieldMap.GetLength(1); j++)
                validTicketFieldMap[i, j] = true;

            foreach (var nearbyTicket in nearbyTickets)
            {
                var isValidTicket = true;
                foreach (var ticketField in nearbyTicket)
                {
                    var isValidField = false;
                    foreach (var rule in rules)
                    {
                        if (IsValidTicketField(rule, ticketField))
                            isValidField = true;
                    }

                    if (!isValidField)
                    {
                        count += ticketField;
                        isValidTicket = false;
                    }
                }

                if (isValidTicket)
                {
                    for (var i = 0; i < nearbyTicket.Length; i++)
                    {
                        var ticketField = nearbyTicket[i];
                        for (var j = 0; j < rules.Count; j++)
                        {
                            if (!IsValidTicketField(rules[j], ticketField))
                                validTicketFieldMap[i, j] = false;
                        }
                    }
                }
            }

            return (count, validTicketFieldMap);
        }

        private bool IsValidTicketField((int, int, int, int) rule, int ticketField)
        {
            (int r1, int r2, int r3, int r4) = rule;
            return (ticketField >= r1 && ticketField <= r2) || (ticketField >= r3 && ticketField <= r4);
        }
    }
}