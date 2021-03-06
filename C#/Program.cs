﻿using System;

namespace AoC2020
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Which day?");

            switch (Console.ReadLine())
            {
                case "1":
                    DayOne.Run();
                    break;
                case "2":
                    DayTwo.Run();
                    break;
                case "3":
                    DayThree.Run();
                    break;
                case "4":
                    DayFour.Run();
                    break;
                case "5":
                    DayFive.Run();
                    break;
                case "6":
                    DaySix.Run();
                    break;
                case "7":
                    DaySeven.Run();
                    break;
                case "8":
                    DayEight.Run();
                    break;
                case "9":
                    DayNine.Run();
                    break;
                case "10":
                    DayTen.Run();
                    break;
                case "11":
                    DayEleven.Run();
                    break;
                case "12":
                    DayTwelve.Run();
                    break;
                case "13":
                    DayThirteen.Run();
                    break;
                case "14":
                    DayFourteen.Run();
                    break;
                case "15":
                    DayFifteen.Run();
                    break;
                case "16":
                    DaySixteen.Run();
                    break;
                case "17":
                    DaySeventeen.RunPart1();
                    DaySeventeen.RunPart2();
                    break;
                case "18":
                    DayEighteen.Run();
                    break;
                case "19":
                    DayNineteen.Run();
                    break;
                case "20":
                    DayTwenty.Run();
                    break;
                case "21":
                    DayTwentyone.Run();
                    break;
                case "22":
                    DayTwentytwo.Run();
                    break;
                case "23":
                    DayTwentythree.Run();
                    break;
                case "24":
                    DayTwentyfour.Run();
                    break;
                case "25":
                    DayTwentyfive.Run();
                    break;
                default:
                    Console.WriteLine("Sorry don't know that one");
                    break;

            }

            Console.WriteLine();
            Console.WriteLine("Have an awesome day");
        }
    }
}
