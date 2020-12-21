using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public sealed class Day21 : DayBase<IEnumerable<(List<string>, List<string>)>>
    {
        protected override int Day { get; } = 21;

        public Day21() => Input = ProcessInput(ReadInput());

        protected override IEnumerable<(List<string>, List<string>)> ProcessInput(List<string> rawInput)
        {
            foreach (var line in rawInput)
            {
                var ingredients = new List<string>();
                var allergens = new List<string>();

                var split = line.Split(" (contains ");
                ingredients.AddRange(split[0].Split(' '));
                allergens.AddRange(split[1].Split(new[] {' ', ',', ')'}, StringSplitOptions.RemoveEmptyEntries));

                yield return (ingredients, allergens);
            }
        }

        public override long Part1()
        {
            List<(List<string> ingredients, List<string> allergens)> foods = Input.ToList();

            var allergenMap = MapAllergensToIngredients(foods);
            var allergenFreeIngredients = foods.SelectMany(x => x.ingredients).Except(allergenMap.Values).ToList();

            var count = foods.Sum(food =>
            {
                return allergenFreeIngredients.Count(ingredient => food.ingredients.Contains(ingredient));
            });

            return count;
        }

        public override long Part2()
        {
            List<(List<string> ingredients, List<string> allergens)> foods = Input.ToList();

            var allergenMap = MapAllergensToIngredients(foods);

            var canonicalDangerousIngredientList = allergenMap
                .OrderBy(entry => entry.Key)
                .Select(entry => entry.Value)
                .Aggregate((ingredient, b) => ingredient + "," + b);

            Console.WriteLine(canonicalDangerousIngredientList);

            return -1;
        }

        private Dictionary<string, string> MapAllergensToIngredients(List<(List<string> ingredients, List<string> allergens)> foods)
        {
            var allergenDictionary = new Dictionary<string, string>();

            var allergenCount = foods
                .SelectMany(x => x.allergens)
                .Distinct()
                .Count();

            while (allergenDictionary.Count != allergenCount)
            {
                var unmappedAllergens = foods
                    .SelectMany(food => food.allergens.Where(allergen => !allergenDictionary.ContainsKey(allergen)))
                    .Distinct()
                    .ToList();

                foreach (var allergen in unmappedAllergens)
                {
                    var foodsWithGivenAllergen = foods.Where(food => food.allergens.Contains(allergen));
                    var unmappedIngredientsList = foodsWithGivenAllergen
                        .Select(food => food.ingredients.Where(ingredient => !allergenDictionary.ContainsValue(ingredient)));
                    var possibleIngredients = unmappedIngredientsList.Aggregate((ing1, ing2) => ing1.Intersect(ing2)).ToList();
                    if (possibleIngredients.Count == 1)
                        allergenDictionary[allergen] = possibleIngredients.First();
                }
            }

            return allergenDictionary;
        }
    }
}