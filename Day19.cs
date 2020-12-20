using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    public sealed class Day19 : DayBase<(Dictionary<int, string>, List<string>)>
    {
        protected override int Day { get; } = 19;

        public Day19() => Input = ProcessInput(ReadInput());

        protected override (Dictionary<int, string>, List<string>) ProcessInput(List<string> rawInput)
        {
            var rules = new Dictionary<int, string>();
            var messages = new List<string>();

            var parseMessages = false;
            foreach (var line in rawInput)
            {
                if (parseMessages)
                {
                    messages.Add(line);
                }
                else
                {
                    if (line == string.Empty)
                    {
                        parseMessages = true;
                    }
                    else
                    {
                        var ruleParts = line.Split(": ");
                        rules[int.Parse(ruleParts[0])] = ruleParts[1];
                    }
                }
            }

            return (rules.OrderBy(rule => rule.Key).ToDictionary(rule => rule.Key, x => x.Value), messages);
        }

        private Dictionary<int, string> _regexCache;

        public override long Part1()
        {
            _regexCache = new Dictionary<int, string>();
            var rules = new Dictionary<int, string>(Input.Item1);
            var rule0Regex = $"^{GetRegexForRuleId(0, rules)}$";
            return Input.Item2.Count(message => Regex.IsMatch(message, rule0Regex, RegexOptions.Compiled));
        }

        public override long Part2()
        {
            _regexCache = new Dictionary<int, string>();
            var rules = new Dictionary<int, string>(Input.Item1);

            (string rule8Regex, string rule11Regex) = GenerateCustomRule8And11(rules);
            rules[8] = rule8Regex;
            rules[11] = rule11Regex;

            _regexCache[8] = rule8Regex;
            _regexCache[11] = rule11Regex;

            var rule0Regex = $"^{GetRegexForRuleId(0, rules)}$";
            return Input.Item2.Count(message => Regex.IsMatch(message, rule0Regex, RegexOptions.Compiled));
        }

        private (string, string) GenerateCustomRule8And11(Dictionary<int, string> rules)
        {
            // 8: 42 | 42 8
            // 11: 42 31 | 42 11 31

            var rule42Regex = GetRegexForRuleId(42, rules);
            var rule31Regex = GetRegexForRuleId(31, rules);

            var rule8Regex = $"({rule42Regex})+";

            const int magicNumber = 5;
            var rule42RegexPart = Enumerable.Repeat($"{rule42Regex}(", magicNumber);
            var rule31RegexPart = Enumerable.Repeat($")?{rule31Regex}", magicNumber);
            var rule11Regex = $"({string.Join("", rule42RegexPart)}{string.Join("", rule31RegexPart)})";

            return (rule8Regex, rule11Regex);
        }

        private string ConvertRuleToRegex(string rule, Dictionary<int, string> rules)
        {
            string regex;

            if (rule.Contains('"'))
            {
                regex = $"{rule[1]}";
            }
            else if (rule.Contains(" | "))
            {
                var parts = rule.Split(" | ");
                var part0 = parts[0].Split(' ');
                var part1 = parts[1].Split(' ');
                var regexList0 = part0.Select(part => $"{GetRegexForRuleId(int.Parse(part), rules)}");
                var regexList1 = part1.Select(part => $"{GetRegexForRuleId(int.Parse(part), rules)}");
                regex = $"({string.Join("", regexList0)}|{string.Join("", regexList1)})";
            }
            else
            {
                var parts = rule.Split(' ');
                regex = string.Join("", parts.Select(part => $"{GetRegexForRuleId(int.Parse(part), rules)}"));
            }

            return regex;
        }

        private string GetRegexForRuleId(int index, Dictionary<int, string> rules)
        {
            if (_regexCache.ContainsKey(index))
                return _regexCache[index];

            var regex = ConvertRuleToRegex(rules[index], rules);
            _regexCache[index] = regex;

            return regex;
        }
    }
}