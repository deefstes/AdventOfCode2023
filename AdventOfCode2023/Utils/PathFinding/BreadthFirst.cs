using AdventOfCode2023.Utils.Graph;

namespace AdventOfCode2023.Utils.Pathfinding
{
    public class BreadthFirst : IPathFinder
    {
        public bool HasSolution { get; }
        public int TotalCost { get; }
        public List<string> Path { get; }
        public Dictionary<string, int> DistancesMap { get; }

        public BreadthFirst(IWeightedGraph graph, string start, string finish, bool earlyExit = false)
        {
            Path = [];
            DistancesMap = [];

            Queue<string> frontier = [];
            Dictionary<string, string> cameFrom = [];

            frontier.Enqueue(start);

            var distance = 0;
            while (frontier.Count != 0)
            {
                var current = frontier.Dequeue();
                if (current.Equals(finish))
                    break;

                distance++;

                foreach (var neighbour in graph.Neighbours(graph.Node(current)!))
                {
                    if (!cameFrom.ContainsKey(neighbour.Name))
                    {
                        frontier.Enqueue(neighbour.Name);
                        cameFrom[neighbour.Name] = current;
                        DistancesMap[neighbour.Name] = distance;
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
