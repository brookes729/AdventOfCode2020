using System;
using System.IO;

namespace AoC2020
{
    public static class DayTwelve
    {
        public static void Run()
        {
            var File = new StreamReader(Path.GetFullPath("Resource/DayTwelve.txt"));

            var CurrentLine = string.Empty;
            var PartOneCount = 0;
            var PartTwoCount = 0;

            var NorthAmount = 0;
            var EastAmount = 0;

            var CurrentDirection = 'E';

            // Part one
            while ((CurrentLine = File.ReadLine()) != null)
            {
                var Direction = CurrentLine[0];
                var Value = int.Parse(CurrentLine.Substring(1));

                if (Direction == 'R')
                {
                    CurrentDirection = FindDirection(CurrentDirection, Value);
                    continue;
                }
                if (Direction == 'L')
                {
                    CurrentDirection = FindDirection(CurrentDirection, 360 - Value);
                    continue;
                }

                if (Direction == 'F')
                {
                    Direction = CurrentDirection;
                }

                switch (Direction)
                {
                    case 'N':
                        NorthAmount += Value;
                        break;
                    case 'S':
                        NorthAmount -= Value;
                        break;
                    case 'E':
                        EastAmount += Value;
                        break;
                    case 'W':
                        EastAmount -= Value;
                        break;
                }
            }
            File.Close();

            PartOneCount = Math.Abs(NorthAmount) + Math.Abs(EastAmount);


            File = new StreamReader(Path.GetFullPath("Resource/DayTwelve.txt"));
            NorthAmount = 0;
            EastAmount = 0;
            var WaypointNorthAmount = 1;
            var WaypointEastAmount = 10;

            // Part two
            while ((CurrentLine = File.ReadLine()) != null)
            {
                var Direction = CurrentLine[0];
                var Value = int.Parse(CurrentLine.Substring(1));

                if (Direction == 'R')
                {
                    RotateWayPoint(ref WaypointNorthAmount, ref WaypointEastAmount, Value);
                    continue;
                }
                if (Direction == 'L')
                {
                    RotateWayPoint(ref WaypointNorthAmount, ref WaypointEastAmount, 360 - Value);
                    continue;
                }

                if (Direction == 'F')
                {
                    NorthAmount += WaypointNorthAmount * Value;
                    EastAmount += WaypointEastAmount * Value;
                }

                switch (Direction)
                {
                    case 'N':
                        WaypointNorthAmount += Value;
                        break;
                    case 'S':
                        WaypointNorthAmount -= Value;
                        break;
                    case 'E':
                        WaypointEastAmount += Value;
                        break;
                    case 'W':
                        WaypointEastAmount -= Value;
                        break;
                }
            }
            File.Close();

            PartTwoCount = Math.Abs(NorthAmount) + Math.Abs(EastAmount);

            Console.WriteLine($"Part One Count: {PartOneCount}");
            Console.WriteLine($"Part Two Count: {PartTwoCount}");
        }

        private static void RotateWayPoint(ref int waypointNorthAmount, ref int waypointEastAmount, int value)
        {
            var StartNorth = waypointNorthAmount;
            var StartEast = waypointEastAmount;
            switch (value)
            {
                case 90:
                    waypointNorthAmount = -StartEast;
                    waypointEastAmount = StartNorth;
                    break;
                case 180:
                    waypointNorthAmount = -StartNorth;
                    waypointEastAmount = -StartEast;
                    break;
                case 270:
                    waypointNorthAmount = StartEast;
                    waypointEastAmount = -StartNorth;
                    break;
            }
        }

        private static char FindDirection(char direction, int value)
        {
            var CurrentDirection = direction;
            switch (value)
            {
                case 90:
                    switch (direction)
                    {
                        case 'N':
                            CurrentDirection = 'E';
                            break;
                        case 'S':
                            CurrentDirection = 'W';
                            break;
                        case 'E':
                            CurrentDirection = 'S';
                            break;
                        case 'W':
                            CurrentDirection = 'N';
                            break;
                    }
                    break;
                case 180:
                    switch (direction)
                    {
                        case 'N':
                            CurrentDirection = 'S';
                            break;
                        case 'S':
                            CurrentDirection = 'N';
                            break;
                        case 'E':
                            CurrentDirection = 'W';
                            break;
                        case 'W':
                            CurrentDirection = 'E';
                            break;
                    }
                    break;
                case 270:
                    switch (direction)
                    {
                        case 'N':
                            CurrentDirection = 'W';
                            break;
                        case 'S':
                            CurrentDirection = 'E';
                            break;
                        case 'E':
                            CurrentDirection = 'N';
                            break;
                        case 'W':
                            CurrentDirection = 'S';
                            break;
                    }
                    break;
            }
            return CurrentDirection;
        }
    }
}