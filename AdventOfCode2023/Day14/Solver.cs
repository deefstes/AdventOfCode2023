namespace AdventOfCode2023.Day14
{
    using AdventOfCode2023.Utils;
    using AdventOfCode2023.Utils.Graph;
    using System;

    public class Solver : ISolver
    {
        public string Part1(string input)
        {
            var grid = input.AsGrid();
            grid = Tilt(grid, Direction.North);
            var load = CalcLoad(grid, Direction.North);

            return load.ToString();
        }

        public string Part2(string input)
        {
            List<int> loadHistory = [];
            var repeatingLen = 0L;

            var grid = input.AsGrid();
            var reps = 1000000000;
            for (int i = 0; i < reps; i++)
            {
                for (int dir = 0; dir < 4; dir++)
                {
                    grid = Tilt(grid, Direction.North);
                    grid = RotateCCW(grid);
                }

                loadHistory.Add(CalcLoad(grid, Direction.North));
                repeatingLen = CalcRepeatingLen(loadHistory);
                if (repeatingLen != 0)
                    break;
            }

            for (var i = 0; i < (reps - loadHistory.Count) % repeatingLen; i++)
            {
                for (int dir = 0; dir < 4; dir++)
                {
                    grid = Tilt(grid, Direction.North);
                    grid = RotateCCW(grid);
                }
            }

            return CalcLoad(grid, Direction.North).ToString();
        }

        private static char[,] Tilt(char[,] grid, Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    for (int x = 0; x < grid.GetLength(0); x++)
                    {
                        for (int y = 0; y < grid.GetLength(1); y++)
                        {
                            if (grid[x, y] == '.')
                            {
                                // Find next round rock down
                                for (int yy = y + 1; yy < grid.GetLength(1); yy++)
                                {
                                    if (grid[x, yy] == 'O')
                                    {
                                        grid[x, y] = 'O';
                                        grid[x, yy] = '.';
                                        break;
                                    }
                                    else if (grid[x, yy] == '#')
                                        break;
                                }
                            }
                        }
                    }
                    break;
                case Direction.South:
                    for (int x = 0; x < grid.GetLength(0); x++)
                    {
                        for (int y = grid.GetLength(1) - 1; y >= 0; y--)
                        {
                            if (grid[x, y] == '.')
                            {
                                // Find next round rock up
                                for (int yy = y - 1; yy >= 0; yy--)
                                {
                                    if (grid[x, yy] == 'O')
                                    {
                                        grid[x, y] = 'O';
                                        grid[x, yy] = '.';
                                        break;
                                    }
                                    else if (grid[x, yy] == '#')
                                        break;
                                }
                            }
                        }
                    }
                    break;
                case Direction.West:
                    for (int y = 0; y < grid.GetLength(1); y++)
                    {
                        for (int x = 0; x < grid.GetLength(0); x++)
                        {
                            if (grid[x, y] == '.')
                            {
                                // Find next round rock right
                                for (int xx = x + 1; xx < grid.GetLength(0); xx++)
                                {
                                    if (grid[xx, y] == 'O')
                                    {
                                        grid[x, y] = 'O';
                                        grid[xx, y] = '.';
                                        break;
                                    }
                                    else if (grid[xx, y] == '#')
                                        break;
                                }
                            }
                        }
                    }
                    break;
                case Direction.East:
                    for (int y = 0; y < grid.GetLength(1); y++)
                    {
                        for (int x = grid.GetLength(0) - 1; x >= 0; x--)
                        {
                            if (grid[x, y] == '.')
                            {
                                // Find next round rock right
                                for (int xx = x - 1; xx >= 0; xx--)
                                {
                                    if (grid[xx, y] == 'O')
                                    {
                                        grid[x, y] = 'O';
                                        grid[xx, y] = '.';
                                        break;
                                    }
                                    else if (grid[xx, y] == '#')
                                        break;
                                }
                            }
                        }
                    }
                    break;
                default:
                    throw new Exception("Direction not yet implemented");
            }

            return grid;
        }

        private static int CalcLoad(char[,] grid, Direction direction)
        {
            var load = 0;
            switch (direction)
            {
                case Direction.North:
                    for (int x = 0; x < grid.GetLength(0); x++)
                        for (int y = 0; y < grid.GetLength(1); y++)
                            if (grid[x, y] == 'O')
                                load += grid.GetLength(1) - y;
                    break;
                default:
                    throw new Exception("Direction not yet implemented");
            }
            return load;
        }

        private static char[,] RotateCCW(char[,] grid)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            char[,] rotatedGrid = new char[cols, rows];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    rotatedGrid[cols - 1 - j, i] = grid[i, j];
                }
            }

            return rotatedGrid;
        }

        static int CalcRepeatingLen(List<int> history)
        {
            int maxLen = history.Count / 4;

            for (int repeatingLen = 1; repeatingLen <= maxLen; repeatingLen++)
            {
                bool repeats = true;

                for (int i = 0; i < repeatingLen; i++)
                {
                    bool valid = true;

                    for (int j = 1; j <= 3; j++)
                    {
                        if (history[history.Count - 1 - i] != history[history.Count - 1 - i - repeatingLen * j])
                        {
                            valid = false;
                            break;
                        }
                    }

                    if (!valid)
                    {
                        repeats = false;
                        break;
                    }
                }

                if (repeats)
                    return repeatingLen;
            }

            return 0;
        }
    }
}
