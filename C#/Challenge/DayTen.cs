using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2020
{
    public static class DayTen
    {
        private static Dictionary<int, int> TribonacciNumbers = new Dictionary<int, int>() { { -2, 0 }, { -1, 0 }, { 0, 1 } };
        public static void Run()
        {
            var File = new StreamReader(Path.GetFullPath("Resource/DayTen.txt"));

            var CurrentLine = string.Empty;
            var PartOneCount = 0;
            var PartTwoCount = 0d;

            var Joltages = new List<int>() { 0 };

            while ((CurrentLine = File.ReadLine()) != null)
            {
                Joltages.Add(int.Parse(CurrentLine));
            }
            File.Close();

            var JoltageJumps = Joltages.OrderBy(x => x)
                                             .Zip(Joltages.OrderBy(x => x).Skip(1), (x, y) => y - x).ToList();

            var JoltageJumpGrouped = JoltageJumps.GroupBy(x => x)
                                             .ToDictionary(x => x.Key, x => x.Count());

            PartOneCount = (JoltageJumpGrouped[1]) * (JoltageJumpGrouped[3] + 1);

            PartTwoCount = 1;
            var CurrentCountOfOnes = 0;
            for (int i = 0; i < JoltageJumps.Count(); i++)
            {
                if (JoltageJumps[i] == 1)
                {
                    CurrentCountOfOnes++;
                }
                else if (CurrentCountOfOnes > 0)
                {
                    PartTwoCount *= GetNthTribonacciNumber(CurrentCountOfOnes);
                    CurrentCountOfOnes = 0;
                }
            }
            if (CurrentCountOfOnes > 0)
            {
                PartTwoCount *= GetNthTribonacciNumber(CurrentCountOfOnes);
                CurrentCountOfOnes = 0;
            }

            Console.WriteLine($"Part One Count: {PartOneCount}");
            Console.WriteLine($"Part Two Count: {PartTwoCount}");
        }

        private static int GetNthTribonacciNumber(int n)
        {
            if (TribonacciNumbers.ContainsKey(n))
            {
                return TribonacciNumbers[n];
            }

            var Value = GetNthTribonacciNumber(n - 1) + GetNthTribonacciNumber(n - 2) + GetNthTribonacciNumber(n - 3);

            TribonacciNumbers[n] = Value;

            return Value;
        }
    }
}