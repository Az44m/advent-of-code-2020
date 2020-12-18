using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public sealed class Day18 : DayBase<List<string>>
    {
        protected override int Day { get; } = 18;

        public Day18() => Input = ProcessInput(ReadInput());

        protected override List<string> ProcessInput(List<string> rawInput) => rawInput;

        public override long Part1()
        {
            long sum = 0;

            foreach (var operationString in Input)
            {
                var resolved = ResolveComplexOperationString(operationString, false);
                sum += long.Parse(resolved);
            }

            return sum;
        }

        public override long Part2()
        {
            long sum = 0;

            foreach (var operationString in Input)
            {
                var resolved = ResolveComplexOperationString(operationString, true);
                sum += long.Parse(resolved);
            }

            return sum;
        }

        private string ResolveComplexOperationString(string operationString, bool addFirst)
        {
            while (true)
            {
                var unboxedOperationString = UnboxOperationString(operationString).ToArray();
                if (unboxedOperationString.Length == 1)
                    return ResolveSingleOperationString(operationString, addFirst);

                foreach (var singleOperationString in unboxedOperationString)
                {
                    if (!singleOperationString.Contains('('))
                    {
                        var resolvedOperationString = ResolveSingleOperationString(singleOperationString, addFirst);
                        operationString = operationString.Replace($"({singleOperationString})", $"{resolvedOperationString}");
                        break;
                    }
                }
            }
        }

        private string ResolveSingleOperationString(string singleOperation, bool addFirst)
        {
            var operations = singleOperation.Split(' ');
            while (operations.Length != 1)
            {
                var operatorIndex = addFirst && operations.Contains("+") ? operations.ToList().IndexOf("+") : 1;
                var operand1 = long.Parse(operations[operatorIndex - 1]);
                var operand2 = long.Parse(operations[operatorIndex + 1]);
                var @operator = operations[operatorIndex];
                var operation = $"{operand1} {@operator} {operand2}";

                if (@operator == "+")
                    singleOperation = ReplaceFirst(singleOperation, operation, $"{operand1 + operand2}");
                else if (@operator == "*")
                    singleOperation = ReplaceFirst(singleOperation, operation, $"{operand1 * operand2}");

                operations = singleOperation.Split(' ');
            }

            return singleOperation;
        }

        private string ReplaceFirst(string text, string searchTerm, string replaceTerm)
        {
            var index = text.IndexOf(searchTerm, StringComparison.Ordinal);
            if (index < 0)
                return text;

            return text.Substring(0, index) + replaceTerm + text.Substring(index + searchTerm.Length);
        }


        private IEnumerable<string> UnboxOperationString(string value)
        {
            var parenthesisIndexes = new Stack<int>();

            for (var i = 0; i < value.Length; ++i)
            {
                var c = value[i];

                if (c == '(')
                {
                    parenthesisIndexes.Push(i);
                }
                else if (c == ')')
                {
                    var openBracket = parenthesisIndexes.Pop();
                    yield return value.Substring(openBracket + 1, i - openBracket - 1);
                }
            }

            yield return value;
        }
    }
}