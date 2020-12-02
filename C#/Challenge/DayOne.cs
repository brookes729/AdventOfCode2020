using System;
using System.Collections.Generic;
using System.IO;

namespace AoC2020
{
    public static class DayOne
    {
        public static void Run()
        {
            var Line = string.Empty;
            var Numbers = new List<int>();
            var File = new StreamReader(Path.GetFullPath("Resource/DayOne.txt"));

            while ((Line = File.ReadLine()) != null)
            {
                if (int.TryParse(Line, out var Number))
                {
                    // Part One
                    if (Numbers.Contains(2020 - Number))
                    {
                        Console.WriteLine($"The multiple of two numbers that add up to 2020 is: {Number * (2020 - Number)}");
                    }

                    // Part Two
                    foreach (var ExistingNumber in Numbers)
                    {
                        if (Numbers.Contains(2020 - Number - ExistingNumber))
                        {
                            Console.WriteLine($"The multiple of three numbers that add up to 2020 is: {Number * ExistingNumber * (2020 - Number - ExistingNumber)}");
                            break;
                        }
                    }
                    Numbers.Add(Number);
                }
            }
            File.Close();
        }
    }
}