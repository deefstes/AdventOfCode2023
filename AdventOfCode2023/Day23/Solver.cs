namespace AdventOfCode2023.Day23
{
    using AdventOfCode2023.Utils;
    using AdventOfCode2023.Utils.Graph;
    using AdventOfCode2023.Utils.Pathfinding;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Solver : ISolver
    {
        public string Part1(string input)
        {
            var inputGrid = input.AsGrid();
            var width = inputGrid.GetLength(0);
            var height = inputGrid.GetLength(1);

            //var weightedGrid = new WeightedGrid<GraphNode>(width, height);
            //var graphFull = new WeightedGraph<GraphNode>();
            var graphCompact = new WeightedGraph<GraphNode>();
            List<GraphNode> intersections = [];

            GraphNode start = new GraphNode("start");
            GraphNode finish = new GraphNode("finish");


            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    if (inputGrid[x, y] == '#')
                        continue;

                    var curCoords = new Coordinates(x, y);

                    if (y == 0 && inputGrid[x, y] == '.')
                        start = new GraphNode(curCoords.ToString(), 1, curCoords);

                    if (y == inputGrid.GetLength(1)-1 && inputGrid[x, y] == '.')
                        finish = new GraphNode(curCoords.ToString(), 1, curCoords);

                    if (curCoords.NeighboursCardinal().Where(n => n.InBounds(width, height)).Where(n => new List<char> { '.', '<', '>', '^', 'v' }.Contains(inputGrid[n.X, n.Y])).Count() > 2)
                        intersections.Add(new GraphNode(curCoords.ToString(), 1, curCoords));
                }

            intersections.Add(start);
            intersections.Add(finish);

            var directionList = new List<Direction> { Direction.North, Direction.South, Direction.East, Direction.West };
            foreach (GraphNode intersection in intersections)
            {
                foreach (Direction dir in directionList)
                {
                    var curCoords = intersection.Coords;
                    var (nextIntersection, dist) = WalkToNextInterestion(inputGrid, intersections, curCoords, dir, ExcludeUphills);
                    //var (nextIntersection, dist) = WalkToNextInterestion(inputGrid, intersections, curCoords, dir, (c,d)=>false);

                    if (nextIntersection != null)
                    {
                        var c = graphCompact.Cost(intersection, nextIntersection);
                        if (c == int.MaxValue)
                            c = 0;
                        graphCompact.AddConnection(intersection, nextIntersection, Math.Max(c, dist));
                    }
                }
            }

            //var longestPath = new LongestPath(graphFull, start, finish);

            //return longestPath.DistancesMap[start].ToString();

            Console.WriteLine(graphCompact.ToUML(g => g.Name));

            return "";
        }

        public static bool ExcludeUphills(char cell, Direction dir)
        {
            if (dir == Direction.North && cell == 'v')
                return true;
            if (dir == Direction.South && cell == '^')
                return true;
            if (dir == Direction.East && cell == '<')
                return true;
            if (dir == Direction.West && cell == '>')
                return true;

            return false;
        }

        public (GraphNode?, int) WalkToNextInterestion(char[,] grid, List<GraphNode> intersections, Coordinates curCoords, Direction dir, Func<char, Direction, bool> excludeFunc)
        {
            var directionList = new List<Direction> { Direction.North, Direction.South, Direction.East, Direction.West };
            var walkLen = 0;

            while (true)
            {
                var nextCoords = curCoords!.Move(dir);

                // Exclude out of bounds cells 
                if (!nextCoords.InBounds(grid.GetLength(0), grid.GetLength(1)))
                    return (null, 0);

                if (excludeFunc(grid[nextCoords.X, nextCoords.Y], dir))
                    return (null, 0);

                //// Exclude uphill walks
                //if (dir == Direction.North && grid[nextCoords.X, nextCoords.Y] == 'v')
                //    return (null, 0);
                //if (dir == Direction.South && grid[nextCoords.X, nextCoords.Y] == '^')
                //    return (null, 0);
                //if (dir == Direction.East && grid[nextCoords.X, nextCoords.Y] == '<')
                //    return (null, 0);
                //if (dir == Direction.West && grid[nextCoords.X, nextCoords.Y] == '>')
                //    return (null, 0);

                // Exclude walls
                if (grid[nextCoords!.X, nextCoords!.Y] == '#')
                    return (null, 0);

                curCoords = nextCoords!;
                walkLen++;

                var foundIntersection = intersections.FirstOrDefault(n => n.Coords.Equals(curCoords));
                if (foundIntersection != null)
                    return (foundIntersection, walkLen);

                var candidateDirs = directionList.Where(d =>
                {
                    if (d == dir.TurnLeft(4))
                        return false;
                    var nextCoords = curCoords.Move(d);
                    if (!nextCoords.InBounds(grid.GetLength(0), grid.GetLength(1)))
                        return false;
                    if (grid[nextCoords.X, nextCoords.Y] == '#')
                        return false;
                    if (excludeFunc(grid[nextCoords.X, nextCoords.Y], d))
                        return false;
                    return true;
                });

                if (candidateDirs.Count() != 1)
                    return (null, 0);

                dir = candidateDirs.First();
            }
        }

        public string Part2(string input)
        {
            return "Not yet implemented";
        }
    }
}
