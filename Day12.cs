using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public sealed class Day12 : DayBase<List<(char, int)>>
    {
        protected override int Day { get; } = 12;

        public Day12() => Input = ProcessInput(ReadInput());

        protected override List<(char, int)> ProcessInput(List<string> rawInput) => rawInput.Select(x => (x[0], int.Parse(x[1..]))).ToList();

        public override long Part1()
        {
            var currentPosition = (x: 0, y: 0);
            var directions = new[] {'N', 'E', 'S', 'W'};
            var currentDirection = directions[1];

            void Move(char action, int value)
            {
                switch (action)
                {
                    case 'N':
                        currentPosition.y += value;
                        break;
                    case 'S':
                        currentPosition.y -= value;
                        break;
                    case 'E':
                        currentPosition.x += value;
                        break;
                    case 'W':
                        currentPosition.x -= value;
                        break;
                    case 'L':
                        currentDirection = directions[(4 - (value / 90) + Array.IndexOf(directions, currentDirection)) % 4];
                        break;
                    case 'R':
                        currentDirection = directions[(value / 90 + Array.IndexOf(directions, currentDirection)) % 4];
                        break;
                    case 'F':
                        Move(currentDirection, value);
                        break;
                }
            }

            foreach ((char action, int value)in Input)
                Move(action, value);

            return Math.Abs(currentPosition.x) + Math.Abs(currentPosition.y);
        }

        public override long Part2()
        {
            var currentPosition = (x: 0, y: 0);
            var currentWayPointPosition = (x: 10, y: 1);

            void Move(char action, int value)
            {
                switch (action)
                {
                    case 'N':
                        currentWayPointPosition.y += value;
                        break;
                    case 'S':
                        currentWayPointPosition.y -= value;
                        break;
                    case 'E':
                        currentWayPointPosition.x += value;
                        break;
                    case 'W':
                        currentWayPointPosition.x -= value;
                        break;
                    case 'L':
                        currentWayPointPosition = Utils.RotatePoint(currentWayPointPosition, value);
                        break;
                    case 'R':
                        currentWayPointPosition = Utils.RotatePoint(currentWayPointPosition, -value);
                        break;
                    case 'F':
                        currentPosition.x += currentWayPointPosition.x * value;
                        currentPosition.y += currentWayPointPosition.y * value;
                        break;
                }
            }

            foreach ((char action, int value)in Input)
                Move(action, value);

            return Math.Abs(currentPosition.x) + Math.Abs(currentPosition.y);
        }
    }
}