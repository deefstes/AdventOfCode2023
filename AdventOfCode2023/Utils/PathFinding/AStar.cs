using AdventOfCode2023.Utils.Graph;

namespace AdventOfCode2023.Utils.Pathfinding
{
    public class AStar<TNode> : IPathFinder<TNode> where TNode : IEquatable<TNode>, IComparable<TNode>
    {
        public bool HasSolution { get; }
        public int TotalCost { get; }
        public List<TNode> Path { get; }

        private readonly Dictionary<TNode, TNode> _cameFrom = [];
        private readonly Dictionary<TNode, int> _costSoFar = [];
        private readonly Func<TNode, TNode, int> _heuristicFunction;
        private readonly IWeightedGraph<TNode> _graph;

        public AStar(IWeightedGraph<TNode> graph, TNode start, TNode finish, Func<TNode, TNode, int>? heuristicFunction = null)
        {
            _graph = graph;
            _heuristicFunction = heuristicFunction ?? DefaultHeuristicFunction;
            Path = [];

            var frontier = new PriorityQueue<TNode, int>();
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

                foreach (var next in graph.Neighbours(current))
                {
                    int newCost = _costSoFar[current] + graph.Cost(current, next);
                    if (!_costSoFar.ContainsKey(next) || newCost < _costSoFar[next])
                    {
                        _costSoFar[next] = newCost;
                        int priority = newCost + Heuristic(next, finish);
                        frontier.Enqueue(next, priority);
                        _cameFrom[next] = current;
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

        private int Heuristic(TNode a, TNode b)
        {
            return _heuristicFunction(a, b);
        }

        private int DefaultHeuristicFunction(TNode a, TNode b)
        {
            if (a == null || b == null)
                throw new ArgumentException("Null node");

            return 1;
        }
    }
}
