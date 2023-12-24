namespace AdventOfCode2023.Day21
{
    using AdventOfCode2023.Utils;
    using AdventOfCode2023.Utils.Graph;
    using AdventOfCode2023.Utils.Pathfinding;
    using System.Collections.Generic;
    using System.Linq;

    public class Solver : ISolver
    {
        public string Part1(string input)
        {
            var map = input.AsGrid();
            GraphNode[,] gridNodes = new GraphNode[map.GetLength(0), map.GetLength(1)];
            Coordinates start = new(0, 0);
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    var curCoords = new Coordinates(x, y);
                    switch (map[x, y])
                    {
                        case '.':
                            gridNodes[x, y] = new(curCoords.ToString(), 1, curCoords);
                            break;
                        case '#':
                            break;
                        case 'S':
                            gridNodes[x, y] = new(curCoords.ToString(), 1, curCoords);
                            start = new(x, y);
                            break;
                    }
                }
            }
            var grid = new WeightedGrid<GraphNode>(gridNodes);

            int steps = map.GetLength(0) == 11 ? 6 : 64; // Little hack to use different end conditions for unit tests vs solution
            var positions = FindLocationsAfterSteps(grid, grid.Node(start.ToString())!, steps);

            return positions.ToString();
        }

        public string Part2(string input)
        {
            var map = input.AsGrid();
            GraphNode[,] gridNodes = new GraphNode[map.GetLength(0)*5, map.GetLength(1)*5];
            Coordinates start = new(0, 0);
            for (int y = 0; y < map.GetLength(0)*5; y++)
            {
                for (int x = 0; x < map.GetLength(1)*5; x++)
                {
                    var curCoords = new Coordinates(x, y);
                    switch (map[x % map.GetLength(0), y % map.GetLength(1)])
                    {
                        case '.':
                            gridNodes[x, y] = new(curCoords.ToString(), 1, curCoords);
                            break;
                        case '#':
                            break;
                        case 'S':
                            gridNodes[x, y] = new(curCoords.ToString(), 1, curCoords);
                            if (start.IsZero())
                                start = new(x, y);
                            break;
                    }
                }
            }
            var grid = new WeightedGrid<GraphNode>(gridNodes);

            start = start.Move(Direction.East, map.GetLength(0) * 2).Move(Direction.South, map.GetLength(0) * 2);

            List<Coordinates> coordsChecks = [];
            coordsChecks.Add(start.Move(Direction.West, map.GetLength(0) / 2));
            coordsChecks.Add(start.Move(Direction.West, map.GetLength(0) / 2 + map.GetLength(0)));
            coordsChecks.Add(start.Move(Direction.West, map.GetLength(0) / 2 + map.GetLength(0) + map.GetLength(0)));

            var positions = FindLocationsForCoords(grid, grid.Node(start.ToString())!, coordsChecks);
            var solutions = positions.Select(t => ((long)t.Item1, t.Item2)).ToList();
            var coefficients = Utils.DiscoverPolynomial(solutions);

            int steps = map.GetLength(0) == 11 ? 5000 : 26501365; // Little hack to use different end conditions for unit tests vs solution
            double total = Utils.SolvePolynomial(coefficients, steps);

            return total.ToString();
        }

        private long FindLocationsAfterSteps(WeightedGrid<GraphNode> grid, GraphNode position, int steps) => FindLocationsAfterSteps(grid, position, [steps]).First();

        private List<long> FindLocationsAfterSteps(WeightedGrid<GraphNode> grid, GraphNode position, List<int> steps)
        {
            List<long> response = [];

            var bfs = new BreadthFirst<GraphNode>(grid, position, new("0,0"));

            //Console.WriteLine(grid.DrawDistMap(bfs));

            foreach (var stepCnt in steps)
            {
                var oddEven = stepCnt % 2;
                var dists = bfs.DistancesMap.Where(kvp => kvp.Value <= stepCnt).Where(kvp => kvp.Value % 2 == oddEven).ToList();
                response.Add(dists.Count);
            }

            return response;
        }

        private List<(int, long)> FindLocationsForCoords(WeightedGrid<GraphNode> grid, GraphNode position, List<Coordinates> coordinates)
        {
            List<(int, long)> response = [];

            var bfs = new BreadthFirst<GraphNode>(grid, position, new("0,0"));

            //Console.WriteLine(grid.DrawDistMap(bfs));

            foreach (var node in grid.Nodes())
            {
                var stepCnt = bfs.DistancesMap[node];
                var oddEven = stepCnt % 2;
                var dists = bfs.DistancesMap.Where(kvp => kvp.Value <= stepCnt).Where(kvp => kvp.Value % 2 == oddEven).ToList();
                response.Add((dists.Count, stepCnt));
            }

            return response;
        }
    }
}
