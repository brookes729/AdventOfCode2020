using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AoC2020
{
    public static class DayFifteen
    {
        public static void Run()
        {
            var CurrentLine = "2,20,0,4,1,17";
            var PartOneCount = 0L;
            var PartTwoCount = 0L;

            var PreviousNumbers = CurrentLine.Split(",")
                                             .SkipLast(1)
                                             .Select((x, i) =>
                                                {
                                                    var NumericValue = int.Parse(x);
                                                    var Turn = i + 1;
                                                    return new { NumericValue, Turn };
                                                })
                                             .ToDictionary(x => x.NumericValue, x => x.Turn);

            var LastNumber = int.Parse(CurrentLine.Split(",").Last());
            var NextNumber = 0;
            var Turn = PreviousNumbers.Count() + 1;

            while (Turn < 30000000)
            {
                NextNumber = PreviousNumbers.ContainsKey(LastNumber) ? Turn - PreviousNumbers[LastNumber] : 0;
                PreviousNumbers[LastNumber] = Turn;
                LastNumber = NextNumber;
                Turn++;

                if (Turn == 2020)
                {
                    PartOneCount = LastNumber;
                }
            }

            PartTwoCount = LastNumber;

            Console.WriteLine($"Part One Count: {PartOneCount}");
            Console.WriteLine($"Part Two Count: {PartTwoCount}");
        }

    }
}