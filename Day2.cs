using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public class PasswordCheck
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public char CharToCheck { get; set; }
        public string Password { get; set; }
    }

    public sealed class Day2 : DayBase<PasswordCheck[]>
    {
        protected override int Day { get; } = 2;
        public Day2() => Input = ProcessInput(ReadInput());

        protected override PasswordCheck[] ProcessInput(List<string> rawInput)
        {
            return rawInput
                .Select(inputRow =>
                {
                    var input = inputRow.Split(new[] {'-', ' ', ':'}, StringSplitOptions.RemoveEmptyEntries);
                    return new PasswordCheck
                    {
                        Min = int.Parse(input[0]),
                        Max = int.Parse(input[1]),
                        CharToCheck = char.Parse(input[2]),
                        Password = input[3]
                    };
                })
                .ToArray();
        }

        public override int Part1()
        {
            return CountValidPasswords((min, max, password, charToCheck) =>
            {
                var charCount = password.Count(c => c == charToCheck);
                return charCount >= min && charCount <= max;
            });
        }

        public override int Part2()
        {
            return CountValidPasswords((min, max, password, charToCheck) =>
            {
                return password[min - 1] == charToCheck ^ password[max - 1] == charToCheck;
            });
        }

        private int CountValidPasswords(Func<int, int, string, char, bool> validatorFunction)
        {
            return Input.Sum(pwCheck => validatorFunction(pwCheck.Min, pwCheck.Max, pwCheck.Password, pwCheck.CharToCheck) ? 1 : 0);
        }
    }
}
