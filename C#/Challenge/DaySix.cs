using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2020
{
    public static class DaySix
    {
        public static void Run()
        {
            var File = new StreamReader(Path.GetFullPath("Resource/DaySix.txt"));

            var CurrentLine = string.Empty;
            var PartOneCount = 0;
            var PartTwoCount = 0;

            while ((CurrentLine = File.ReadLine()) != null)
            {
                var RulesSingle = new List<char>();
                var RulesAll = new List<char>();

                RulesAll.AddRange(CurrentLine);

                while (CurrentLine != null && !string.IsNullOrEmpty(CurrentLine))
                {
                    RulesSingle = RulesSingle.Union(CurrentLine.ToList()).ToList();
                    RulesAll = RulesAll.Intersect(CurrentLine.ToList()).ToList();

                    CurrentLine = File.ReadLine();
                }
                PartOneCount += RulesSingle.Count();
                PartTwoCount += RulesAll.Count();
            }
            File.Close();

            Console.WriteLine($"Part One Count: {PartOneCount}");
            Console.WriteLine($"Part Two Count: {PartTwoCount}");
        }
    }
}