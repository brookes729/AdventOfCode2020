using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2020
{
    public static class DayTwenty
    {
        public static void Run()
        {
            var File = new StreamReader(Path.GetFullPath("Resource/DayTwenty.txt"));

            var CurrentLine = string.Empty;
            var PartOneCount = 0L;
            var PartTwoCount = 0L;

            var PotentialBoxes = new List<Box>();
            var TileSize = 10; // could be calculated but being lazy

            while ((CurrentLine = File.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(CurrentLine))
                {
                    continue;
                }
                if (CurrentLine.StartsWith("Tile"))
                {
                    var Box = new Box() { Id = long.Parse(CurrentLine.Split(" ")[1].TrimEnd(':')) };

                    CurrentLine = File.ReadLine();
                    Box.Edges[0] = CurrentLine;
                    Box.Edges[1] = Char.ToString(CurrentLine[TileSize - 1]);
                    Box.Edges[3] = Char.ToString(CurrentLine[0]);

                    for (int i = 1; i < TileSize - 1; i++)
                    {
                        CurrentLine = File.ReadLine();
                        Box.Edges[1] += Char.ToString(CurrentLine[TileSize - 1]);
                        Box.Edges[3] += Char.ToString(CurrentLine[0]);
                        Box.InnerContent.Add(CurrentLine.Substring(1, TileSize - 2));
                    }

                    CurrentLine = File.ReadLine();
                    Box.Edges[2] = CurrentLine;
                    Box.Edges[1] += Char.ToString(CurrentLine[TileSize - 1]);
                    Box.Edges[3] += Char.ToString(CurrentLine[0]);

                    PotentialBoxes.Add(Box);
                }
            }
            File.Close();

            // Got Potential boxes
            // Set first one and work from there
            var BoxGrid = new Dictionary<int, Dictionary<int, Box>>() {
                {0, new Dictionary<int, Box>() { {0, PotentialBoxes[0]} }}
            };

            PotentialBoxes = PotentialBoxes.Skip(1).ToList();

            // while potential boxes count > 0
            while (PotentialBoxes.Count() > 0)
            {
                var NextGrid = BoxGrid.Select(x =>
                                            {
                                                var Value = x.Value.Select(y => y)
                                                                    .ToDictionary(y => y.Key, y => y.Value);
                                                return new { x.Key, Value };
                                            })
                                            .ToDictionary(x => x.Key, x => x.Value);

                //    FE Set Box FE Edge
                foreach (var KnownBoxRow in BoxGrid)
                {
                    foreach (var KnownBox in KnownBoxRow.Value)
                    {
                        var UpdatedKnownBox = new Box() { Id = KnownBox.Value.Id, InnerContent = KnownBox.Value.InnerContent };
                        foreach (var Edge in KnownBox.Value.Edges)
                        {
                            //      Look for PotentialMatches
                            var PotentialMatches = new List<Box>();
                            foreach (var PotentialBox in PotentialBoxes)
                            {
                                if (PotentialBox.Edges.Values.Any(edge => edge == Edge.Value || edge == string.Concat(Edge.Value.Reverse())))
                                {
                                    PotentialMatches.Add(PotentialBox);
                                }
                            }

                            if (PotentialMatches.Count() == 0)
                            {
                                //      if 0 remove Set Box Edge (maybe set marker for edge to help later calc)
                                //KnownBox.Value.Edges.Remove(Edge.Key);
                            }
                            else if (PotentialMatches.Count() == 1)
                            {
                                //      if 1 Set New Box
                                var NewBoxRowPos = KnownBoxRow.Key;
                                var NewBoxColPos = KnownBox.Key;

                                Box RotatedBox = null;

                                switch (Edge.Key)
                                {
                                    case 0:
                                        NewBoxRowPos--;
                                        if (Edge.Value == PotentialMatches[0].Edges[0])
                                        {
                                            RotatedBox = RotateAndFlipBox(PotentialMatches[0], 2, true);
                                        }
                                        else if (Edge.Value == PotentialMatches[0].Edges[1])
                                        {
                                            RotatedBox = RotateAndFlipBox(PotentialMatches[0], 1, true);
                                        }
                                        else if (Edge.Value == PotentialMatches[0].Edges[2])
                                        {
                                            RotatedBox = PotentialMatches[0];
                                        }
                                        else if (Edge.Value == PotentialMatches[0].Edges[3])
                                        {
                                            RotatedBox = RotateAndFlipBox(PotentialMatches[0], 3, false);
                                        }
                                        else if (Edge.Value == string.Concat(PotentialMatches[0].Edges[0].Reverse()))
                                        {
                                            RotatedBox = RotateAndFlipBox(PotentialMatches[0], 2, false);
                                        }
                                        else if (Edge.Value == string.Concat(PotentialMatches[0].Edges[1].Reverse()))
                                        {
                                            RotatedBox = RotateAndFlipBox(PotentialMatches[0], 1, false);
                                        }
                                        else if (Edge.Value == string.Concat(PotentialMatches[0].Edges[2].Reverse()))
                                        {
                                            RotatedBox = RotateAndFlipBox(PotentialMatches[0], 0, true);
                                        }
                                        else if (Edge.Value == string.Concat(PotentialMatches[0].Edges[3].Reverse()))
                                        {
                                            RotatedBox = RotateAndFlipBox(PotentialMatches[0], 3, true);
                                        }
                                        RotatedBox.Edges.Remove(2);
                                        break;
                                    case 1:
                                        NewBoxColPos++;
                                        if (Edge.Value == PotentialMatches[0].Edges[0])
                                        {
                                            RotatedBox = RotateAndFlipBox(PotentialMatches[0], 1, true);
                                        }
                                        else if (Edge.Value == PotentialMatches[0].Edges[1])
                                        {
                                            RotatedBox = RotateAndFlipBox(PotentialMatches[0], 0, true);
                                        }
                                        else if (Edge.Value == PotentialMatches[0].Edges[2])
                                        {
                                            RotatedBox = RotateAndFlipBox(PotentialMatches[0], 1, false);
                                        }
                                        else if (Edge.Value == PotentialMatches[0].Edges[3])
                                        {
                                            RotatedBox = PotentialMatches[0];
                                        }
                                        else if (Edge.Value == string.Concat(PotentialMatches[0].Edges[0].Reverse()))
                                        {
                                            RotatedBox = RotateAndFlipBox(PotentialMatches[0], 3, false);
                                        }
                                        else if (Edge.Value == string.Concat(PotentialMatches[0].Edges[1].Reverse()))
                                        {
                                            RotatedBox = RotateAndFlipBox(PotentialMatches[0], 2, false);
                                        }
                                        else if (Edge.Value == string.Concat(PotentialMatches[0].Edges[2].Reverse()))
                                        {
                                            RotatedBox = RotateAndFlipBox(PotentialMatches[0], 3, true);
                                        }
                                        else if (Edge.Value == string.Concat(PotentialMatches[0].Edges[3].Reverse()))
                                        {
                                            RotatedBox = RotateAndFlipBox(PotentialMatches[0], 2, true);
                                        }
                                        RotatedBox.Edges.Remove(3);
                                        break;
                                    case 2:
                                        NewBoxRowPos++;
                                        if (Edge.Value == PotentialMatches[0].Edges[0])
                                        {
                                            RotatedBox = PotentialMatches[0];
                                        }
                                        else if (Edge.Value == PotentialMatches[0].Edges[1])
                                        {
                                            RotatedBox = RotateAndFlipBox(PotentialMatches[0], 3, false);
                                        }
                                        else if (Edge.Value == PotentialMatches[0].Edges[2])
                                        {
                                            RotatedBox = RotateAndFlipBox(PotentialMatches[0], 2, true);
                                        }
                                        else if (Edge.Value == PotentialMatches[0].Edges[3])
                                        {
                                            RotatedBox = RotateAndFlipBox(PotentialMatches[0], 1, true);
                                        }
                                        else if (Edge.Value == string.Concat(PotentialMatches[0].Edges[0].Reverse()))
                                        {
                                            RotatedBox = RotateAndFlipBox(PotentialMatches[0], 0, true);
                                        }
                                        else if (Edge.Value == string.Concat(PotentialMatches[0].Edges[1].Reverse()))
                                        {
                                            RotatedBox = RotateAndFlipBox(PotentialMatches[0], 3, true);
                                        }
                                        else if (Edge.Value == string.Concat(PotentialMatches[0].Edges[2].Reverse()))
                                        {
                                            RotatedBox = RotateAndFlipBox(PotentialMatches[0], 2, false);
                                        }
                                        else if (Edge.Value == string.Concat(PotentialMatches[0].Edges[3].Reverse()))
                                        {
                                            RotatedBox = RotateAndFlipBox(PotentialMatches[0], 1, false);
                                        }
                                        RotatedBox.Edges.Remove(0);
                                        break;
                                    case 3:
                                        NewBoxColPos--;
                                        if (Edge.Value == PotentialMatches[0].Edges[0])
                                        {
                                            RotatedBox = RotateAndFlipBox(PotentialMatches[0], 1, false);
                                        }
                                        else if (Edge.Value == PotentialMatches[0].Edges[1])
                                        {
                                            RotatedBox = PotentialMatches[0];
                                        }
                                        else if (Edge.Value == PotentialMatches[0].Edges[2])
                                        {
                                            RotatedBox = RotateAndFlipBox(PotentialMatches[0], 1, true);
                                        }
                                        else if (Edge.Value == PotentialMatches[0].Edges[3])
                                        {
                                            RotatedBox = RotateAndFlipBox(PotentialMatches[0], 0, true);
                                        }
                                        else if (Edge.Value == string.Concat(PotentialMatches[0].Edges[0].Reverse()))
                                        {
                                            RotatedBox = RotateAndFlipBox(PotentialMatches[0], 3, true);
                                        }
                                        else if (Edge.Value == string.Concat(PotentialMatches[0].Edges[1].Reverse()))
                                        {
                                            RotatedBox = RotateAndFlipBox(PotentialMatches[0], 2, true);
                                        }
                                        else if (Edge.Value == string.Concat(PotentialMatches[0].Edges[2].Reverse()))
                                        {
                                            RotatedBox = RotateAndFlipBox(PotentialMatches[0], 3, false);
                                        }
                                        else if (Edge.Value == string.Concat(PotentialMatches[0].Edges[3].Reverse()))
                                        {
                                            RotatedBox = RotateAndFlipBox(PotentialMatches[0], 2, false);
                                        }
                                        RotatedBox.Edges.Remove(1);
                                        break;
                                }

                                if (!NextGrid.ContainsKey(NewBoxRowPos)) NextGrid[NewBoxRowPos] = new Dictionary<int, Box>();
                                NextGrid[NewBoxRowPos][NewBoxColPos] = RotatedBox;

                                PotentialBoxes.Remove(PotentialMatches[0]);
                            }
                            else
                            {
                                UpdatedKnownBox.Edges.Add(Edge.Key, Edge.Value);
                                //      else check for boxes around the new one
                                //         if matches edges set
                                //         if still multiple move on  
                            }
                        }

                        NextGrid[KnownBoxRow.Key][KnownBox.Key] = UpdatedKnownBox;
                    }
                }

                BoxGrid = NextGrid;
            }

            // Find corners, calc part 1
            var MinRow = BoxGrid.Keys.Min();
            var MaxRow = BoxGrid.Keys.Max();
            var MinCol = BoxGrid[MinRow].Keys.Min();
            var MaxCol = BoxGrid[MinRow].Keys.Max();

            PartOneCount = BoxGrid[MinRow][MinCol].Id * BoxGrid[MinRow][MaxCol].Id * BoxGrid[MaxRow][MinCol].Id * BoxGrid[MaxRow][MaxCol].Id;

            var CombinedInnerContent = CombineGridForInnerContent(BoxGrid);

            PartTwoCount = FlipAndRoateToFindSeaMonster(CombinedInnerContent);

            Console.WriteLine($"Part One Count: {PartOneCount}");
            Console.WriteLine($"Part Two Count: {PartTwoCount}");
        }

        private static long FlipAndRoateToFindSeaMonster(List<string> combinedInnerContent)
        {
            var NumberOfSeaMonsters = FindNumberOfSeaMonster(combinedInnerContent);
            // Try to rotate
            NumberOfSeaMonsters = NumberOfSeaMonsters != 0 ?
                                        NumberOfSeaMonsters
                                        : FindNumberOfSeaMonster(
                                            RotateInnerContent90Clockwise(combinedInnerContent));
            NumberOfSeaMonsters = NumberOfSeaMonsters != 0 ?
                                        NumberOfSeaMonsters
                                        : FindNumberOfSeaMonster(
                                            RotateInnerContent90Clockwise(
                                                RotateInnerContent90Clockwise(combinedInnerContent)));
            NumberOfSeaMonsters = NumberOfSeaMonsters != 0 ?
                                        NumberOfSeaMonsters
                                        : FindNumberOfSeaMonster(
                                            RotateInnerContent90Clockwise(
                                                RotateInnerContent90Clockwise(
                                                    RotateInnerContent90Clockwise(combinedInnerContent))));

            // Try to flip and rotate
            NumberOfSeaMonsters = NumberOfSeaMonsters != 0 ?
                                        NumberOfSeaMonsters
                                        : FindNumberOfSeaMonster(
                                            FlipInnerContent(combinedInnerContent));
            NumberOfSeaMonsters = NumberOfSeaMonsters != 0 ?
                                        NumberOfSeaMonsters
                                        : FindNumberOfSeaMonster(
                                            FlipInnerContent(
                                                RotateInnerContent90Clockwise(combinedInnerContent)));
            NumberOfSeaMonsters = NumberOfSeaMonsters != 0 ?
                                        NumberOfSeaMonsters
                                        : FindNumberOfSeaMonster(
                                            FlipInnerContent(
                                                RotateInnerContent90Clockwise(
                                                    RotateInnerContent90Clockwise(combinedInnerContent))));
            NumberOfSeaMonsters = NumberOfSeaMonsters != 0 ?
                                        NumberOfSeaMonsters
                                        : FindNumberOfSeaMonster(
                                            FlipInnerContent(
                                                RotateInnerContent90Clockwise(
                                                    RotateInnerContent90Clockwise(
                                                        RotateInnerContent90Clockwise(combinedInnerContent)))));

            return combinedInnerContent.Sum(s => s.Count(c => c == '#')) - 15 * NumberOfSeaMonsters;
        }

        private static long FindNumberOfSeaMonster(List<string> combinedInnerContent)
        {
            var Monsters = 0;
            for (var i = 0; i < combinedInnerContent.Count() - 2; i++)
            {
                var HeadSectionStart = 0;
                var HeadSection = combinedInnerContent[i];
                var HeadMatch = Regex.Match(combinedInnerContent[i], ".{18}#.");
                while (HeadMatch.Success)
                {
                    var BodySection = combinedInnerContent[i + 1].Substring(HeadSectionStart + HeadMatch.Index, 20);
                    var BodyMatch = Regex.Match(BodySection, "#.{4}##.{4}##.{4}###");
                    if (BodyMatch.Success)
                    {
                        var TailSection = combinedInnerContent[i + 2].Substring(HeadSectionStart + HeadMatch.Index, 20);
                        var TailMatch = Regex.Match(TailSection, ".{1}#.{2}#.{2}#.{2}#.{2}#.{2}#.{3}");
                        if (TailMatch.Success)
                        {
                            Monsters++;
                        }
                    }
                    HeadSectionStart += HeadMatch.Index + 1;
                    HeadSection = HeadSection.Substring(HeadMatch.Index + 1);
                    HeadMatch = Regex.Match(HeadSection, ".{18}#.");
                }
            }

            return Monsters;
        }

        private static List<string> CombineGridForInnerContent(Dictionary<int, Dictionary<int, Box>> boxGrid)
        {
            var MinRow = boxGrid.Keys.Min();
            var MaxRow = boxGrid.Keys.Max();
            var MinCol = boxGrid[MinRow].Keys.Min();
            var MaxCol = boxGrid[MinRow].Keys.Max();

            var CombinedInnerContent = new Dictionary<int, string>();

            for (var i = MinRow; i <= MaxRow; i++)
            {
                var StartingInnerRow = CombinedInnerContent.Count;
                for (var j = MinCol; j <= MaxCol; j++)
                {
                    for (var k = 0; k < boxGrid[i][j].InnerContent.Count(); k++)
                    {
                        CombinedInnerContent[StartingInnerRow + k] = (CombinedInnerContent.ContainsKey(StartingInnerRow + k) ?
                                                                        CombinedInnerContent[StartingInnerRow + k] : "")
                                                                        + boxGrid[i][j].InnerContent[k];
                    }
                }
            }

            return CombinedInnerContent.Values.ToList();
        }

        private static Box RotateAndFlipBox(Box box, int rotations, bool flipHorizontally)
        {
            var NewBox = new Box() { Id = box.Id };

            if (flipHorizontally)
            {
                switch (rotations)
                {
                    case 0:
                        NewBox.Edges[0] = string.Concat(box.Edges[0].Reverse());
                        NewBox.Edges[1] = box.Edges[3];
                        NewBox.Edges[2] = string.Concat(box.Edges[2].Reverse());
                        NewBox.Edges[3] = box.Edges[1];
                        NewBox.InnerContent = FlipInnerContent(box.InnerContent);
                        break;
                    case 1:
                        NewBox.Edges[0] = box.Edges[3];
                        NewBox.Edges[1] = box.Edges[2];
                        NewBox.Edges[2] = box.Edges[1];
                        NewBox.Edges[3] = box.Edges[0];
                        NewBox.InnerContent = FlipInnerContent(RotateInnerContent90Clockwise(box.InnerContent));
                        break;
                    case 2:
                        NewBox.Edges[0] = box.Edges[2];
                        NewBox.Edges[1] = string.Concat(box.Edges[1].Reverse());
                        NewBox.Edges[2] = box.Edges[0];
                        NewBox.Edges[3] = string.Concat(box.Edges[3].Reverse());
                        NewBox.InnerContent = FlipInnerContent(
                                                RotateInnerContent90Clockwise(
                                                    RotateInnerContent90Clockwise(box.InnerContent)));
                        break;
                    case 3:
                        NewBox.Edges[0] = string.Concat(box.Edges[1].Reverse());
                        NewBox.Edges[1] = string.Concat(box.Edges[0].Reverse());
                        NewBox.Edges[2] = string.Concat(box.Edges[3].Reverse());
                        NewBox.Edges[3] = string.Concat(box.Edges[2].Reverse());
                        NewBox.InnerContent = FlipInnerContent(
                                                RotateInnerContent90Clockwise(
                                                    RotateInnerContent90Clockwise(
                                                        RotateInnerContent90Clockwise(box.InnerContent))));
                        break;
                }
            }
            else
            {
                switch (rotations)
                {
                    case 1:
                        NewBox.Edges[0] = string.Concat(box.Edges[3].Reverse());
                        NewBox.Edges[1] = box.Edges[0];
                        NewBox.Edges[2] = string.Concat(box.Edges[1].Reverse());
                        NewBox.Edges[3] = box.Edges[2];
                        NewBox.InnerContent = RotateInnerContent90Clockwise(box.InnerContent);
                        break;
                    case 2:
                        NewBox.Edges[0] = string.Concat(box.Edges[2].Reverse());
                        NewBox.Edges[1] = string.Concat(box.Edges[3].Reverse());
                        NewBox.Edges[2] = string.Concat(box.Edges[0].Reverse());
                        NewBox.Edges[3] = string.Concat(box.Edges[1].Reverse());
                        NewBox.InnerContent = RotateInnerContent90Clockwise(
                                                    RotateInnerContent90Clockwise(box.InnerContent));
                        break;
                    case 3:
                        NewBox.Edges[0] = box.Edges[1];
                        NewBox.Edges[1] = string.Concat(box.Edges[2].Reverse());
                        NewBox.Edges[2] = box.Edges[3];
                        NewBox.Edges[3] = string.Concat(box.Edges[0].Reverse());
                        NewBox.InnerContent = RotateInnerContent90Clockwise(
                                                    RotateInnerContent90Clockwise(
                                                        RotateInnerContent90Clockwise(box.InnerContent)));
                        break;
                }
            }

            return NewBox;
        }

        private static List<string> FlipInnerContent(List<string> innerContent)
        {
            var NewContent = new List<string>();

            foreach (var innerContentLine in innerContent)
            {
                NewContent.Add(string.Concat(innerContentLine.Reverse()));
            }

            return NewContent;
        }
        private static List<string> RotateInnerContent90Clockwise(List<string> innerContent)
        {
            var NewContent = new Dictionary<int, string>();

            foreach (var InnerContentLine in innerContent)
            {
                var ColNumber = 0;
                foreach (var InnerContentChar in InnerContentLine)
                {
                    NewContent[ColNumber] = InnerContentChar + (NewContent.ContainsKey(ColNumber) ? NewContent[ColNumber] : "");
                    ColNumber++;
                }
            }

            return NewContent.Values.ToList();
        }
    }

    public class Box
    {
        public long Id;
        public Dictionary<int, string> Edges = new Dictionary<int, string>();

        public List<string> InnerContent = new List<string>();
    }
}