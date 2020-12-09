using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2020
{
    public static class DayNine
    {
        public static void Run()
        {
            var File = new StreamReader(Path.GetFullPath("Resource/DayNine.txt"));

            var CurrentLine = string.Empty;
            var PartOneCount = 0;
            var PartTwoCount = 0;

            var Numbers = new List<int>();
            var PreambleSize = 25;

            for (int i = 0; i < PreambleSize; i++)
            {
                Numbers.Add(int.Parse(File.ReadLine()));
            }

            while ((CurrentLine = File.ReadLine()) != null)
            {
                var NextValue = int.Parse(CurrentLine);
                var PreviousValues = Numbers.TakeLast(PreambleSize).ToList();

                var PairedNumbers = PreviousValues.Select(x => NextValue - x) // find the paired number it would need
                                                  .Where(x => x != NextValue) // Make sure it's not a double
                                                  .Intersect(PreviousValues); // check in the previous values if it is there

                if (!PairedNumbers.Any())
                {
                    PartOneCount = NextValue;

                    for (int i = 0; i < Numbers.Count(); i++)
                    {
                        var PotentialNumbers = new List<int>() { Numbers[i] };
                        for (int j = i + 1; j < Numbers.Count(); j++)
                        {
                            PotentialNumbers.Add(Numbers[j]);
                            if (PotentialNumbers.Sum() == NextValue)
                            {
                                PartTwoCount = PotentialNumbers.Min() + PotentialNumbers.Max();
                            }
                            else if (PotentialNumbers.Sum() > NextValue)
                            {
                                break;
                            }
                        }
                    }

                    break;
                }
                Numbers.Add(NextValue);
            }
            File.Close();

            Console.WriteLine($"Part One Count: {PartOneCount}");
            Console.WriteLine($"Part Two Count: {PartTwoCount}");
        }
    }
}