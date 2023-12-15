using AdventOfCode2023.Utils.Graph;

namespace AdventOfCode2023.Utils.Pathfinding
{
    public class BreadthFirst : IPathFinder
    {
        public bool HasSolution { get; }
        public int TotalCost { get; }
        public List<Coordinates> Path { get; }
        public Dictionary<Coordinates, int> DistancesMap { get; }

        public BreadthFirst(IWeightedGraph graph, Coordinates start, Coordinates finish, bool earlyExit = false)
        {
            Path = [];
            DistancesMap = [];

            Queue<Coordinates> frontier = [];
            Dictionary<Coordinates, Coordinates> cameFrom = [];

            frontier.Enqueue(start);

            var distance = 0;
            while (frontier.Count != 0)
            {
                var current = frontier.Dequeue();
                if (current.Equals(finish))
                    break;

                distance++;

                foreach (var neighbour in graph.Neighbors(graph.Node(current)!))
                {
                    if (!cameFrom.ContainsKey(neighbour.Coords))
                    {
                        frontier.Enqueue(neighbour.Coords);
                        cameFrom[neighbour.Coords] = current;
                        DistancesMap[neighbour.Coords] = distance;
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
