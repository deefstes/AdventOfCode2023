using AdventOfCode2023.Utils.Pathfinding;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace AdventOfCode2023.Utils.Graph
{
    public class WeightedGraph<TNode> : IWeightedGraph<TNode> where TNode : IComparable<TNode>, IEquatable<TNode>
    {
        private bool _directed;
        private readonly Dictionary<string, TNode> _nodes = [];
        private readonly Dictionary<(TNode, TNode), int> _connections = [];
        private readonly Func<TNode, TNode, int> _costFunction;

        public WeightedGraph(Func<TNode, TNode, int>? costFunction = null, bool directed = true)
        {
            _directed = directed;
            _costFunction = costFunction ?? DefaultCostFunction;
        }

        public int Cost(TNode from, TNode to)
        {
            return _costFunction(from, to);
        }

        public bool AddConnection(TNode from, TNode to, int cost)
        {
            _nodes[from.ToString()] = from;
            _nodes[to.ToString()] = to;

            _connections[(from, to)] = cost;
            if (!_directed)
                _connections[(to, from)] = cost;

            return true;
        }

        public bool DeleteConnection(TNode from, TNode to)
        {
            _connections.Remove((from, to));
            if (!_directed)
                _connections.Remove((to, from));

            return true;
        }

        public IEnumerable<TNode> Neighbours(TNode node)
        {
            List<TNode> neighbours = [];

            foreach (TNode neighbour in _connections
                .Where(c => c.Key.Item1.Equals(node))
                .Select(c=>c.Key.Item2))
            {
                neighbours.Add(neighbour);
            }

            return neighbours;
        }

        public TNode? Node(string name)
        {
            if (_nodes.TryGetValue(name, out TNode? node))
                return node;

            return default;
        }

        private int DefaultCostFunction(TNode from, TNode to)
        {
            if (_connections.TryGetValue((from, to), out var cost))
                return cost;
            else
                return int.MaxValue;
        }

        public string ToUML(Func<TNode?, string> renderFunc)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("@startuml");

            List<string> lines = [];
            foreach (var kvp in _connections)
                lines.Add($"({renderFunc(kvp.Key.Item1)}) --> ({renderFunc(kvp.Key.Item2)}) : {kvp.Value}");
            lines.Sort();

            foreach (var line in lines)
                sb.AppendLine(line);

            sb.AppendLine("@enduml");

            return sb.ToString();
        }

        public IEnumerable<TNode> Nodes()
        {
            foreach (var node in _nodes.Values)
                yield return node;
        }

        public IEnumerable<(TNode, TNode, int)> Connections()
        {
            foreach (var kvp in _connections)
                yield return (kvp.Key.Item1, kvp.Key.Item2, kvp.Value);
        }

        public void ReverseDistances()
        {
            foreach (var c in _connections)
            {
                _connections[c.Key] = -c.Value;
            }
        }

        private class DirectedEqualityComparer : IEqualityComparer<(TNode, TNode)>
        {
            public bool Equals((TNode, TNode) x, (TNode, TNode) y)
            {
                return x.Item1.Equals(y.Item1) && x.Item2.Equals(y.Item2);
            }

            public int GetHashCode([DisallowNull] (TNode, TNode) obj)
            {
                return obj.Item1.GetHashCode() * 13 + obj.Item2.GetHashCode();
            }
        }

        private class UndirectedEqualityComparer : IEqualityComparer<(TNode, TNode)>
        {
            public bool Equals((TNode, TNode) x, (TNode, TNode) y)
            {
                return x.Item1.Equals(y.Item1) && x.Item2.Equals(y.Item2)
                    || x.Item1.Equals(y.Item2) && x.Item2.Equals(y.Item1);
            }

            public int GetHashCode([DisallowNull] (TNode, TNode) obj)
            {
                return obj.Item1.GetHashCode() * 13 + obj.Item2.GetHashCode();
            }
        }
    }
}
