using System.Diagnostics;

namespace AdventOfCode2023.Utils
{
    public static class Utils
    {
        public static (T, long) MeasureExecutionTime<T>(Func<T> function)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            T result = function();
            stopwatch.Stop();

            return (result, stopwatch.ElapsedMilliseconds);
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
    }
}
