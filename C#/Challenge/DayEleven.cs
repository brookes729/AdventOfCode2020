using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2020
{
    public static class DayEleven
    {
        public static void Run()
        {
            var File = new StreamReader(Path.GetFullPath("Resource/DayEleven.txt"));

            var CurrentLine = string.Empty;
            var PartOneCount = 0;
            var PartTwoCount = 0d;

            var Seats = new Dictionary<int, Dictionary<int, char>>();
            var RowNumber = 0;

            while ((CurrentLine = File.ReadLine()) != null)
            {
                Seats.Add(RowNumber, CurrentLine.Select((x, i) =>
                {
                    x = x == 'L' ? '#' : '.';
                    return new { x, i };
                }).ToDictionary(kvp => kvp.i, kvp => kvp.x));
                RowNumber++;
            }
            File.Close();

            var MaxRows = Seats.Count;
            var MaxCols = Seats[0].Count;
            var PartOneSeats = Seats.Select(x => x).ToDictionary(x => x.Key, x => x.Value);

            var NextSeats = new Dictionary<int, Dictionary<int, char>>();
            var Changes = true;
            // Part one
            while (Changes)
            {
                Changes = false;

                var NearbySeatsCounts = new Dictionary<int, Dictionary<int, int>>();
                NearbySeatsCounts[-1] = new Dictionary<int, int>();
                NearbySeatsCounts[0] = new Dictionary<int, int>();

                NextSeats = new Dictionary<int, Dictionary<int, char>>();

                for (int i = 0; i < PartOneSeats.Count(); i++)
                {
                    NearbySeatsCounts[i + 1] = new Dictionary<int, int>();

                    for (int j = 0; j < PartOneSeats[i].Count; j++)
                    {
                        if (PartOneSeats[i][j] == '#')
                        {
                            IncreaseSeatCount(ref NearbySeatsCounts, i, j, -1, -1);
                            IncreaseSeatCount(ref NearbySeatsCounts, i, j, -1, 0);
                            IncreaseSeatCount(ref NearbySeatsCounts, i, j, -1, 1);
                            IncreaseSeatCount(ref NearbySeatsCounts, i, j, 0, -1);
                            IncreaseSeatCount(ref NearbySeatsCounts, i, j, 0, 1);
                            IncreaseSeatCount(ref NearbySeatsCounts, i, j, 1, -1);
                            IncreaseSeatCount(ref NearbySeatsCounts, i, j, 1, 0);
                            IncreaseSeatCount(ref NearbySeatsCounts, i, j, 1, 1);
                        }
                    }
                }

                for (int i = 0; i < PartOneSeats.Count(); i++)
                {
                    NextSeats[i] = new Dictionary<int, char>();

                    for (int j = 0; j < PartOneSeats[i].Count; j++)
                    {
                        if (PartOneSeats[i][j] == '#'
                            && NearbySeatsCounts[i].ContainsKey(j)
                            && NearbySeatsCounts[i][j] > 3)
                        {
                            Changes = true;
                            NextSeats[i].Add(j, 'L');
                            continue;
                        }

                        if (PartOneSeats[i][j] == 'L'
                                  && (
                                      !NearbySeatsCounts[i].ContainsKey(j)
                                      || (
                                          NearbySeatsCounts[i].ContainsKey(j)
                                          && NearbySeatsCounts[i][j] == 0
                                      )
                                  )
                              )
                        {
                            Changes = true;
                            NextSeats[i].Add(j, '#');
                            continue;
                        }

                        NextSeats[i].Add(j, PartOneSeats[i][j]);

                    }
                }

                PartOneSeats = NextSeats;
            }


            PartOneCount = PartOneSeats.Sum(x => x.Value.Sum(y => y.Value == '#' ? 1 : 0));

            // Part two
            var PartTwoSeats = Seats.Select(x => x).ToDictionary(x => x.Key, x => x.Value);
            NextSeats = new Dictionary<int, Dictionary<int, char>>();
            Changes = true;
            while (Changes)
            {
                Changes = false;

                var NearbyVisibleSeatsCounts = new Dictionary<int, Dictionary<int, int>>();

                NextSeats = new Dictionary<int, Dictionary<int, char>>();

                for (int i = 0; i < PartTwoSeats.Count(); i++)
                {

                    for (int j = 0; j < PartTwoSeats[i].Count; j++)
                    {
                        if (PartTwoSeats[i][j] == '#')
                        {
                            IncreaseVisibleSeatCount(ref NearbyVisibleSeatsCounts, PartTwoSeats, i, j, -1, -1, MaxRows, MaxCols);
                            IncreaseVisibleSeatCount(ref NearbyVisibleSeatsCounts, PartTwoSeats, i, j, -1, 0, MaxRows, MaxCols);
                            IncreaseVisibleSeatCount(ref NearbyVisibleSeatsCounts, PartTwoSeats, i, j, -1, 1, MaxRows, MaxCols);
                            IncreaseVisibleSeatCount(ref NearbyVisibleSeatsCounts, PartTwoSeats, i, j, 0, -1, MaxRows, MaxCols);
                            IncreaseVisibleSeatCount(ref NearbyVisibleSeatsCounts, PartTwoSeats, i, j, 0, 1, MaxRows, MaxCols);
                            IncreaseVisibleSeatCount(ref NearbyVisibleSeatsCounts, PartTwoSeats, i, j, 1, -1, MaxRows, MaxCols);
                            IncreaseVisibleSeatCount(ref NearbyVisibleSeatsCounts, PartTwoSeats, i, j, 1, 0, MaxRows, MaxCols);
                            IncreaseVisibleSeatCount(ref NearbyVisibleSeatsCounts, PartTwoSeats, i, j, 1, 1, MaxRows, MaxCols);
                        }
                    }
                }

                for (int i = 0; i < PartTwoSeats.Count(); i++)
                {
                    NextSeats[i] = new Dictionary<int, char>();

                    for (int j = 0; j < PartTwoSeats[i].Count; j++)
                    {
                        if (PartTwoSeats[i][j] == '#'
                            && NearbyVisibleSeatsCounts.ContainsKey(i)
                            && NearbyVisibleSeatsCounts[i].ContainsKey(j)
                            && NearbyVisibleSeatsCounts[i][j] > 4)
                        {
                            Changes = true;
                            NextSeats[i].Add(j, 'L');
                            continue;
                        }

                        if (PartTwoSeats[i][j] == 'L'
                                  && (
                                      !NearbyVisibleSeatsCounts.ContainsKey(i)
                                      || !NearbyVisibleSeatsCounts[i].ContainsKey(j)
                                      || (
                                          NearbyVisibleSeatsCounts[i].ContainsKey(j)
                                          && NearbyVisibleSeatsCounts[i][j] == 0
                                      )
                                  )
                              )
                        {
                            Changes = true;
                            NextSeats[i].Add(j, '#');
                            continue;
                        }

                        NextSeats[i].Add(j, PartTwoSeats[i][j]);
                    }
                }

                PartTwoCount = PartTwoSeats.Sum(x => x.Value.Sum(y => y.Value == '#' ? 1 : 0));

                PartTwoSeats = NextSeats;
            }


            Console.WriteLine($"Part One Count: {PartOneCount}");
            Console.WriteLine($"Part Two Count: {PartTwoCount}");
        }

        private static void IncreaseSeatCount(ref Dictionary<int, Dictionary<int, int>> nearbySeatsCounts,
                                                int i, int j,
                                                int directionI, int directionJ)
        {
            nearbySeatsCounts[i + directionI][j + directionJ] = nearbySeatsCounts[i + directionI].ContainsKey(j + directionJ) ?
                                                                    ++nearbySeatsCounts[i + directionI][j + directionJ]
                                                                    : 1;

        }

        private static void IncreaseVisibleSeatCount(ref Dictionary<int, Dictionary<int, int>> nearbySeatsCounts,
                                                        Dictionary<int, Dictionary<int, char>> seats,
                                                        int i, int j,
                                                        int directionI, int directionJ,
                                                        int maxRows, int maxCols)
        {
            var FoundSeat = false;
            var FoundEdge = false;

            while (!FoundSeat && !FoundEdge)
            {
                i += directionI;
                j += directionJ;

                FoundEdge = (i < 0) || (j < 0) || (i >= maxRows) || (j >= maxCols);
                if (!FoundEdge)
                {
                    FoundSeat = seats[i][j] != '.';
                }
            }
            if (FoundSeat)
            {
                if (!nearbySeatsCounts.ContainsKey(i))
                {
                    nearbySeatsCounts[i] = new Dictionary<int, int>();
                }
                nearbySeatsCounts[i][j] = nearbySeatsCounts[i].ContainsKey(j) ?
                                                                       ++nearbySeatsCounts[i][j]
                                                                       : 1;
            }

        }
    }
}