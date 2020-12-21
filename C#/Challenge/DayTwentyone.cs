using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2020
{
    public static class DayTwentyone
    {
        public static void Run()
        {
            var File = new StreamReader(Path.GetFullPath("Resource/DayTwentyone.txt"));

            var CurrentLine = string.Empty;
            var PartOneCount = 0L;

            var AllergenPotentialIngredients = new Dictionary<string, List<string>>();
            var AllIngredientsCount = new Dictionary<string, int>();

            while ((CurrentLine = File.ReadLine()) != null)
            {
                var IngredientAllergenSplit = CurrentLine.TrimEnd(')').Split(" (contains ");
                var Ingredients = IngredientAllergenSplit[0].Split(" ").ToList();

                foreach (var Ingredient in Ingredients)
                {
                    AllIngredientsCount[Ingredient] = AllIngredientsCount.ContainsKey(Ingredient) ?
                                                        ++AllIngredientsCount[Ingredient] : 1;
                }

                if (IngredientAllergenSplit.Count() > 1)
                {
                    var Allergens = IngredientAllergenSplit[1].Split(", ");
                    foreach (var Allergen in Allergens)
                    {
                        if (!AllergenPotentialIngredients.ContainsKey(Allergen))
                        {
                            AllergenPotentialIngredients.Add(Allergen, Ingredients);
                        }
                        else
                        {
                            AllergenPotentialIngredients[Allergen] = AllergenPotentialIngredients[Allergen].Intersect(Ingredients).ToList();
                        }
                    }
                }
            }
            File.Close();

            var NoAllergenIngredientsCount = AllIngredientsCount.Where(x =>
                                                    !AllergenPotentialIngredients.Values.Any(y =>
                                                                                            y.Any(z => z == x.Key)))
                                                    .ToDictionary(x => x.Key, x => x.Value);

            PartOneCount = NoAllergenIngredientsCount.Sum(x => x.Value);

            var AllergenIngredient = CrossMatch(AllergenPotentialIngredients);

            var PartTwo = AllergenIngredient.OrderBy(x => x.Key)
                                            .Select(x => x.Value.FirstOrDefault())
                                            .Aggregate((x, y) => x + "," + y);

            Console.WriteLine($"Part One Count: {PartOneCount}");
            Console.WriteLine($"Part Two: {PartTwo}");
        }


        private static Dictionary<string, List<string>> CrossMatch(Dictionary<string, List<string>> potentialIngredients)
        {
            var SinglePotential = potentialIngredients.Where(x => x.Value.Count() == 1).ToList();

            if (SinglePotential.Count() == potentialIngredients.Count())
            {
                return potentialIngredients;
            }
            else if (SinglePotential.Count() == 0)
            {
                throw new Exception("well thats not good");
            }

            foreach (var DeterminedPotential in SinglePotential)
            {
                potentialIngredients = potentialIngredients.Except(SinglePotential)
                                                               .Select(x =>
                                                               {
                                                                   var NewList = x.Value.Where(y =>
                                                                                       y != DeterminedPotential.Value.First())
                                                                                    .ToList();
                                                                   return new { x.Key, NewList };
                                                               })
                                                               .ToDictionary(x => x.Key, x => x.NewList);
            }

            return CrossMatch(potentialIngredients.Union(SinglePotential).ToDictionary(x => x.Key, x => x.Value));
        }
    }
}