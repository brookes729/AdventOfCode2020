using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2020
{
    public static class DayTwentythree
    {
        public static void Run()
        {
            var CurrentLine = "326519478";
            var PartOneCount = "";
            var PartTwoCount = 0L;

            var CurrentCups = CurrentLine.Select(c => int.Parse(Char.ToString(c))).ToList();
            var Rounds = 100;

            CurrentCups = RunGame(CurrentCups, Rounds);

            var IndexOfOne = CurrentCups.IndexOf(1);
            PartOneCount = CurrentCups.Skip(IndexOfOne + 1)
                                      .Concat(CurrentCups.Take(IndexOfOne))
                                      .Aggregate("", (a, b) => a + b);

            Console.WriteLine($"Part One Count: {PartOneCount}");

            CurrentCups = CurrentLine.Select(c => int.Parse(Char.ToString(c))).Concat(Enumerable.Range(10, 999991)).ToList();
            Rounds = 10000000;

            CurrentCups = RunGame(CurrentCups, Rounds);

            IndexOfOne = CurrentCups.IndexOf(1);

            PartTwoCount = (long)CurrentCups[IndexOfOne + 1] * (long)CurrentCups[IndexOfOne + 2];

            Console.WriteLine($"Part Two Count: {PartTwoCount}");
        }

        private static List<int> RunGame(List<int> currentCups, int rounds)
        {
            var CupsLinkedList = new LinkedList<int>(currentCups);
            var MaxCup = CupsLinkedList.Count();

            var CurrentCup = CupsLinkedList.First;

            var ValueToElementDictionary = new Dictionary<int, LinkedListNode<int>>();
            var CurrentElement = CupsLinkedList.First;
            for (int i = 0; i < currentCups.Count; i++)
            {
                ValueToElementDictionary.Add(CurrentElement.Value, CurrentElement);
                CurrentElement = CurrentElement.Next;
            }

            for (int i = 0; i < rounds; i++)
            {
                var PickedUpCups = new List<LinkedListNode<int>>();
                var NextPickUp = CurrentCup.Next;
                for (int j = 0; j < 3; j++)
                {
                    if (NextPickUp == null)
                    {
                        NextPickUp = CupsLinkedList.First;
                    }
                    PickedUpCups.Add(NextPickUp);
                    NextPickUp = NextPickUp.Next;
                }

                var Destination = CurrentCup.Value - 1;
                if (Destination == 0) Destination = MaxCup;

                while (PickedUpCups.Any(x => x.Value == Destination))
                {
                    Destination = Destination - 1;
                    if (Destination == 0) Destination = MaxCup;
                }

                var DestinationCupElement = ValueToElementDictionary[Destination];

                var AddAfterElement = DestinationCupElement;
                foreach (var PickedUpCup in PickedUpCups)
                {
                    CupsLinkedList.Remove(PickedUpCup);
                    CupsLinkedList.AddAfter(AddAfterElement, PickedUpCup);
                    AddAfterElement = PickedUpCup;
                }

                CurrentCup = CurrentCup.Next;
                if (CurrentCup == null)
                {
                    CurrentCup = CupsLinkedList.First;
                }
            }

            return CupsLinkedList.ToList();
        }
    }
}