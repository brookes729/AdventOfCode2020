using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2020
{
    public static class DayEighteen
    {
        public static void Run()
        {
            var File = new StreamReader(Path.GetFullPath("Resource/DayEighteen.txt"));

            var CurrentLine = string.Empty;
            var PartOneCount = 0L;
            var PartTwoCount = 0L;


            while ((CurrentLine = File.ReadLine()) != null)
            {
                var LineValue = CalculateValue(CurrentLine, false);
                PartOneCount += LineValue;

                LineValue = CalculateValue(CurrentLine, true);
                PartTwoCount += LineValue;
            }
            File.Close();


            Console.WriteLine($"Part One Count: {PartOneCount}");
            Console.WriteLine($"Part Two Count: {PartTwoCount}");
        }

        private static long CalculateValue(string CurrentLine, bool plusFirst)
        {
            long? LastNumber = null;
            var LastSymbol = "";

            while (CurrentLine.IndexOf("(") > -1)
            {
                CurrentLine = Regex.Replace(CurrentLine,
                                            "(\\({1}[^\\(]+?\\))",
                                            match => CalculateValue(match.Value.Substring(1, match.Value.Length - 2), plusFirst).ToString());
            }

            if (plusFirst && !Regex.Match(CurrentLine, "^\\d+ \\+ \\d+$").Success)
            {
                while (CurrentLine.IndexOf("+") > -1)
                {
                    CurrentLine = Regex.Replace(CurrentLine,
                                                "((\\d+|\\({1}[^\\(]+?\\)) \\+ (\\d+|\\({1}[^\\(]+?\\)))",
                                                match => CalculateValue(match.Value, plusFirst).ToString());
                }
            }

            var SplitLine = CurrentLine.Split(' ');

            foreach (var NextInput in SplitLine)
            {
                if (NextInput == "+")
                {
                    LastSymbol = NextInput;
                }
                else if (NextInput == "*")
                {
                    LastSymbol = NextInput;
                }
                else if (long.TryParse(NextInput, out long NextValue))
                {
                    if (!LastNumber.HasValue)
                    {
                        LastNumber = NextValue;
                    }
                    else if (!string.IsNullOrEmpty(LastSymbol))
                    {
                        if (LastSymbol == "*")
                        {
                            LastNumber *= NextValue;
                        }
                        else if (LastSymbol == "+")
                        {
                            LastNumber += NextValue;
                        }
                        LastSymbol = null;
                    }
                    else
                    {
                        throw new Exception("Got a second number but no symbol");
                    }
                }
            }

            return LastNumber.Value;
        }
    }
}