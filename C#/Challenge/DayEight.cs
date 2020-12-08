using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2020
{
    public static class DayEight
    {
        public static void Run()
        {
            var File = new StreamReader(Path.GetFullPath("Resource/DayEight.txt"));

            var CurrentLine = string.Empty;
            var PartOneCount = 0;
            var PartTwoCount = 0;

            var Instructions = new Dictionary<int, (string Code, int Amount)>();
            var LineNumber = 1;

            while ((CurrentLine = File.ReadLine()) != null)
            {
                var CodeAndAmountSplit = CurrentLine.Split(" ");
                Instructions.Add(LineNumber, (CodeAndAmountSplit[0], int.Parse(CodeAndAmountSplit[1])));
                LineNumber++;
            }
            File.Close();

            PartOneCount = RunCode(1, Instructions, new List<int>());

            var AttemptSwapping = RunCodeTryChange(1, Instructions, new List<int>(), false);
            PartTwoCount = AttemptSwapping.Accumulator;

            Console.WriteLine($"Part One Count: {PartOneCount}");
            Console.WriteLine($"Part Two Count: {PartTwoCount}");
        }

        private static int RunCode(int startingLine, Dictionary<int, (string Code, int Amount)> instructions, List<int> visitedLines)
        {
            if (visitedLines.Contains(startingLine))
            {
                return 0;
            }
            visitedLines.Add(startingLine);

            var Accumulator = 0;
            var NextLine = startingLine;
            switch (instructions[startingLine].Code)
            {
                case "acc":
                    Accumulator += instructions[startingLine].Amount;
                    NextLine++;
                    break;
                case "jmp":
                    NextLine += instructions[startingLine].Amount;
                    break;
                case "nop":
                    NextLine++;
                    break;
            }

            Accumulator += RunCode(NextLine, instructions, visitedLines);

            return Accumulator;
        }

        private static (int Accumulator, bool Looped) RunCodeTryChange(int startingLine,
                                                                        Dictionary<int, (string Code, int Amount)> instructions,
                                                                        List<int> visitedLines,
                                                                        bool alreadySwapped)
        {
            if (visitedLines.Contains(startingLine))
            {
                return (0, true);
            }
            else if (startingLine == instructions.Keys.Max() + 1)
            {
                return (0, false);
            }
            visitedLines.Add(startingLine);

            var Accumulator = 0;
            var NextLine = startingLine;
            var Swap = true;
            var NextLineSwapped = startingLine;
            switch (instructions[startingLine].Code)
            {
                case "acc":
                    Accumulator += instructions[startingLine].Amount;
                    NextLine++;
                    Swap = false;
                    break;
                case "jmp":
                    NextLine += instructions[startingLine].Amount;
                    NextLineSwapped++;
                    break;
                case "nop":
                    NextLine++;
                    NextLineSwapped += instructions[startingLine].Amount;
                    break;
            }

            if (!alreadySwapped && Swap)
            {
                var Swapped = RunCodeTryChange(NextLineSwapped, instructions, new List<int>(visitedLines), true);
                if (!Swapped.Looped)
                {
                    return Swapped;
                }
            }

            var NotSwappedThisLine = RunCodeTryChange(NextLine, instructions, visitedLines, alreadySwapped);
            NotSwappedThisLine.Accumulator += Accumulator;
            return NotSwappedThisLine;
        }
    }
}