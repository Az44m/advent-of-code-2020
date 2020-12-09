using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public class Bag
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }

    public sealed class Day7 : DayBase<Dictionary<string, List<Bag>>>
    {
        protected override int Day { get; } = 7;
        public Day7() => Input = ProcessInput(ReadInput());

        protected override Dictionary<string, List<Bag>> ProcessInput(List<string> rawInput)
        {
            var bagRuleMap = new Dictionary<string, List<Bag>>();

            foreach (string rawBagRule in rawInput)
            {
                var bagRules = rawBagRule
                    .Replace(" bags.", "")
                    .Replace(" bags", "")
                    .Replace(" bag.", "")
                    .Replace(" bag", "")
                    .Split(", ");

                var outerBagRule = bagRules[0].Split(" contain ");
                bagRuleMap[outerBagRule[0]] = new List<Bag>();

                if (outerBagRule[1] != "no other")
                {
                    var firstInnerBagRule = outerBagRule[1].Split(' ');
                    bagRuleMap[outerBagRule[0]].Add(new Bag
                    {
                        Name = $"{firstInnerBagRule[1]} {firstInnerBagRule[2]}", Count = int.Parse(firstInnerBagRule[0])
                    });
                }

                for (int i = 1; i < bagRules.Length; i++)
                {
                    var innerBagRules = bagRules[i];
                    var innerBagRule = innerBagRules.Split(' ');
                    bagRuleMap[outerBagRule[0]].Add(new Bag {Name = $"{innerBagRule[1]} {innerBagRule[2]}", Count = int.Parse(innerBagRule[0])});
                }
            }

            return bagRuleMap;
        }

        public override long Part1()
        {
            var bagsToCheck = new List<string> {"shiny gold"};
            var resultList = new HashSet<string>();

            while (bagsToCheck.ToList().Any())
            {
                var currentBag = bagsToCheck.First();
                resultList.Add(currentBag);
                var bags = Input.Where(bagRuleEntry => bagRuleEntry.Value.Any(bag => bag.Name == currentBag)).Select(kvp => kvp.Key);
                bagsToCheck.Remove(currentBag);
                bagsToCheck.AddRange(bags);
            }

            return resultList.Count - 1;
        }

        public override long Part2()
        {
            var bagsToCheck = new Dictionary<string, int> {{"shiny gold", 1}};
            var result = 0;

            while (bagsToCheck.ToList().Any())
            {
                var currentBag = bagsToCheck.First().Key;
                foreach (var innerBags in Input[currentBag])
                {
                    var newCount = bagsToCheck[currentBag] * innerBags.Count;
                    if (bagsToCheck.ContainsKey(innerBags.Name))
                        bagsToCheck[innerBags.Name] += newCount;
                    else
                        bagsToCheck.Add(innerBags.Name, newCount);
                }

                result += bagsToCheck[currentBag];
                bagsToCheck.Remove(currentBag);
            }

            return result - 1;
        }
    }
}