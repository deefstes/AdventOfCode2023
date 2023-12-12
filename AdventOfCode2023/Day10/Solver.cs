namespace AdventOfCode2023.Day10
{
    using AdventOfCode2023.Utils;
    using AdventOfCode2023.Utils.Graph;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Solver : ISolver
    {
        public string Part1(string input)
        {
            var grid = input.AsGrid();
            var loop = GetLoop(grid);

            return (loop.Count/2).ToString();
        }

        public string Part2(string input)
        {
            var grid = input.AsGrid();
            var loop = GetLoop(grid);
            var enclosed = GetEnclosed(grid, loop);

            return enclosed.Count.ToString();
        }

        private static Coordinates FindStartingPoint(char[,] grid)
        {
            for (int x=0; x<grid.GetLength(0); x++)
            {
                for (int y=0; y<grid.GetLength(1); y++)
                {
                    if (grid[x, y] == 'S')
                        return new Coordinates(x, y);
                }
            }

            return new Coordinates(0,0);
        }

        private static readonly char[] northTiles = ['F', '|', '7'];
        private static readonly char[] eastTiles = ['-', '7', 'J'];
        private static readonly char[] southTiles = ['|', 'J', 'L'];

        private List<Coordinates> GetLoop(char[,] grid)
        {
            var startCell = FindStartingPoint(grid);
            var nextCell = startCell;
            var currentDir = Direction.North;
            List<Coordinates> loop = [];
            loop.Add(startCell);

            do
            {
                switch (grid[nextCell.X, nextCell.Y])
                {
                    case 'S':
                        foreach (Direction dir in new[] { Direction.North, Direction.East, Direction.South })
                        {
                            var testCell = nextCell.Move(dir);
                            if (!testCell.InBounds(grid.GetLength(0), grid.GetLength(1)))
                                continue;

                            if (dir == Direction.North && northTiles.Contains(grid[testCell.X, testCell.Y]))
                            {
                                currentDir = dir;
                                nextCell = testCell;
                                break;
                            }
                            if (dir == Direction.East && eastTiles.Contains(grid[testCell.X, testCell.Y]))
                            {
                                currentDir = dir;
                                nextCell = testCell;
                                break;
                            }
                            if (dir == Direction.South && southTiles.Contains(grid[testCell.X, testCell.Y]))
                            {
                                currentDir = dir;
                                nextCell = testCell;
                                break;
                            }
                        }
                        break;
                    case '|':
                        switch (currentDir)
                        {
                            case Direction.North:
                                nextCell = nextCell.Move(Direction.North);
                                currentDir = Direction.North;
                                break;
                            case Direction.South:
                                nextCell = nextCell.Move(Direction.South);
                                currentDir = Direction.South;
                                break;
                        }
                        break;
                    case '-':
                        switch (currentDir)
                        {
                            case Direction.East:
                                nextCell = nextCell.Move(Direction.East);
                                currentDir = Direction.East;
                                break;
                            case Direction.West:
                                nextCell = nextCell.Move(Direction.West);
                                currentDir = Direction.West;
                                break;
                        }
                        break;
                    case 'L':
                        switch (currentDir)
                        {
                            case Direction.South:
                                nextCell = nextCell.Move(Direction.East);
                                currentDir = Direction.East;
                                break;
                            case Direction.West:
                                nextCell = nextCell.Move(Direction.North);
                                currentDir = Direction.North;
                                break;
                        }
                        break;
                    case 'J':
                        switch (currentDir)
                        {
                            case Direction.South:
                                nextCell = nextCell.Move(Direction.West);
                                currentDir = Direction.West;
                                break;
                            case Direction.East:
                                nextCell = nextCell.Move(Direction.North);
                                currentDir = Direction.North;
                                break;
                        }
                        break;
                    case '7':
                        switch (currentDir)
                        {
                            case Direction.East:
                                nextCell = nextCell.Move(Direction.South);
                                currentDir = Direction.South;
                                break;
                            case Direction.North:
                                nextCell = nextCell.Move(Direction.West);
                                currentDir = Direction.West;
                                break;
                        }
                        break;
                    case 'F':
                        switch (currentDir)
                        {
                            case Direction.West:
                                nextCell = nextCell.Move(Direction.South);
                                currentDir = Direction.South;
                                break;
                            case Direction.North:
                                nextCell = nextCell.Move(Direction.East);
                                currentDir = Direction.East;
                                break;
                        }
                        break;
                }

                if (!nextCell.Equals(startCell))
                    loop.Add(nextCell);
            } while (!nextCell.Equals(startCell));

            return loop;
        }

        /// <summary>
        /// This function starts by walking along the established path and adding every grid cell to the left of the
        /// path (which does not fall on the path itself) to one List of cells and every grid cell to the right of the
        /// path to another List of cells. It also maintains a List of all the cells in the grid that have not been
        /// checked yet (not on the path itself and not in either of the two Lists just mentioned). It then iterates
        /// over this List of unchecked cells. If a cell a adjacent to any cell in either of these two pools, the cell
        /// is added to the pool and removed from the unchecked list. It continues to do this until no further cells
        /// are found to be adjacent to either of the pools. If at any point it is discovered that a cell is adjacent
        /// to either of the pools and is also adjacent to the border of the grid, it is obvious that said pool is not
        /// enclosed inside the path and so no further checks will be performed against that pool, assuming the other
        /// one must be the enclosed pool.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private List<Coordinates> GetEnclosed(char[,] grid, List<Coordinates> path)
        {
            List<Coordinates> toBeChecked = []; // List of coordinates that haven't been checked yet
            List<Coordinates> poolLeft = []; // List of coordinates forming a possible inclusion on left of path
            List<Coordinates> poolRight = []; // List of coordinates forming a possible inclusion on right of path
            bool leftPossible = true; // Whether poolLeft is a viable inclusion
            bool rightPossible = true; // Whether poolRight is a viable inclusion
            //Direction testDirLeft = Direction.North;
            //Direction testDirRight = Direction.North;

            for (int x = 0; x < grid.GetLength(0); x++)
                for (int y = 0; y < grid.GetLength(1); y++)
                    if (!path.Contains(new Coordinates(x, y)))
                        toBeChecked.Add(new Coordinates(x, y));

            for (int i = 0; i < path.Count - 1; i++)
            {
                var dirToNext = path[i].DirectionTo(path[i + 1]);

                //switch (dirToNext)
                //{
                //    case Direction.North:
                //        testDirLeft = Direction.West;
                //        testDirRight = Direction.East;
                //        break;
                //    case Direction.East:
                //        testDirLeft = Direction.North;
                //        testDirRight = Direction.South;
                //        break;
                //    case Direction.South:
                //        testDirLeft = Direction.East;
                //        testDirRight = Direction.West;
                //        break;
                //    case Direction.West:
                //        testDirLeft = Direction.South;
                //        testDirRight = Direction.North;
                //        break;
                //}

                List<Coordinates> testCellsLeft = [];
                List<Coordinates> testCellsRight = [];

                switch (grid[path[i].X, path[i].Y])
                    {
                        case '|':
                        switch (dirToNext)
                        {
                            case Direction.North:
                                testCellsLeft.Add(path[i].Move(Direction.West));
                                testCellsRight.Add(path[i].Move(Direction.East));
                                break;
                            case Direction.South:
                                testCellsLeft.Add(path[i].Move(Direction.East));
                                testCellsRight.Add(path[i].Move(Direction.West));
                                break;
                        }
                        break;
                    case '_':
                        switch (dirToNext)
                        {
                            case Direction.East:
                                testCellsLeft.Add(path[i].Move(Direction.North));
                                testCellsRight.Add(path[i].Move(Direction.South));
                                break;
                            case Direction.West:
                                testCellsLeft.Add(path[i].Move(Direction.South));
                                testCellsRight.Add(path[i].Move(Direction.North));
                                break;
                        }
                        break;
                    case '7':
                        switch (dirToNext)
                        {
                            case Direction.South:
                                testCellsLeft.Add(path[i].Move(Direction.North));
                                testCellsLeft.Add(path[i].Move(Direction.East));
                                break;
                            case Direction.West:
                                testCellsRight.Add(path[i].Move(Direction.East));
                                testCellsRight.Add(path[i].Move(Direction.North));
                                break;
                        }
                        break;
                    case 'F':
                        switch (dirToNext)
                        {
                            case Direction.East:
                                testCellsLeft.Add(path[i].Move(Direction.West));
                                testCellsLeft.Add(path[i].Move(Direction.North));
                                break;
                            case Direction.South:
                                testCellsRight.Add(path[i].Move(Direction.North));
                                testCellsRight.Add(path[i].Move(Direction.West));
                                break;
                        }
                        break;
                    case 'J':
                        switch (dirToNext)
                        {
                            case Direction.North:
                                testCellsRight.Add(path[i].Move(Direction.South));
                                testCellsRight.Add(path[i].Move(Direction.East));
                                break;
                            case Direction.West:
                                testCellsLeft.Add(path[i].Move(Direction.East));
                                testCellsLeft.Add(path[i].Move(Direction.South));
                                break;
                        }
                        break;
                    case 'L':
                        switch (dirToNext)
                        {
                            case Direction.North:
                                testCellsLeft.Add(path[i].Move(Direction.South));
                                testCellsLeft.Add(path[i].Move(Direction.West));
                                break;
                            case Direction.East:
                                testCellsRight.Add(path[i].Move(Direction.West));
                                testCellsRight.Add(path[i].Move(Direction.South));
                                break;
                        }
                        break;
                }

                if (leftPossible)
                    foreach (Coordinates testCell in testCellsLeft)
                    {
                        if (!testCell.InBounds(grid.GetLength(0), grid.GetLength(1)))
                            leftPossible = false;
                        else if (path.Contains(testCell))
                        {
                            toBeChecked.Remove(testCell);
                        }
                        else if (!poolLeft.Contains(testCell))
                        {
                            poolLeft.Add(testCell);
                            toBeChecked.Remove(testCell);
                        }
                    }

                if (rightPossible)
                    foreach (Coordinates testCell in testCellsRight)
                    {
                        if (!testCell.InBounds(grid.GetLength(0), grid.GetLength(1)))
                            rightPossible = false;
                        else if (path.Contains(testCell))
                        {
                            toBeChecked.Remove(testCell);
                        }
                        else if (!poolRight.Contains(testCell))
                        {
                            poolRight.Add(testCell);
                            toBeChecked.Remove(testCell);
                        }
                    }
            }

            // Check unchecked cells until no further additions are made
            var changesMade = 1;
            while (
                toBeChecked.Count > 0 &&
                changesMade > 0
                )
            {
                changesMade = 0;

                // Iterate over toBeChecked in reverse because we may be removing items from the list
                for (int i = toBeChecked.Count - 1; i >= 0; i--)
                {
                    if (leftPossible)
                    {
                        if (poolLeft.Contains(toBeChecked[i].Move(Direction.North)) ||
                            poolLeft.Contains(toBeChecked[i].Move(Direction.East)) ||
                            poolLeft.Contains(toBeChecked[i].Move(Direction.South)) ||
                            poolLeft.Contains(toBeChecked[i].Move(Direction.West))
                            )
                            {
                            if (toBeChecked[i].TouchesBorder(grid.GetLength(0), grid.GetLength(1)))
                            {
                                leftPossible = false;
                                toBeChecked.RemoveAt(i);
                                changesMade++;
                                continue;
                            }
                            else
                            {
                                poolLeft.Add(toBeChecked[i]);
                                toBeChecked.RemoveAt(i);
                                changesMade++;
                                continue;
                            }
                        }
                    }

                    if (rightPossible)
                    {
                        if (poolRight.Contains(toBeChecked[i].Move(Direction.North)) ||
                            poolRight.Contains(toBeChecked[i].Move(Direction.East)) ||
                            poolRight.Contains(toBeChecked[i].Move(Direction.South)) ||
                            poolRight.Contains(toBeChecked[i].Move(Direction.West))
                            )
                        {
                            if (toBeChecked[i].TouchesBorder(grid.GetLength(0), grid.GetLength(1)))
                            {
                                rightPossible = false;
                                toBeChecked.RemoveAt(i);
                                changesMade++;
                                continue;
                            }
                            else
                            {
                                poolRight.Add(toBeChecked[i]);
                                toBeChecked.RemoveAt(i);
                                changesMade++;
                                continue;
                            }
                        }
                    }
                }
            }

            return leftPossible ? poolLeft : poolRight;
        }

        private enum Side
        {
            left,
            right
        }
    }
}
