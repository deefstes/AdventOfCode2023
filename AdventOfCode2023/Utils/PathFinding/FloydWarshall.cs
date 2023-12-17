using AdventOfCode2023.Utils.Graph;

namespace AdventOfCode2023.Utils.Pathfinding
{
    public class FloydWarshall : IPathFinder
    {
        public bool HasSolution { get; }
        public int TotalCost { get; }
        public List<string> Path { get; }
        public Dictionary<(string, string), int> DistancesMap { get; }
        private Dictionary<(string, string), string> ComeFromMap { get; }

        public FloydWarshall(IWeightedGraph graph, string start, string finish)
        {
            Path = [];
            HasSolution = true;
            DistancesMap = [];
            ComeFromMap = [];

            foreach (var node in graph.Nodes())
            {
                DistancesMap[(node.Name, node.Name)] = 0;
            }

            foreach (var connection in graph.Connections())
            {
                DistancesMap[(connection.Item1.Name, connection.Item2.Name)] = connection.Item3;
                ComeFromMap[(connection.Item1.Name, connection.Item2.Name)] = connection.Item1.Name;
            }

            foreach (var k in graph.Nodes().Select(n => n.Name))
                foreach (var i in graph.Nodes().Select(n => n.Name))
                    foreach (var j in graph.Nodes().Select(n => n.Name))
                    {
                        if (!DistancesMap.TryGetValue((i, j), out int ijDist))
                            ijDist = 99999;

                        if (!DistancesMap.TryGetValue((i, k), out int ikDist))
                            ikDist = 99999;

                        if (!DistancesMap.TryGetValue((k, j), out int kjDist))
                            kjDist = 99999;

                        if (ijDist > ikDist + kjDist)
                        {
                            DistancesMap[(i, j)] = ikDist + kjDist;
                            ComeFromMap[(i, j)] = k;
                        }
                        else
                            DistancesMap[(i, j)] = ijDist;
                    }

            var current = finish;
            while (!current.Equals(start))
            {
                Path.Add(current);
                current = ComeFromMap[(start, current)];
            }
            Path.Add(start);
            Path.Reverse();
        }
    }
}
