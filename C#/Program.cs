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
                default:
                    Console.WriteLine("Sorry don't know that one");
                    break;

            }

            Console.WriteLine();
            Console.WriteLine("Have an awesome day");
        }
    }
}
