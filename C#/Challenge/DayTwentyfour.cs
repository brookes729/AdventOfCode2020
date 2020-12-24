using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2020
{
    public static class DayTwentyfour
    {
        public static void Run()
        {
            var File = new StreamReader(Path.GetFullPath("Resource/DayTwentyfour.txt"));

            var CurrentLine = string.Empty;
            var PartOneCount = 0L;
            var PartTwoCount = 0L;

            var BlackTiles = new Dictionary<int, Dictionary<int, bool>>();

            while ((CurrentLine = File.ReadLine()) != null)
            {
                var RowIndex = 0;
                var ColIndex = 0;

                for (int i = 0; i < CurrentLine.Length; i++)
                {
                    switch (CurrentLine[i])
                    {
                        case 'e':
                            ColIndex++;
                            break;
                        case 'w':
                            ColIndex--;
                            break;
                        case 'n':
                            i++;
                            RowIndex--;
                            if (CurrentLine[i] == 'e')
                            {
                                ColIndex = RowIndex % 2 == 0 ? ColIndex : ColIndex + 1;
                            }
                            else
                            {
                                ColIndex = RowIndex % 2 == 0 ? ColIndex - 1 : ColIndex;
                            }
                            break;
                        case 's':
                            i++;
                            RowIndex++;
                            if (CurrentLine[i] == 'e')
                            {
                                ColIndex = RowIndex % 2 == 0 ? ColIndex : ColIndex + 1;
                            }
                            else
                            {
                                ColIndex = RowIndex % 2 == 0 ? ColIndex - 1 : ColIndex;
                            }
                            break;
                    }
                }

                if (!BlackTiles.ContainsKey(RowIndex)) BlackTiles[RowIndex] = new Dictionary<int, bool>();

                BlackTiles[RowIndex][ColIndex] = !(BlackTiles[RowIndex].ContainsKey(ColIndex) && BlackTiles[RowIndex][ColIndex]);
            }
            File.Close();

            PartOneCount = BlackTiles.Sum(x => x.Value.Count(y => y.Value));

            for (int i = 0; i < 100; i++)
            {
                var BlackTileAdjacentCounts = new Dictionary<int, Dictionary<int, int>>();
                foreach (var Row in BlackTiles)
                {
                    foreach (var Tile in Row.Value)
                    {
                        if (Tile.Value)
                        {
                            if (!BlackTileAdjacentCounts.ContainsKey(Row.Key - 1)) BlackTileAdjacentCounts[Row.Key - 1] = new Dictionary<int, int>();
                            if (!BlackTileAdjacentCounts.ContainsKey(Row.Key)) BlackTileAdjacentCounts[Row.Key] = new Dictionary<int, int>();
                            if (!BlackTileAdjacentCounts.ContainsKey(Row.Key + 1)) BlackTileAdjacentCounts[Row.Key + 1] = new Dictionary<int, int>();

                            var DiagEast = Row.Key % 2 == 0 ? Tile.Key + 1 : Tile.Key;

                            // Make sure we have the current tile in the counts
                            BlackTileAdjacentCounts[Row.Key][Tile.Key] = BlackTileAdjacentCounts[Row.Key].ContainsKey(Tile.Key) ?
                                                                            BlackTileAdjacentCounts[Row.Key][Tile.Key] : 0;

                            // Add adjacent tiles
                            BlackTileAdjacentCounts[Row.Key][Tile.Key + 1] = BlackTileAdjacentCounts[Row.Key].ContainsKey(Tile.Key + 1) ?
                                                                            BlackTileAdjacentCounts[Row.Key][Tile.Key + 1] + 1 : 1;
                            BlackTileAdjacentCounts[Row.Key][Tile.Key - 1] = BlackTileAdjacentCounts[Row.Key].ContainsKey(Tile.Key - 1) ?
                                                                            BlackTileAdjacentCounts[Row.Key][Tile.Key - 1] + 1 : 1;
                            BlackTileAdjacentCounts[Row.Key + 1][DiagEast] = BlackTileAdjacentCounts[Row.Key + 1].ContainsKey(DiagEast) ?
                                                                            BlackTileAdjacentCounts[Row.Key + 1][DiagEast] + 1 : 1;
                            BlackTileAdjacentCounts[Row.Key + 1][DiagEast - 1] = BlackTileAdjacentCounts[Row.Key + 1].ContainsKey(DiagEast - 1) ?
                                                                            BlackTileAdjacentCounts[Row.Key + 1][DiagEast - 1] + 1 : 1;
                            BlackTileAdjacentCounts[Row.Key - 1][DiagEast] = BlackTileAdjacentCounts[Row.Key - 1].ContainsKey(DiagEast) ?
                                                                            BlackTileAdjacentCounts[Row.Key - 1][DiagEast] + 1 : 1;
                            BlackTileAdjacentCounts[Row.Key - 1][DiagEast - 1] = BlackTileAdjacentCounts[Row.Key - 1].ContainsKey(DiagEast - 1) ?
                                                                            BlackTileAdjacentCounts[Row.Key - 1][DiagEast - 1] + 1 : 1;
                        }
                    }
                }

                foreach (var RowCount in BlackTileAdjacentCounts)
                {
                    foreach (var TileCount in RowCount.Value)
                    {
                        if (!BlackTiles.ContainsKey(RowCount.Key)) BlackTiles[RowCount.Key] = new Dictionary<int, bool>();
                        if (BlackTiles[RowCount.Key].ContainsKey(TileCount.Key) && BlackTiles[RowCount.Key][TileCount.Key])
                        {
                            BlackTiles[RowCount.Key][TileCount.Key] = TileCount.Value == 1 || TileCount.Value == 2;
                        }
                        else
                        {
                            BlackTiles[RowCount.Key][TileCount.Key] = TileCount.Value == 2;
                        }
                    }
                }
            }
            PartTwoCount = BlackTiles.Sum(x => x.Value.Count(y => y.Value));

            Console.WriteLine($"Part One Count: {PartOneCount}");
            Console.WriteLine($"Part Two Count: {PartTwoCount}");
        }
    }
}