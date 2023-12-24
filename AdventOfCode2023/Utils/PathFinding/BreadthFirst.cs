using AdventOfCode2023.Utils.Graph;

namespace AdventOfCode2023.Utils.Pathfinding
{
    public class BreadthFirst<TNode> : IPathFinder<TNode> where TNode : IEquatable<TNode>, IComparable<TNode>
    {
        public bool HasSolution { get; }
        public int TotalCost { get; }
        public List<TNode> Path { get; }
        public Dictionary<TNode, int> DistancesMap { get; }

        public BreadthFirst(IWeightedGraph<TNode> graph, TNode start, TNode finish, bool earlyExit = false)
        {
            Path = [];
            DistancesMap = [];

            Queue<TNode> frontier = [];
            Dictionary<TNode, TNode> cameFrom = [];

            frontier.Enqueue(start);

            var distance = 0;
            while (frontier.Count != 0)
            {
                var current = frontier.Dequeue();
                if (earlyExit && current.Equals(finish))
                    break;

                DistancesMap.TryGetValue(current, out distance);
                distance++;

                foreach (var neighbour in graph.Neighbours(current))
                {
                    if (!cameFrom.ContainsKey(neighbour))
                    {
                        frontier.Enqueue(neighbour);
                        cameFrom[neighbour] = current;
                        DistancesMap[neighbour] = distance;
                    }
                }
            }

            HasSolution = cameFrom.ContainsKey(finish);
            if (HasSolution)
            {
                TotalCost = DistancesMap[finish];

                var current = finish;
                while (!current.Equals(start))
                {
                    Path.Add(current);
                    current = cameFrom[current];
                }

                Path.Add(start);
                Path.Reverse();
            }
        }
    }
}
