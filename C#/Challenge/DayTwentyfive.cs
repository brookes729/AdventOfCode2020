using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2020
{
    public static class DayTwentyfive
    {
        public static void Run()
        {
            var CurrentLine = string.Empty;
            var PartOneCount = 0L;
            var PartTwoCount = 0L;

            var CardPublicKey = 16915772;
            var DoorPublicKey = 18447943;

            var SubjectNumber = 7L;
            
            var CurrentValue = SubjectNumber;
            var CardLoopSize = 0;
            var DoorLoopSize = 0;
            var Counter = 1;
            
            while (CardLoopSize == 0 || DoorLoopSize == 0)
            {
                Counter++;
                CurrentValue = (CurrentValue * SubjectNumber) % 20201227;

                if (CurrentValue == CardPublicKey) CardLoopSize = Counter;
                if (CurrentValue == DoorPublicKey) DoorLoopSize = Counter;
            }

            SubjectNumber = CurrentValue;
            for (int i = 1; i < Math.Min(CardLoopSize, DoorLoopSize); i++)
            {
                CurrentValue = (CurrentValue * SubjectNumber) % 20201227;                
            }
            PartOneCount = CurrentValue;

            Console.WriteLine($"Part One Count: {PartOneCount}");
            Console.WriteLine($"Part Two Count: Unknown");
        }
    }
}