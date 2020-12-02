using System;
using System.IO;
using System.Linq;

namespace AoC2020
{
    public static class DayTwo
    {
        public static void Run()
        {
            var CurrentLine = string.Empty;
            var GoodPasswordsByCount = 0;
            var GoodPasswordsByPosition = 0;
            var File = new StreamReader(Path.GetFullPath("Resource/DayTwo.txt"));

            //1-3 a: abcde
            while ((CurrentLine = File.ReadLine()) != null)
            {
                CheckLine(CurrentLine, ref GoodPasswordsByCount, ref GoodPasswordsByPosition);
            }
            File.Close();

            Console.WriteLine($"Good Passwords by count: {GoodPasswordsByCount}");
            Console.WriteLine($"Good Passwords by position: {GoodPasswordsByPosition}");
        }

        private static void CheckLine(string lineToCheck, ref int goodPasswordsByCount, ref int goodPasswordsByPosition)
        {
            var LineParts = lineToCheck.Split(" ");
            var MinMax = LineParts[0].Split("-");
            var MinNumber = Convert.ToInt16(MinMax[0]);
            var MaxNumber = Convert.ToInt16(MinMax[1]);
            var Character = LineParts[1][0];

            var CharCount = LineParts[2].Count(x => x == Character);

            if (MinNumber <= CharCount && MaxNumber >= CharCount)
            {
                goodPasswordsByCount++;
            }

            if (LineParts[2][MinNumber - 1] == Character
                ^ LineParts[2][MaxNumber - 1] == Character)
            {
                goodPasswordsByPosition++;
            }
        }
    }
}