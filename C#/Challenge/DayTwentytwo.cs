using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2020
{
    public static class DayTwentytwo
    {
        public static void Run()
        {
            var File = new StreamReader(Path.GetFullPath("Resource/DayTwentytwo.txt"));

            var CurrentLine = string.Empty;
            var PartOneCount = 0L;
            var PartTwoCount = 0L;

            var DeckOne = new List<int>();
            var DeckTwo = new List<int>();
            var Player = 0;

            while ((CurrentLine = File.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(CurrentLine)) continue;

                if (CurrentLine.StartsWith("Player"))
                {
                    Player++;
                    continue;
                }

                if (Player == 1)
                {
                    DeckOne.Add(int.Parse(CurrentLine));
                }
                else
                {
                    DeckTwo.Add(int.Parse(CurrentLine));
                }

            }
            File.Close();

            var GameOneDeckOne = DeckOne.Select(x => x).ToList();
            var GameOneDeckTwo = DeckTwo.Select(x => x).ToList();

            PlayGame(ref GameOneDeckOne, ref GameOneDeckTwo);

            var Winner = GameOneDeckOne.Union(GameOneDeckTwo);

            PartOneCount = Enumerable.Range(1, Winner.Count())
                                     .Reverse()
                                     .Zip(Winner, (a, b) => a * b)
                                     .Aggregate((a, b) => a + b);

            PlayGame(ref DeckOne, ref DeckTwo, true);

            Winner = DeckOne.Union(DeckTwo);

            PartTwoCount = Enumerable.Range(1, Winner.Count())
                                     .Reverse()
                                     .Zip(Winner, (a, b) => a * b)
                                     .Aggregate((a, b) => a + b);

            Console.WriteLine($"Part One Count: {PartOneCount}");
            Console.WriteLine($"Part Two Count: {PartTwoCount}");
        }

        private static void PlayGame(ref List<int> DeckOne, ref List<int> DeckTwo, bool Recurisive = false)
        {
            var SeenGameList = new List<string>();

            while (DeckOne.Count() != 0 && DeckTwo.Count != 0)
            {
                var GameValue = string.Join(",", DeckOne) + "~" + string.Join(",", DeckTwo);
                if (SeenGameList.Contains(GameValue))
                {
                    DeckTwo = new List<int>();
                    break;
                }
                SeenGameList.Add(GameValue);

                var CardOne = DeckOne[0];
                var CardTwo = DeckTwo[0];
                var PlayerOneWins = true;

                if (Recurisive && CardOne < DeckOne.Count && CardTwo < DeckTwo.Count)
                {
                    var NewDeckOne = DeckOne.Skip(1).Take(CardOne).ToList();
                    var NewDeckTwo = DeckTwo.Skip(1).Take(CardTwo).ToList();
                    PlayGame(ref NewDeckOne, ref NewDeckTwo, true);
                    PlayerOneWins = NewDeckOne.Count() != 0;
                }
                else
                {
                    PlayerOneWins = CardOne > CardTwo;
                }

                if (PlayerOneWins)
                {
                    DeckOne = DeckOne.Skip(1).Union(new List<int>() { CardOne, CardTwo }).ToList();
                    DeckTwo = DeckTwo.Skip(1).ToList();
                }
                else
                {
                    DeckOne = DeckOne.Skip(1).ToList();
                    DeckTwo = DeckTwo.Skip(1).Union(new List<int>() { CardTwo, CardOne }).ToList();
                }
            }
        }
    }
}