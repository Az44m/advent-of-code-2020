﻿using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public sealed class Day6 : DayBase<List<string>>
    {
        protected override int Day { get; } = 6;

        public Day6() => Input = ProcessInput(ReadInput());

        protected override List<string> ProcessInput(List<string> rawInput)
        {
            var passports = new List<string>();
            var passportBuilder = new StringBuilder();

            foreach (var row in rawInput)
            {
                if (row != string.Empty)
                {
                    passportBuilder.Append($",{row}");
                }
                else
                {
                    passports.Add(passportBuilder.ToString().Trim());
                    passportBuilder.Clear();
                }
            }

            return passports;
        }

        public override int Part1()
        {
            return Input.Sum(word => word.ToHashSet().Count - 1);
        }

        public override int Part2()
        {
            return Input.Sum(word =>
            {
                var answersForAPerson = word.Split(',');
                var dict = new Dictionary<char, int>();
                foreach (var answer in answersForAPerson)
                {
                    foreach (var letter in answer)
                    {
                        if (dict.ContainsKey(letter))
                            dict[letter]++;
                        else
                            dict.Add(letter, 1);
                    }
                }

                return dict.Count(kvp => kvp.Value == answersForAPerson.Length - 1);
            });
        }
    }
}