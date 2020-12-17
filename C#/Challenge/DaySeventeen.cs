using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2020
{
    public static class DaySeventeen
    {
        public static void RunPart1()
        {
            var File = new StreamReader(Path.GetFullPath("Resource/DaySeventeen.txt"));

            var CurrentLine = string.Empty;
            var PartOneCount = 0;

            var Cubes = new Dictionary<int, Dictionary<int, Dictionary<int, char>>>()
            {
                { 0, new Dictionary<int, Dictionary<int, char>>() }
            };
            var RowNumber = 0;

            while ((CurrentLine = File.ReadLine()) != null)
            {
                Cubes[0].Add(RowNumber, CurrentLine.Select((x, i) => new { x, i }).ToDictionary(kvp => kvp.i, kvp => kvp.x));
                RowNumber++;
            }
            File.Close();

            var NextCubes = new Dictionary<int, Dictionary<int, Dictionary<int, char>>>();
            for (var Cycles = 0; Cycles < 6; Cycles++)
            {

                var NearbyCubeCounts = new Dictionary<int, Dictionary<int, Dictionary<int, int>>>();

                NextCubes = new Dictionary<int, Dictionary<int, Dictionary<int, char>>>();

                foreach (var ZSelection in Cubes)
                {
                    foreach (var YSelection in ZSelection.Value)
                    {
                        foreach (var XSelection in YSelection.Value)
                        {
                            if (Cubes[ZSelection.Key][YSelection.Key][XSelection.Key] == '#')
                            {
                                IncreaseCubeCount(ref NearbyCubeCounts, ZSelection.Key, YSelection.Key, XSelection.Key);
                            }
                        }
                    }
                }


                foreach (var ZSelection in NearbyCubeCounts)
                {
                    foreach (var YSelection in ZSelection.Value)
                    {
                        foreach (var XSelection in YSelection.Value)
                        {
                            if (!NextCubes.ContainsKey(ZSelection.Key))
                                NextCubes[ZSelection.Key] = new Dictionary<int, Dictionary<int, char>>();
                            if (!NextCubes[ZSelection.Key].ContainsKey(YSelection.Key))
                                NextCubes[ZSelection.Key][YSelection.Key] = new Dictionary<int, char>();

                            var CurrentValue = '.';

                            if (Cubes.ContainsKey(ZSelection.Key)
                                && Cubes[ZSelection.Key].ContainsKey(YSelection.Key)
                                && Cubes[ZSelection.Key][YSelection.Key].ContainsKey(XSelection.Key))
                            {
                                CurrentValue = Cubes[ZSelection.Key][YSelection.Key][XSelection.Key];
                            }

                            if (CurrentValue == '#'
                                && (XSelection.Value == 2 || XSelection.Value == 3))
                            {
                                NextCubes[ZSelection.Key][YSelection.Key].Add(XSelection.Key, '#');

                            }
                            else if (CurrentValue != '#'
                              && XSelection.Value == 3)
                            {
                                NextCubes[ZSelection.Key][YSelection.Key].Add(XSelection.Key, '#');
                            }
                            else
                            {
                                NextCubes[ZSelection.Key][YSelection.Key].Add(XSelection.Key, '.');
                            }
                        }
                    }
                }

                Cubes = NextCubes;

                PartOneCount = Cubes.Sum(z => z.Value.Sum(y => y.Value.Sum(x => x.Value == '#' ? 1 : 0)));

                Console.WriteLine($"Cycle {Cycles}: Part One Count: {PartOneCount}");
            }


            Console.WriteLine($"Part One Count: {PartOneCount}");
        }

        public static void RunPart2()
        {
            var File = new StreamReader(Path.GetFullPath("Resource/DaySeventeen.txt"));

            var CurrentLine = string.Empty;
            var PartTwoCount = 0;

            var Cubes = new Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, char>>>>()
            {
                { 0, new Dictionary<int, Dictionary<int, Dictionary<int, char>>>() }
            };
            Cubes[0][0] = new Dictionary<int, Dictionary<int, char>>();
            var RowNumber = 0;

            while ((CurrentLine = File.ReadLine()) != null)
            {
                Cubes[0][0].Add(RowNumber, CurrentLine.Select((x, i) => new { x, i }).ToDictionary(kvp => kvp.i, kvp => kvp.x));
                RowNumber++;
            }
            File.Close();

            var NextCubes = new Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, char>>>>();
            for (var Cycles = 0; Cycles < 6; Cycles++)
            {

                var NearbyCubeCounts = new Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, int>>>>();

                NextCubes = new Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, char>>>>();

                foreach (var ZSelection in Cubes)
                {
                    foreach (var YSelection in ZSelection.Value)
                    {
                        foreach (var XSelection in YSelection.Value)
                        {
                            foreach (var WSelection in XSelection.Value)
                            {
                                if (Cubes[ZSelection.Key][YSelection.Key][XSelection.Key][WSelection.Key] == '#')
                                {
                                    IncreaseCubeCount2(ref NearbyCubeCounts, ZSelection.Key, YSelection.Key, XSelection.Key, WSelection.Key);
                                }
                            }
                        }
                    }
                }


                foreach (var ZSelection in NearbyCubeCounts)
                {
                    foreach (var YSelection in ZSelection.Value)
                    {
                        foreach (var XSelection in YSelection.Value)
                        {
                            foreach (var WSelection in XSelection.Value)
                            {
                                if (!NextCubes.ContainsKey(ZSelection.Key))
                                    NextCubes[ZSelection.Key] = new Dictionary<int, Dictionary<int, Dictionary<int, char>>>();
                                if (!NextCubes[ZSelection.Key].ContainsKey(YSelection.Key))
                                    NextCubes[ZSelection.Key][YSelection.Key] = new Dictionary<int, Dictionary<int, char>>();
                                if (!NextCubes[ZSelection.Key][YSelection.Key].ContainsKey(XSelection.Key))
                                    NextCubes[ZSelection.Key][YSelection.Key][XSelection.Key] = new Dictionary<int, char>();

                                var CurrentValue = '.';

                                if (Cubes.ContainsKey(ZSelection.Key)
                                    && Cubes[ZSelection.Key].ContainsKey(YSelection.Key)
                                    && Cubes[ZSelection.Key][YSelection.Key].ContainsKey(XSelection.Key)
                                    && Cubes[ZSelection.Key][YSelection.Key][XSelection.Key].ContainsKey(WSelection.Key))
                                {
                                    CurrentValue = Cubes[ZSelection.Key][YSelection.Key][XSelection.Key][WSelection.Key];
                                }

                                if (CurrentValue == '#'
                                    && (WSelection.Value == 2 || WSelection.Value == 3))
                                {
                                    NextCubes[ZSelection.Key][YSelection.Key][XSelection.Key].Add(WSelection.Key, '#');

                                }
                                else if (CurrentValue != '#'
                                  && WSelection.Value == 3)
                                {
                                    NextCubes[ZSelection.Key][YSelection.Key][XSelection.Key].Add(WSelection.Key, '#');
                                }
                                else
                                {
                                    NextCubes[ZSelection.Key][YSelection.Key][XSelection.Key].Add(WSelection.Key, '.');
                                }
                            }
                        }
                    }
                }

                Cubes = NextCubes;

                PartTwoCount = Cubes.Sum(z => z.Value.Sum(y => y.Value.Sum(x => x.Value.Sum(w => w.Value == '#' ? 1 : 0))));

                Console.WriteLine($"Cycle {Cycles}: Part Two Count: {PartTwoCount}");
            }

            Console.WriteLine($"Part Two Count: {PartTwoCount}");
        }

        private static void IncreaseCubeCount2(ref Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, int>>>> nearbyCubeCounts,
                                                int z, int y, int x, int w)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    for (int k = -1; k < 2; k++)
                    {
                        for (int l = -1; l < 2; l++)
                        {
                            if (!(i == 0 && j == 0 && k == 0 && l == 0))
                            {
                                IncreaseIndividualCubeCount2(ref nearbyCubeCounts, z, y, x, w, i, j, k, l);
                            }
                        }
                    }
                }
            }
        }

        private static void IncreaseIndividualCubeCount2(ref Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, int>>>> nearbyCubeCounts,
                                                        int z, int y, int x, int w,
                                                        int directionZ, int directionY, int directionX, int directionW)
        {
            var NewZ = z + directionZ;
            var NewY = y + directionY;
            var NewX = x + directionX;
            var NewW = w + directionW;

            if (!nearbyCubeCounts.ContainsKey(NewZ)) nearbyCubeCounts[NewZ] = new Dictionary<int, Dictionary<int, Dictionary<int, int>>>();
            if (!nearbyCubeCounts[NewZ].ContainsKey(NewY)) nearbyCubeCounts[NewZ][NewY] = new Dictionary<int, Dictionary<int, int>>();
            if (!nearbyCubeCounts[NewZ][NewY].ContainsKey(NewX)) nearbyCubeCounts[NewZ][NewY][NewX] = new Dictionary<int, int>();

            nearbyCubeCounts[NewZ][NewY][NewX][NewW] = nearbyCubeCounts[NewZ][NewY][NewX].ContainsKey(NewW) ?
                                                                    nearbyCubeCounts[NewZ][NewY][NewX][NewW] + 1
                                                                    : 1;
        }

        private static void IncreaseCubeCount(ref Dictionary<int, Dictionary<int, Dictionary<int, int>>> nearbyCubeCounts,
                                                int z,
                                                int y,
                                                int x)
        {
            IncreaseIndividualCubeCount(ref nearbyCubeCounts, z, y, x, 1, -1, -1);
            IncreaseIndividualCubeCount(ref nearbyCubeCounts, z, y, x, 1, -1, 0);
            IncreaseIndividualCubeCount(ref nearbyCubeCounts, z, y, x, 1, -1, 1);
            IncreaseIndividualCubeCount(ref nearbyCubeCounts, z, y, x, 1, 0, -1);
            IncreaseIndividualCubeCount(ref nearbyCubeCounts, z, y, x, 1, 0, 0);
            IncreaseIndividualCubeCount(ref nearbyCubeCounts, z, y, x, 1, 0, 1);
            IncreaseIndividualCubeCount(ref nearbyCubeCounts, z, y, x, 1, 1, -1);
            IncreaseIndividualCubeCount(ref nearbyCubeCounts, z, y, x, 1, 1, 0);
            IncreaseIndividualCubeCount(ref nearbyCubeCounts, z, y, x, 1, 1, 1);

            IncreaseIndividualCubeCount(ref nearbyCubeCounts, z, y, x, 0, -1, -1);
            IncreaseIndividualCubeCount(ref nearbyCubeCounts, z, y, x, 0, -1, 0);
            IncreaseIndividualCubeCount(ref nearbyCubeCounts, z, y, x, 0, -1, 1);
            IncreaseIndividualCubeCount(ref nearbyCubeCounts, z, y, x, 0, 0, -1);
            IncreaseIndividualCubeCount(ref nearbyCubeCounts, z, y, x, 0, 0, 1);
            IncreaseIndividualCubeCount(ref nearbyCubeCounts, z, y, x, 0, 1, -1);
            IncreaseIndividualCubeCount(ref nearbyCubeCounts, z, y, x, 0, 1, 0);
            IncreaseIndividualCubeCount(ref nearbyCubeCounts, z, y, x, 0, 1, 1);

            IncreaseIndividualCubeCount(ref nearbyCubeCounts, z, y, x, -1, -1, -1);
            IncreaseIndividualCubeCount(ref nearbyCubeCounts, z, y, x, -1, -1, 0);
            IncreaseIndividualCubeCount(ref nearbyCubeCounts, z, y, x, -1, -1, 1);
            IncreaseIndividualCubeCount(ref nearbyCubeCounts, z, y, x, -1, 0, -1);
            IncreaseIndividualCubeCount(ref nearbyCubeCounts, z, y, x, -1, 0, 0);
            IncreaseIndividualCubeCount(ref nearbyCubeCounts, z, y, x, -1, 0, 1);
            IncreaseIndividualCubeCount(ref nearbyCubeCounts, z, y, x, -1, 1, -1);
            IncreaseIndividualCubeCount(ref nearbyCubeCounts, z, y, x, -1, 1, 0);
            IncreaseIndividualCubeCount(ref nearbyCubeCounts, z, y, x, -1, 1, 1);
        }

        private static void IncreaseIndividualCubeCount(ref Dictionary<int, Dictionary<int, Dictionary<int, int>>> nearbyCubeCounts,
                                                        int z, int y, int x,
                                                        int directionZ, int directionY, int directionX, int Increase = 1)
        {
            var NewZ = z + directionZ;
            var NewY = y + directionY;
            var NewX = x + directionX;

            if (!nearbyCubeCounts.ContainsKey(NewZ)) nearbyCubeCounts[NewZ] = new Dictionary<int, Dictionary<int, int>>();
            if (!nearbyCubeCounts[NewZ].ContainsKey(NewY)) nearbyCubeCounts[NewZ][NewY] = new Dictionary<int, int>();

            nearbyCubeCounts[NewZ][NewY][NewX] = nearbyCubeCounts[NewZ][NewY].ContainsKey(NewX) ?
                                                                    nearbyCubeCounts[NewZ][NewY][NewX] + Increase
                                                                    : Increase;
        }
    }
}