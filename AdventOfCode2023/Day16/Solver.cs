namespace AdventOfCode2023.Day16
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
            var cave = input.AsGrid();
            var energisedCells = TraceBeam(cave, new(0, 0), Direction.East, []);
            energisedCells = energisedCells.Distinct().ToList();

            return energisedCells.Count().ToString();
        }

        /// <summary>
        /// This is a very poorly optimised function. It simply performs the same operation that Part 1 performed but repeats
        /// it for each possible starting position. A simple optimisation which would probably improve this function's
        /// performance by orders of magnitude would be to maintain a memoization cache for each cell/direction pair which
        /// can be reused for subsequent checks as the values for internal cells will never change and needs to be calculated
        /// only once. The TraceBeam function already makes use of a cache to check for cycles. This cache could be extended
        /// to include a distance for each cell/direction pair in the map. If I were less lazy, I'd do that now. But the sun
        /// is shining outside and I'm actually on holiday.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Part2(string input)
        {
            var cave = input.AsGrid();
            List<(Coordinates, Direction)> startingCells = [];
            for (int i = 0; i < cave.GetLength(0); i++)
            {
                startingCells.Add((new(i, 0), Direction.South));
                startingCells.Add((new(i, cave.GetLength(1) - 1), Direction.North));
            }
            for (int i = 0; i < cave.GetLength(1); i++)
            {
                startingCells.Add((new(0, i), Direction.East));
                startingCells.Add((new(cave.GetLength(1) - 1, i), Direction.West));
            }

            var maxValue = 0;
            foreach (var cell in startingCells)
            {
                var energisedCells = TraceBeam(cave, cell.Item1, cell.Item2, []).Distinct().ToList();
                maxValue = Math.Max(maxValue, energisedCells.Count);
            }

            return maxValue.ToString();
        }

        private static List<Coordinates> TraceBeam(char[,] grid, Coordinates position, Direction direction, HashSet<(Coordinates, Direction)> cache)
        {
            List<Coordinates> cells = [];
            var branch = false;

            while (!branch)
            {
                if (!position.InBounds(grid.GetLength(0), grid.GetLength(1)))
                    return cells;

                cells.Add(position);

                if (cache.Contains((position, direction)))
                    return cells;

                cache.Add((position, direction));

                switch (grid[position.X, position.Y])
                {
                    case '.':
                        position = position.Move(direction);
                        break;
                    case '/':
                        switch (direction)
                        {
                            case Direction.North: direction = Direction.East; break;
                            case Direction.East: direction = Direction.North; break;
                            case Direction.South: direction = Direction.West; break;
                            case Direction.West: direction = Direction.South; break;
                        }
                        branch = true;
                        return cells.Union(TraceBeam(grid, position.Move(direction), direction, cache)).ToList();
                    case '\\':
                        switch (direction)
                        {
                            case Direction.North: direction = Direction.West; break;
                            case Direction.East: direction = Direction.South; break;
                            case Direction.South: direction = Direction.East; break;
                            case Direction.West: direction = Direction.North; break;
                        }
                        branch = true;
                        return cells.Union(TraceBeam(grid, position.Move(direction), direction, cache)).ToList();
                    case '|':
                        switch (direction)
                        {
                            case Direction.North:
                            case Direction.South:
                                position = position.Move(direction);
                                break;
                            case Direction.East:
                            case Direction.West:
                                branch = true;
                                return cells
                                    .Union(TraceBeam(grid, position.Move(Direction.North), Direction.North, cache))
                                    .Union(TraceBeam(grid, position.Move(Direction.South), Direction.South, cache)).ToList();
                        }
                        break;
                    case '-':
                        switch (direction)
                        {
                            case Direction.East:
                            case Direction.West:
                                position = position.Move(direction);
                                break;
                            case Direction.North:
                            case Direction.South:
                                branch = true;
                                return cells
                                    .Union(TraceBeam(grid, position.Move(Direction.East), Direction.East, cache))
                                    .Union(TraceBeam(grid, position.Move(Direction.West), Direction.West, cache)).ToList();
                        }
                        break;
                }
            }

            return cells;
        }

        //public int CountEnergized(Location pos, Direction dir)
        //{
        //    return BfsHelpers.Bfs(
        //            startFrom: new[] { startFrom },
        //            cur =>
        //            {
        //                if (!map.Inside(cur.pos))
        //                    return Array.Empty<(V, V)>();

        //                switch (map[cur.pos])
        //                {
        //                    case '.':
        //                    case '-' when cur.dir == V.left || cur.dir == V.right:
        //                    case '|' when cur.dir == V.up || cur.dir == V.down:
        //                        return new[] { (cur.pos + cur.dir, cur.dir) };

        //                    case '/' when cur.dir == V.right:
        //                        return new[] { (cur.pos + V.up, V.up) };

        //                    case '/' when cur.dir == V.up:
        //                        return new[] { (cur.pos + V.right, V.right) };

        //                    case '/' when cur.dir == V.down:
        //                        return new[] { (cur.pos + V.left, V.left) };

        //                    case '/' when cur.dir == V.left:
        //                        return new[] { (cur.pos + V.down, V.down) };

        //                    case '\\' when cur.dir == V.right:
        //                        return new[] { (cur.pos + V.down, V.down) };

        //                    case '\\' when cur.dir == V.up:
        //                        return new[] { (cur.pos + V.left, V.left) };

        //                    case '\\' when cur.dir == V.down:
        //                        return new[] { (cur.pos + V.right, V.right) };

        //                    case '\\' when cur.dir == V.left:
        //                        return new[] { (cur.pos + V.up, V.up) };

        //                    case '-' when cur.dir == V.up || cur.dir == V.down:
        //                        return new[]
        //                        {
        //                            (cur.pos + V.left, V.left),
        //                            (cur.pos + V.right, V.right),
        //                        };

        //                    case '|' when cur.dir == V.left || cur.dir == V.right:
        //                        return new[]
        //                        {
        //                            (cur.pos + V.up, V.up),
        //                            (cur.pos + V.down, V.down),
        //                        };

        //                    default:
        //                        throw new Exception($"Invalid state: {cur}");
        //                }
        //            }
        //        )
        //        .Select(s => s.State.pos)
        //        .Where(map.Inside)
        //        .Distinct()
        //        .Count();
        //}
    }
}
