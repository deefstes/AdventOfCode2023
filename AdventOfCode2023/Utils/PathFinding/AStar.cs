using AdventOfCode2023.Utils.Graph;

namespace AdventOfCode2023.Utils.Pathfinding
{
    public class AStar : IPathFinder
    {
        public bool HasSolution { get; }
        public int TotalCost { get; }
        public List<Coordinates> Path { get; }

        private readonly Dictionary<Coordinates, Coordinates> _cameFrom = [];
        private readonly Dictionary<Coordinates, int> _costSoFar = [];

        public AStar(IWeightedGraph graph, Coordinates start, Coordinates finish)
        {
            Path = [];

            var frontier = new PriorityQueue<Coordinates, int>();
            frontier.Enqueue(start, 0);

            _cameFrom[start] = start;
            _costSoFar[start] = 0;

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();

                if (current.Equals(finish))
                {
                    break;
                }

                foreach (var next in graph.Neighbors(graph.Node(current)!))
                {
                    int newCost = _costSoFar[current] + graph.Cost(graph.Node(current)!, next);
                    if (!_costSoFar.ContainsKey(next.Coords) || newCost < _costSoFar[next.Coords])
                    {
                        _costSoFar[next.Coords] = newCost;
                        int priority = newCost + Heuristic(next.Coords, finish);
                        frontier.Enqueue(next.Coords, priority);
                        _cameFrom[next.Coords] = current;
                    }
                }
            }

            TotalCost = 0;
            HasSolution = _cameFrom.ContainsKey(finish);

            if (HasSolution)
            {
                TotalCost = _costSoFar[finish];

                var node = finish;
                while (!node.Equals(start))
                {
                    Path.Add(node);
                    node = _cameFrom[node];
                }
                Path.Add(start);
                Path.Reverse();
            }
        }

        static private int Heuristic(Coordinates a, Coordinates b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }
    }
}
