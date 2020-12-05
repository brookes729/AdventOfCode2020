using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2020
{
    public static class DayFive
    {
        public static void Run()
        {
            var File = new StreamReader(Path.GetFullPath("Resource/DayFive.txt"));

            var CurrentLine = string.Empty;
            var PartOneCount = 0;
            var PartTwoCount = 0;

            var TakenSeats = new List<int>();


            while ((CurrentLine = File.ReadLine()) != null)
            {
                var BinaryString = Regex.Replace(Regex.Replace(CurrentLine, "[BR]", "1"), "[FL]", "0");
                var SeatNumber = Convert.ToInt32(BinaryString, 2);

                TakenSeats.Add(SeatNumber);

                PartOneCount = PartOneCount > SeatNumber ? PartOneCount : SeatNumber;
            }
            File.Close();

            var RemainingSeats = Enumerable.Range(0, PartOneCount)
                                           .Except(TakenSeats);

            PartTwoCount = RemainingSeats.Where((seat) => !(RemainingSeats.Contains(seat - 1) || (RemainingSeats.Contains(seat + 1))))
                                            .First();

            Console.WriteLine($"Part One Count: {PartOneCount}");
            Console.WriteLine($"Part Two Count: {PartTwoCount}");
        }
    }
}