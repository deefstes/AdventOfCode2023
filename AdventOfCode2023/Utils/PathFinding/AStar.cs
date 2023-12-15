using AdventOfCode2023.Utils.Graph;

namespace AdventOfCode2023.Utils.Pathfinding
{
    public class AStar : IPathFinder
    {
        public bool HasSolution { get; }
        public int TotalCost { get; }
        public List<string> Path { get; }

        private readonly Dictionary<string, string> _cameFrom = [];
        private readonly Dictionary<string, int> _costSoFar = [];
        private readonly Func<string, string, int> _heuristicFunction;
        private readonly IWeightedGraph _graph;

        public AStar(IWeightedGraph graph, string start, string finish, Func<string, string, int>? heuristicFunction = null)
        {
            _graph = graph;
            _heuristicFunction = heuristicFunction ?? DefaultHeuristicFunction;
            Path = [];

            var frontier = new PriorityQueue<string, int>();
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

                foreach (var next in graph.Neighbours(graph.Node(current)!))
                {
                    int newCost = _costSoFar[current] + graph.Cost(graph.Node(current)!, next);
                    if (!_costSoFar.ContainsKey(next.Name) || newCost < _costSoFar[next.Name])
                    {
                        _costSoFar[next.Name] = newCost;
                        int priority = newCost + Heuristic(next.Name, finish);
                        frontier.Enqueue(next.Name, priority);
                        _cameFrom[next.Name] = current;
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

        private int Heuristic(string a, string b)
        {
            return _heuristicFunction(a, b);
        }

        private int DefaultHeuristicFunction(string a, string b)
        {
            var node1 = _graph.Node(a);
            var node2 = _graph.Node(b);

            if (node1 == null || node2 == null)
                throw new ArgumentException("Null node");

            return Math.Abs(node1!.Coords!.X - node2!.Coords!.X) + Math.Abs(node1!.Coords!.Y - node2!.Coords!.Y);
        }
    }
}
