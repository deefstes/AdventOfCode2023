namespace AdventOfCode2023.Day11
{
    using AdventOfCode2023.Utils;
    using AdventOfCode2023.Utils.Graph;

    public class Solver : ISolver
    {
        public string Part1(string input)
        {
            var grid = input.AsGrid();
            List<int> emptyRows = [];
            List<int> emptyCols = [];

            for (int x = 0; x < grid.GetLength(0); x++)
                if (IsEmptyColumn(grid, x))
                    emptyCols.Add(x);

            for (int y = 0; y < grid.GetLength(1); y++)
                if (IsEmptyRow(grid, y))
                    emptyRows.Add(y);

            List<Coordinates> galaxies = [];
            for (int x = 0; x < grid.GetLength(0); x++)
                for (int y = 0; y < grid.GetLength(1); y++)
                    if (grid[x,y] == '#')
                        galaxies.Add(new Coordinates(x, y));

            int total = 0;
            var combinations = Utils.Combinations(galaxies);
            foreach ((Coordinates, Coordinates) combo in combinations)
            {
                var dist = (int)combo.Item1.ManhattanDistanceTo(combo.Item2);
                var max = new Coordinates(Math.Max(combo.Item1.X, combo.Item2.X), Math.Max(combo.Item1.Y, combo.Item2.Y));
                var min = new Coordinates(Math.Min(combo.Item1.X, combo.Item2.X), Math.Min(combo.Item1.Y, combo.Item2.Y));

                var penalty = emptyCols.Count(c => c > min.X && c < max.X)
                    + emptyRows.Count(r => r > min.Y && r < max.Y);

                total += dist + penalty;
            }

            return total.ToString();
        }

        public string Part2(string input)
        {
            var grid = input.AsGrid();
            List<int> emptyRows = [];
            List<int> emptyCols = [];

            for (int x = 0; x < grid.GetLength(0); x++)
                if (IsEmptyColumn(grid, x))
                    emptyCols.Add(x);

            for (int y = 0; y < grid.GetLength(1); y++)
                if (IsEmptyRow(grid, y))
                    emptyRows.Add(y);

            List<Coordinates> galaxies = [];
            for (int x = 0; x < grid.GetLength(0); x++)
                for (int y = 0; y < grid.GetLength(1); y++)
                    if (grid[x, y] == '#')
                        galaxies.Add(new Coordinates(x, y));

            long total = 0;
            var combinations = Utils.Combinations(galaxies);
            foreach ((Coordinates, Coordinates) combo in combinations)
            {
                long dist = combo.Item1.ManhattanDistanceTo(combo.Item2);
                var max = new Coordinates(Math.Max(combo.Item1.X, combo.Item2.X), Math.Max(combo.Item1.Y, combo.Item2.Y));
                var min = new Coordinates(Math.Min(combo.Item1.X, combo.Item2.X), Math.Min(combo.Item1.Y, combo.Item2.Y));

                var colsExpanded = emptyCols.Count(c => c > min.X && c < max.X);
                var rowsExpanded = emptyRows.Count(r => r > min.Y && r < max.Y);

                long expansionCost = colsExpanded+rowsExpanded;

                total += dist + expansionCost * (1000000 - 1);
            }

            return total.ToString();
        }

        private bool IsEmptyColumn(char[,] grid, int colIndex)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[colIndex, y] != '.')
                    return false;
            }

            return true;
        }

        private bool IsEmptyRow(char[,] grid, int rowIndex)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                if (grid[x, rowIndex] != '.')
                    return false;
            }

            return true;
        }
    }
}
