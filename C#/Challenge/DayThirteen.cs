using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AoC2020
{
    public static class DayThirteen
    {
        public static void Run()
        {
            var File = new StreamReader(Path.GetFullPath("Resource/DayThirteen.txt"));

            var PartOneCount = 0L;
            var PartTwoCount = 0L;

            var CurrentTime = int.Parse(File.ReadLine());
            var BusTimes = File.ReadLine().Split(',')
                                          .Select((Input, Index) =>
                                          {
                                              var Time = Input == "x" ? -1 : long.Parse(Input);
                                              var LongIndex = (long)Index;
                                              return new { Time, LongIndex };
                                          })
                                          .Where(x => x.Time != -1);
            File.Close();

            var NextBusId = 0L;
            var NextBusTime = long.MaxValue;

            foreach (var Bus in BusTimes)
            {
                Math.DivRem(CurrentTime, Bus.Time, out var TimeToBus);

                TimeToBus = Bus.Time - TimeToBus;

                if (TimeToBus < NextBusTime)
                {
                    NextBusTime = TimeToBus;
                    NextBusId = Bus.Time;
                }
            }

            PartOneCount = NextBusTime * NextBusId;

            var BusTimeMultiple = BusTimes.Select(x => x.Time).ToList();
            var BusTimeRemainder = BusTimes.Select(x => x.Time - x.LongIndex).ToList();

            PartTwoCount = ChineseRemainderTheorem(BusTimeMultiple, BusTimeRemainder);

            Console.WriteLine($"Part One Count: {PartOneCount}");
            Console.WriteLine($"Part Two Count: {PartTwoCount}");
        }

        //https://en.wikipedia.org/wiki/Chinese_remainder_theorem
        private static long ChineseRemainderTheorem(List<long> n, List<long> a)
        {
            var prod = n.Aggregate(1L, (i, j) => i * j);
            var sm = 0L;
            for (int i = 0; i < n.Count; i++)
            {
                var p = prod / n[i];
                sm += a[i] * ModularMultiplicativeInverse(p, n[i]) * p;
            }
            return sm % prod;
        }

        private static long ModularMultiplicativeInverse(long a, long mod)
        {
            var b = a % mod;
            for (int x = 1; x < mod; x++)
            {
                if ((b * x) % mod == 1)
                {
                    return x;
                }
            }
            return 1;
        }
    }
}