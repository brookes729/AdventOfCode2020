using System;
using System.IO;
using System.Collections.Generic;

namespace AoC2020
{
    public static class DayThree
    {
        public static void Run()
        {
            var File = new StreamReader(Path.GetFullPath("Resource/DayThree.txt"));

            var CurrentLine = string.Empty;
            double PartTwoCount = 0;
            var RowLength = File.ReadLine().Length; // Don't care about the first row and this reduces calculations in the while loop
            var RowCount = 1;
            var Routes = new double[5];

            while ((CurrentLine = File.ReadLine()) != null)
            {
                if (CurrentLine[((RowCount * 1) % RowLength)] == '#')
                {
                    Routes[0]++;
                }
                if (CurrentLine[((RowCount * 3) % RowLength)] == '#')
                {
                    Routes[1]++;
                }
                if (CurrentLine[((RowCount * 5) % RowLength)] == '#')
                {
                    Routes[2]++;
                }
                if (CurrentLine[((RowCount * 7) % RowLength)] == '#')
                {
                    Routes[3]++;
                }
                if (RowCount % 2 == 0 && CurrentLine[(((RowCount / 2)) % RowLength)] == '#')
                {
                    Routes[4]++;
                }

                RowCount++;
            }
            File.Close();

            PartTwoCount = Routes[0] * Routes[1] * Routes[2] * Routes[3] * Routes[4];

            Console.WriteLine($"Part One Count: {Routes[1]}");
            Console.WriteLine($"Part Two Count: {PartTwoCount}");
        }
    }
}