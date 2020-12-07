using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2020
{
    public static class DaySeven
    {
        public static void Run()
        {
            var File = new StreamReader(Path.GetFullPath("Resource/DaySeven.txt"));

            var CurrentLine = string.Empty;
            var PartOneCount = 0;
            var PartTwoCount = 0;

            var BagRules = new Dictionary<string, string>();
            var BagRulesDetailed = new Dictionary<string, Dictionary<string, int>>();

            while ((CurrentLine = File.ReadLine()) != null)
            {
                if (CurrentLine.IndexOf("no other bags") == -1)
                {
                    var SplitBagFromContents = CurrentLine.Split(" bags contain ");
                    BagRules[SplitBagFromContents[0]] = SplitBagFromContents[1];


                    var SplitContents = SplitBagFromContents[1].Split(", ");

                    BagRulesDetailed[SplitBagFromContents[0]] = new Dictionary<string, int>();

                    foreach (var BagGroup in SplitContents)
                    {
                        var NumberAndColour = BagGroup.Split(" bag")[0];
                        var Number = Int32.Parse(Char.ToString(NumberAndColour[0]));
                        var Colour = NumberAndColour.Substring(2);

                        BagRulesDetailed[SplitBagFromContents[0]][Colour] = Number;
                    }
                }
            }
            File.Close();

            var PotentialBags = new List<string>() { "shiny gold" };
            var FoundNewBag = true;

            while (FoundNewBag)
            {
                FoundNewBag = false;
                foreach (var Bag in BagRules)
                {
                    if (!PotentialBags.Contains(Bag.Key))
                    {
                        foreach (var FoundBag in PotentialBags)
                        {
                            if (Bag.Value.Contains(FoundBag))
                            {
                                PotentialBags.Add(Bag.Key);
                                FoundNewBag = true;
                                break;
                            }
                        }
                    }
                }
            }

            PartOneCount = PotentialBags.Count() - 1;
            PartTwoCount = CountBags("shiny gold", BagRulesDetailed) - 1; // Don't include itself

            Console.WriteLine($"Part One Count: {PartOneCount}");
            Console.WriteLine($"Part Two Count: {PartTwoCount}");
        }

        private static int CountBags(string parentBag, Dictionary<string, Dictionary<string, int>> bagRulesDetailed)
        {
            if (!bagRulesDetailed.ContainsKey(parentBag))
            {
                return 1;
            }

            var InternalCount = 1; // itself
            foreach (var ChildBagColourCount in bagRulesDetailed[parentBag])
            {
                InternalCount += ChildBagColourCount.Value * CountBags(ChildBagColourCount.Key, bagRulesDetailed);
            }

            return InternalCount;
        }
    }
}