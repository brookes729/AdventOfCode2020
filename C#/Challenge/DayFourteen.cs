using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AoC2020
{
    public static class DayFourteen
    {
        public static void Run()
        {
            var File = new StreamReader(Path.GetFullPath("Resource/DayFourteen.txt"));

            var CurrentLine = string.Empty;
            var PartOneCount = 0L;
            var PartTwoCount = 0L;

            var Mask = string.Empty;
            var Memory = new Dictionary<int, long>();
            var MemoryTwo = new Dictionary<long, long>();

            while ((CurrentLine = File.ReadLine()) != null)
            {
                var SplitLine = CurrentLine.Split(" = ");
                if (SplitLine[0] == "mask")
                {
                    Mask = SplitLine[1];
                }
                else
                {
                    var Location = int.Parse(SplitLine[0].Substring(4, SplitLine[0].Length - 5));
                    Memory[Location] = ApplyBitmask(long.Parse(SplitLine[1]), Mask);

                    var MemoryLocations = GetMemoryLocations(Location, Mask);

                    foreach (var MemoryLocation in MemoryLocations)
                    {
                        MemoryTwo[Convert.ToInt64(MemoryLocation, 2)] = long.Parse(SplitLine[1]);
                    }
                }
            }
            File.Close();

            PartOneCount = Memory.Values.Aggregate((a, b) => a + b);
            PartTwoCount = MemoryTwo.Values.Aggregate((a, b) => a + b);

            Console.WriteLine($"Part One Count: {PartOneCount}");
            Console.WriteLine($"Part Two Count: {PartTwoCount}");
        }

        private static long ApplyBitmask(long value, string mask)
        {
            var ValueBinaryString = Convert.ToString(value, 2).PadLeft(36, '0');

            var CombinedString = ValueBinaryString.Zip(mask, (v, m) => m == 'X' ? v : m).Aggregate("", (a, b) => a + b);

            return Convert.ToInt64(CombinedString, 2);
        }

        private static string[] GetMemoryLocations(int location, string mask)
        {
            var LocationBinaryString = Convert.ToString(location, 2).PadLeft(36, '0');

            var CombinedString = LocationBinaryString.Zip(mask, (v, m) => m == '0' ? v : m).Aggregate("", (a, b) => a + b);

            return GetAllCombinedValuesFromArray(CombinedString.Split('X'));
        }

        private static string[] GetAllCombinedValuesFromArray(string[] splitArray)
        {
            if (splitArray.Length == 1)
            {
                return splitArray;
            }

            var Subset = GetAllCombinedValuesFromArray(splitArray.Skip(1).ToArray());

            var Output = Subset.Select(x => splitArray[0] + "0" + x).Union(Subset.Select(x => splitArray[0] + "1" + x));

            return Output.ToArray();
        }
    }
}