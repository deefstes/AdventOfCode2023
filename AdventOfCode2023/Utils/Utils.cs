using System.Diagnostics;
using System.Text;

namespace AdventOfCode2023.Utils
{
    public static class Utils
    {
        public static (T, TimeSpan) MeasureExecutionTime<T>(Func<T> function)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            T result = function();
            stopwatch.Stop();

            return (result, stopwatch.Elapsed);
        }

        public static string Format(this TimeSpan timeSpan)
        {
            if ((int)timeSpan.TotalHours > 0)
                return $"{(int)timeSpan.TotalHours}h " +
                       $"{timeSpan.Minutes}m " +
                       $"{timeSpan.Seconds}.{timeSpan.Milliseconds:D3}s";

            if ((int)timeSpan.TotalMinutes > 0)
                return $"{timeSpan.Minutes}m " +
                       $"{timeSpan.Seconds}.{timeSpan.Milliseconds:D3}s";

            if ((int)timeSpan.TotalSeconds > 0)
                return $"{timeSpan.Seconds}.{timeSpan.Milliseconds:D3}s";

            if ((int)timeSpan.TotalMilliseconds > 0)
                return $"{timeSpan.Milliseconds}.{timeSpan.Microseconds:D3}ms";

            return $"{timeSpan.Microseconds}μs";
        }

        /// <summary>
        /// Convert a string containing newlines to IList<string>, dropping all leading and trailing whitespace in the list and also in each individual string in the list
        /// </summary>
        /// <param name="input">string containing newlines</param>
        /// <returns>IList<string></returns>
        public static IList<string> AsList(this string input)
        {
            return input
                .Trim()
                .Replace("\r", "")
                .Split('\n')
                .ToList()
                .Select(s => s.Trim())
                .ToList();
        }

        public static char[,] AsGrid(this string input)
        {
            var lines = input.AsList();
            var height = lines.Count;
            var width = lines.Max(s => s.Length);

            var grid = new char[width, height];

            for (int y=0; y<height; y++)
            {
                for (int x=0; x<width; x++)
                {
                    grid[x, y] = lines[y][x];
                }
            }
            
            return grid;
        }

        public static T Gcd<T>(T a, T b)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a));
            if (b == null)
                throw new ArgumentNullException(nameof(b));

            dynamic x = a;
            dynamic y = b;

            if (y == 0)
                return Math.Abs(x);
            else
                return Gcd(y, x % y);
        }

        public static T Lcm<T>(T[] values)
        {
            return values.Aggregate((a, b) => {
                if (a == null)
                    throw new ArgumentNullException(nameof(a));
                if (b == null)
                    throw new ArgumentNullException(nameof(b));

                dynamic x = a;
                dynamic y = b;
                return Math.Abs(x * y / Gcd(x, y));
            });
        }

        public static string GridToString(char[,] grid)
        {
            StringBuilder sb = new();
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    sb.Append(grid[i, j]);
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public static List<(T,T)> Combinations<T>(List<T> input)
        {
            List<(T, T)> output = [];

            for (int i = 0; i< input.Count;i++)
            {
                for (int j = i+1;j<input.Count;j++)
                    output.Add((input[j], input[i]));
            }

            return output;
        }
    }
}
