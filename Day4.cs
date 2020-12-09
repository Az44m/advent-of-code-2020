using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    public sealed class Day4 : DayBase<List<string>>
    {
        protected override int Day { get; } = 4;

        public Day4() => Input = ProcessInput(ReadInput());

        protected override List<string> ProcessInput(List<string> rawInput) => Utils.ConcatGroupOfLines(rawInput, " ", string.Empty);

        public override long Part1() => CountValidPassports((_, __) => true);

        public override long Part2()
        {
            return CountValidPassports((key, value) =>
            {
                switch (key)
                {
                    case "byr":
                        var byr = int.Parse(value);
                        return byr >= 1920 && byr <= 2002;
                    case "iyr":
                        var iyr = int.Parse(value);
                        return iyr >= 2010 && iyr <= 2020;
                    case "eyr":
                        var eyr = int.Parse(value);
                        return eyr >= 2020 && eyr <= 2030;
                    case "hgt":
                        if (value.Contains("cm"))
                        {
                            var height = int.Parse(value.Trim('c', 'm'));
                            return height >= 150 && height <= 193;
                        }
                        else
                        {
                            var height = int.Parse(value.Trim('i', 'n'));
                            return height >= 59 && height <= 76;
                        }
                    case "hcl":
                        return Regex.IsMatch(value, "^#[a-z0-9]{6}$");
                    case "ecl":
                        return new[] {"amb", "blu", "brn", "gry", "grn", "hzl", "oth"}.Contains(value);
                    case "pid":
                        return Regex.IsMatch(value, "^[0-9]{9}$");
                }

                return false;
            });
        }

        private int CountValidPassports(Func<string, string, bool> isValidValue)
        {
            var validPassports = 0;
            foreach (var passport in Input)
            {
                var fields = passport
                    .Split(' ')
                    .ToDictionary(field => field.Split(':')[0], field => field.Split(':')[1]);

                var isValid = true;
                var requiredKeys = new[] {"byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"};
                foreach (var requiredKey in requiredKeys)
                {
                    if (!fields.TryGetValue(requiredKey, out var fieldValue))
                    {
                        isValid = false;
                        break;
                    }

                    if (!isValidValue(requiredKey, fieldValue))
                    {
                        isValid = false;
                        break;
                    }
                }

                if (isValid)
                    validPassports++;
            }

            return validPassports;
        }
    }
}