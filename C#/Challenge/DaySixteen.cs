using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AoC2020
{
    public static class DaySixteen
    {
        public static void Run()
        {
            var File = new StreamReader(Path.GetFullPath("Resource/DaySixteen.txt"));

            var CurrentLine = string.Empty;
            var PartOneCount = 0;
            var PartTwoCount = 0L;
            var Rules = new Dictionary<string, List<(int Min, int Max)>>();

            while ((CurrentLine = File.ReadLine()) != null && CurrentLine != string.Empty)
            {
                var SplitLine = CurrentLine.Split(": ");
                var LineRules = SplitLine[1].Split(" or ");
                var FirstRegion = LineRules[0].Split("-").Select(x => int.Parse(x)).ToList();
                var SecondRegion = LineRules[1].Split("-").Select(x => int.Parse(x)).ToList();


                Rules[SplitLine[0]] = new List<(int Min, int Max)>() {
                    (FirstRegion[0], FirstRegion[1]),
                    (SecondRegion[0], SecondRegion[1])
                };
            }

            CurrentLine = File.ReadLine(); // your ticket:
            var MyTicket = (CurrentLine = File.ReadLine()).Split(",").Select(x => int.Parse(x)).ToList();

            CurrentLine = File.ReadLine(); // blank line
            CurrentLine = File.ReadLine(); // nearby tickets:


            var PotentialRulePlacement = Enumerable.Repeat(Rules.Keys.Select(x => x).ToList(), MyTicket.Count())
                                                   .Select((List, Index) => new { List, Index })
                                                   .ToDictionary(x => x.Index, x => x.List);
            var FieldNumber = 0;

            var InvalidRulePlacement = new Dictionary<int, List<string>>();

            foreach (var Value in MyTicket)
            {
                var Valid = false;
                foreach (var Rule in Rules)
                {
                    if ((Value >= Rule.Value[0].Min && Value <= Rule.Value[0].Max)
                        || (Value >= Rule.Value[1].Min && Value <= Rule.Value[1].Max))
                    {
                        Valid = true;
                        break;
                    }
                    else
                    {
                        if (!InvalidRulePlacement.ContainsKey(FieldNumber))
                        {
                            InvalidRulePlacement[FieldNumber] = new List<string>();
                        }
                        InvalidRulePlacement[FieldNumber].Add(Rule.Key);
                    }
                }
                PartOneCount += Valid ? 0 : Value;
                FieldNumber++;
            }

            foreach (var InvalidRule in InvalidRulePlacement)
            {
                PotentialRulePlacement[InvalidRule.Key] = PotentialRulePlacement[InvalidRule.Key].Except(InvalidRule.Value).ToList();
            }

            while ((CurrentLine = File.ReadLine()) != null)
            {
                var Ticket = CurrentLine.Split(",").Select(x => int.Parse(x));
                var ValidTicket = true;
                FieldNumber = 0;

                InvalidRulePlacement = new Dictionary<int, List<string>>();

                foreach (var Value in Ticket)
                {
                    var Valid = false;
                    foreach (var Rule in Rules)
                    {
                        if ((Value >= Rule.Value[0].Min && Value <= Rule.Value[0].Max)
                            || (Value >= Rule.Value[1].Min && Value <= Rule.Value[1].Max))
                        {
                            Valid = true;
                        }
                        else
                        {
                            if (!InvalidRulePlacement.ContainsKey(FieldNumber))
                            {
                                InvalidRulePlacement[FieldNumber] = new List<string>();
                            }
                            InvalidRulePlacement[FieldNumber].Add(Rule.Key);
                        }
                    }
                    PartOneCount += Valid ? 0 : Value;
                    ValidTicket &= Valid;
                    FieldNumber++;
                }

                if (ValidTicket)
                {
                    foreach (var InvalidRule in InvalidRulePlacement)
                    {
                        PotentialRulePlacement[InvalidRule.Key] = PotentialRulePlacement[InvalidRule.Key].Except(InvalidRule.Value).ToList();
                    }
                }
            }
            File.Close();

            PotentialRulePlacement = CrossMatchRules(PotentialRulePlacement);

            var DepartureRules = PotentialRulePlacement.Where(x => x.Value.Any(y => y.StartsWith("departure")));

            PartTwoCount = 1;
            foreach (var DepartureRule in DepartureRules)
            {
                PartTwoCount *= MyTicket[DepartureRule.Key];
            }

            Console.WriteLine($"Part One Count: {PartOneCount}");
            Console.WriteLine($"Part Two Count: {PartTwoCount}");
        }

        private static Dictionary<int, List<string>> CrossMatchRules(Dictionary<int, List<string>> potentialRulePlacement)
        {
            var SinglePotential = potentialRulePlacement.Where(x => x.Value.Count() == 1).ToList();

            if (SinglePotential.Count() == potentialRulePlacement.Count())
            {
                return potentialRulePlacement;
            }
            else if (SinglePotential.Count() == 0)
            {
                throw new Exception("well thats not good");
            }

            foreach (var DeterminedRule in SinglePotential)
            {
                potentialRulePlacement = potentialRulePlacement.Except(SinglePotential)
                                                               .Select(x =>
                                                               {
                                                                   var NewList = x.Value.Where(y =>
                                                                                       y != DeterminedRule.Value.First())
                                                                                    .ToList();
                                                                   return new { x.Key, NewList };
                                                               })
                                                               .ToDictionary(x => x.Key, x => x.NewList);
            }

            return CrossMatchRules(potentialRulePlacement.Union(SinglePotential).ToDictionary(x => x.Key, x => x.Value));
        }
    }
}