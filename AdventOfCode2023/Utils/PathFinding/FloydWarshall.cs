using AdventOfCode2023.Utils.Graph;

namespace AdventOfCode2023.Utils.Pathfinding
{
    public class FloydWarshall<TNode> : IPathFinder<TNode> where TNode : IEquatable<TNode>, IComparable<TNode>
    {
        public bool HasSolution { get; }
        public long TotalCost { get; }
        public List<TNode> Path { get; }
        public Dictionary<(TNode, TNode), long> DistancesMap { get; }
        private Dictionary<(TNode, TNode), TNode> ComeFromMap { get; }

        public FloydWarshall(IWeightedGraph<TNode> graph, TNode start, TNode finish)
        {
            Path = [];
            HasSolution = true;
            DistancesMap = [];
            ComeFromMap = [];

            foreach (var node in graph.Nodes())
            {
                DistancesMap[(node, node)] = 0;
            }

            foreach (var connection in graph.Connections())
            {
                DistancesMap[(connection.Item1, connection.Item2)] = connection.Item3;
                ComeFromMap[(connection.Item1, connection.Item2)] = connection.Item1;
            }

            foreach (var k in graph.Nodes().Select(n => n))
                foreach (var i in graph.Nodes().Select(n => n))
                    foreach (var j in graph.Nodes().Select(n => n))
                    {
                        if (!DistancesMap.TryGetValue((i, j), out long ijDist))
                            ijDist = 99999;

                        if (!DistancesMap.TryGetValue((i, k), out long ikDist))
                            ikDist = 99999;

                        if (!DistancesMap.TryGetValue((k, j), out long kjDist))
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
