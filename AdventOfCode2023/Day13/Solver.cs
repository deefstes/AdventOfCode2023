namespace AdventOfCode2023.Day13
{
    using AdventOfCode2023.Utils;
    using System;
    using System.Text;

    public class Solver : ISolver
    {
        public string Part1(string input)
        {
            var maps = input.Split("\r\n\r\n");

            var total = 0;
            foreach (var map in maps)
            {
                total += FindVerticalMirror(map);
                total += 100 * FindHorizontalMirror(map);
            }

            return total.ToString();
        }

        public string Part2(string input)
        {
            var maps = input.Split("\r\n\r\n");

            var total = 0;
            foreach (var map in maps)
            {
                total += FindVerticalMirror(map, true);
                total += 100 * FindHorizontalMirror(map, true);
            }

            return total.ToString();
        }

        private static int FindVerticalMirror(string map, bool withSmudge = false)
        {
            var grid = map.AsGrid();

            for (int x = 0; x < grid.GetLength(0)-1; x++)
            {
                StringBuilder before = new();
                StringBuilder after = new();
                for (int dist = 0; dist <= Math.Min(x, grid.GetLength(0) - x - 2); dist++)
                {
                    before.Append(grid.ColToString(x - dist));
                    after.Append(grid.ColToString(x + dist + 1));
                }
                if (withSmudge)
                {
                    if (Utils.Levenshtein(before.ToString(), after.ToString()) == 1)
                        return x + 1;
                }
                else
                {
                    if (before.ToString()==after.ToString())
                        return x + 1;
                }
            }

            return 0;
        }

        private static int FindHorizontalMirror(string map, bool withSmudge = false)
        {
            var grid = map.AsGrid();

            for (int y = 0; y < grid.GetLength(1)-1; y++)
            {
                StringBuilder before = new();
                StringBuilder after = new();
                for (int dist = 0; dist <= Math.Min(y, grid.GetLength(1) - y - 2); dist++)
                {
                    before.Append(grid.RowToString(y - dist));
                    after.Append(grid.RowToString(y + dist + 1));
                }
                if (withSmudge)
                {
                    if (Utils.Levenshtein(before.ToString(), after.ToString()) == 1)
                        return y + 1;
                }
                else
                {
                    if (before.ToString() == after.ToString())
                        return y + 1;
                }
            }

            return 0;
        }
    }
}
