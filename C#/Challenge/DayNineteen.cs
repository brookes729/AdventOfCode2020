using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2020
{
    public static class DayNineteen
    {

        public static void Run()
        {
            var File = new StreamReader(Path.GetFullPath("Resource/DayNineteen.txt"));

            var CurrentLine = string.Empty;
            var PartOneCount = 0;
            var PartTwoCount = 0;

            var Rules = new Dictionary<string, string>();

            while ((CurrentLine = File.ReadLine()) != null && !string.IsNullOrEmpty(CurrentLine))
            {
                var SplitLine = CurrentLine.Split(": ");
                Rules[SplitLine[0]] = SplitLine[1];
            }

            // Cloning and referencing can be a pain
            var RulesForPartTwo = Rules.Select(x => x).ToDictionary(x => x.Key, x => x.Value);
            RulesForPartTwo["8"] = "42 | 42 8";
            RulesForPartTwo["11"] = "42 31 | 42 11 31";


            while ((CurrentLine = File.ReadLine()) != null)
            {
                var PassesRule0 = CheckRule(CurrentLine, Rules["0"], Rules, out var RemainPartOne);
                PartOneCount += PassesRule0 && RemainPartOne.Count(x => x == "") == 1 ? 1 : 0;

                PassesRule0 = CheckRule(CurrentLine, Rules["0"], RulesForPartTwo, out var RemainPartTwo);
                PartTwoCount += PassesRule0 && RemainPartTwo.Count(x => x == "") == 1 ? 1 : 0;
            }
            File.Close();

            Console.WriteLine($"Part One Count: {PartOneCount}");
            Console.WriteLine($"Part Two Count: {PartTwoCount}");
        }

        private static bool CheckRule(string currentLine, string currentRule, Dictionary<string, string> rules, out List<string> remaining)
        {
            remaining = new List<string>();
            if (currentRule.Contains("|"))
            {
                var SplitOrRule = currentRule.Split(" | ");
                var MatchOne = CheckRule(currentLine, SplitOrRule[0], rules, out var RemainingSplitOne);
                var MatchTwo = CheckRule(currentLine, SplitOrRule[1], rules, out var RemainingSplitTwo);
                if (MatchOne)
                {
                    remaining = remaining.Union(RemainingSplitOne).ToList();
                }
                if (MatchTwo)
                {
                    remaining = remaining.Union(RemainingSplitTwo).ToList();
                }

                return MatchOne || MatchTwo;
            }

            if (currentRule.StartsWith("\""))
            {
                var CurrentRuleChar = currentRule.Substring(1, currentRule.Length - 2);
                if (currentLine.StartsWith(CurrentRuleChar))
                {
                    remaining.Add(currentLine.Substring(1));

                    return true;
                }
                else
                {
                    return false;
                }
            }

            var SplitRules = currentRule.Split(" ");

            var Match = CheckRule(currentLine, rules[SplitRules[0]], rules, out var NewRemaining);
            if (Match)
            {
                if (SplitRules.Count() > 1)
                {
                    var ChildMatchOverall = false;
                    foreach (var PotentialRemaining in NewRemaining)
                    {
                        var ChildMatch = CheckRule(PotentialRemaining, currentRule.Substring(SplitRules[0].Length + 1), rules, out var ChildRemaining);
                        if (ChildMatch)
                        {
                            remaining = remaining.Union(ChildRemaining).ToList();
                            ChildMatchOverall |= ChildMatch;
                        }
                    }
                    return ChildMatchOverall;
                }
                else
                {
                    remaining = NewRemaining;
                    return true;
                }
            }

            return false;
        }
    }
}