using System;

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
                default:
                    Console.WriteLine("Sorry don't know that one");
                    break;

            }

            Console.WriteLine();
            Console.WriteLine("Have an awesome day");
        }
    }
}
