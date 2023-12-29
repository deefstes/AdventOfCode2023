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

            var graphCompact = CompactIntGraph(inputGrid, inputGrid.GetLength(0), inputGrid.GetLength(1), ExcludeUphills);
            var start = graphCompact.Nodes().Order().TakeLast(2).ToArray()[0];
            var finish = graphCompact.Nodes().Order().TakeLast(2).ToArray()[1];

            Dictionary<int, Dictionary<int, long>> doubleDictGraph = [];
            foreach (var c in graphCompact.Connections())
            {
                if (!doubleDictGraph.ContainsKey(c.Item1))
                    doubleDictGraph[c.Item1] = [];
                if (!doubleDictGraph.ContainsKey(c.Item2))
                    doubleDictGraph[c.Item2] = [];

                doubleDictGraph[c.Item1][c.Item2] = c.Item3;
            }

            return doubleDictGraph.FindLongestPath(start, finish).ToString();
        }

        public string Part2(string input)
        {
            var inputGrid = input.AsGrid();

            var graphCompact = CompactIntGraph(inputGrid, inputGrid.GetLength(0), inputGrid.GetLength(1), (c,d) => false);
            var start = graphCompact.Nodes().Order().TakeLast(2).ToArray()[0];
            var finish = graphCompact.Nodes().Order().TakeLast(2).ToArray()[1];

            Dictionary<int, Dictionary<int, long>> doubleDictGraph = [];
            foreach (var c in graphCompact.Connections())
            {
                if (!doubleDictGraph.ContainsKey(c.Item1))
                    doubleDictGraph[c.Item1] = [];

                doubleDictGraph[c.Item1][c.Item2] = c.Item3;
            }

            return doubleDictGraph.FindLongestPath(start, finish).ToString();
        }

        private WeightedGraph<GraphNode> CompactGraph(char[,] inputGrid, int width, int height, Func<char, Direction, bool> excludeFunc)
        {
            var graphCompact = new WeightedGraph<GraphNode>();
            List<GraphNode> intersections = [];

            var start = new GraphNode("start");
            var finish = new GraphNode("finish");

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    if (inputGrid[x, y] == '#')
                        continue;

                    var curCoords = new Coordinates(x, y);

                    if (y == 0 && inputGrid[x, y] == '.')
                        start = new GraphNode(curCoords.ToString(), 1, curCoords);

                    if (y == inputGrid.GetLength(1) - 1 && inputGrid[x, y] == '.')
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
                    var (nextIntersection, dist) = WalkToNextInterestion(inputGrid, intersections, curCoords!, dir, excludeFunc);

                    if (nextIntersection != null)
                    {
                        var c = graphCompact.Cost(intersection, nextIntersection);
                        if (c == int.MaxValue)
                            c = 0;
                        graphCompact.AddConnection(intersection, nextIntersection, Math.Max(c, dist));
                    }
                }
            }

            return graphCompact;
        }

        private WeightedGraph<int> CompactIntGraph(char[,] inputGrid, int width, int height, Func<char, Direction, bool> excludeFunc)
        {
            var graphCompact = new WeightedGraph<int>();
            List<GraphNode> intersections = [];

            var start = new GraphNode("start");
            var finish = new GraphNode("finish");

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    if (inputGrid[x, y] == '#')
                        continue;

                    var curCoords = new Coordinates(x, y);

                    if (y == 0 && inputGrid[x, y] == '.')
                        start = new GraphNode(curCoords.ToString(), 1, curCoords);

                    if (y == inputGrid.GetLength(1) - 1 && inputGrid[x, y] == '.')
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
                    var (nextIntersection, dist) = WalkToNextInterestion(inputGrid, intersections, curCoords!, dir, excludeFunc);

                    if (nextIntersection != null)
                    {
                        var c = graphCompact.Cost(intersections.IndexOf(intersection), intersections.IndexOf(nextIntersection));
                        if (c == int.MaxValue)
                            c = 0;
                        graphCompact.AddConnection(intersections.IndexOf(intersection), intersections.IndexOf(nextIntersection), Math.Max(c, dist));
                    }
                }
            }

            return graphCompact;
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

        int xFindLongestPath(int start, int end, Dictionary<int, Dictionary<int, int>> edges)
        {
            return Calc(start, 1 << start);

            int Calc(int cur, int used)
            {
                if (cur == end)
                    return 0;

                var res = int.MinValue;
                foreach (var (next, dist) in edges[cur])
                {
                    if ((used & (1L << next)) == 0)
                        res = Math.Max(res, Calc(next, used | (1 << next)) + dist);
                }

                return res;
            }
        }
    }
}
