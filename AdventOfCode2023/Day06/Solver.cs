using AdventOfCode2023.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Day06
{
    public class Solver : ISolver
    {
        public string Part1(string input)
        {
            var lines = input.AsList();

            var times = lines[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            var distances = lines[1].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();


            long result = 1;
            for (int raceIndex = 0; raceIndex < times.Count; raceIndex++)
            {
                result *= ClosedForm(times[raceIndex], distances[raceIndex]);
            }

            return result.ToString();
        }

        public string Part2(string input)
        {
            var lines = input.AsList();

            var timeAvailable = long.Parse(lines[0].Split(':')[1].Replace(" ", ""));
            var distanceToBeat = long.Parse(lines[1].Split(':')[1].Replace(" ", ""));

            return ClosedForm(timeAvailable, distanceToBeat).ToString();
        }

        // Original brute force method
        public string Part1_Original(string input)
        {
            var lines = input.AsList();

            var times = lines[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            var distances = lines[1].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();


            var result = 1;
            for (int raceIndex = 0; raceIndex < times.Count; raceIndex++)
            {
                var timeAvailable = times[raceIndex];
                var distanceToBeat = distances[raceIndex];

                List<(long, long)> races = [];

                for (var chargeTime = 0; chargeTime < timeAvailable; chargeTime++)
                {
                    races.Add((chargeTime, CalcDistance(timeAvailable, chargeTime)));
                }

                var waysToBeat = races.Where(r => r.Item2 > distanceToBeat).Count();
                result *= waysToBeat;
            }

            return result.ToString();
        }

        // Original brute force method
        public string Part2_Original(string input)
        {
            var lines = input.AsList();

            var timeAvailable = long.Parse(lines[0].Split(':')[1].Replace(" ", ""));
            var distanceToBeat = long.Parse(lines[1].Split(':')[1].Replace(" ", ""));

            List<(long, long)> races = [];

            for (var chargeTime = 0; chargeTime < timeAvailable; chargeTime++)
            {
                races.Add((chargeTime, CalcDistance(timeAvailable, chargeTime)));
            }

            var waysToBeat = races.Where(r => r.Item2 > distanceToBeat).Count();

            return waysToBeat.ToString();
        }

        public static long CalcDistance(long time, long chargeTime)
        {
            return (time - chargeTime) * chargeTime;
        }

        public static long ClosedForm(long time, long distance)
        {
            // This function implements a closed form mathematical solution. The formula for calculating the distance covered by a boat is as follows:
            // d = (t-c).c, which is what the CalcDistance() function uses. In it's simplified form that formula is c^2 - tc + d = 0 which can be seen
            // is a simple quadtratic formula. Solving for it will give the two values for c which achieves the same distance as the input d. All values
            // for c between that lower bound and upper bound, will provide a higher distance value which are the winning values.

            var lowerBound = (long)Math.Floor((time - Math.Sqrt(Math.Pow(time, 2) - 4 * distance)) / 2) + 1;
            var upperBound = (long)Math.Ceiling((time + Math.Sqrt(Math.Pow(time, 2) - 4 * distance)) / 2) - 1;

            return upperBound - lowerBound + 1;
        }
    }
}
