using AdventOfCode2023.Utils.Pathfinding;
using System.Runtime.InteropServices;
using System.Text;

namespace AdventOfCode2023.Utils.Graph
{
    public class WeightedGraph : IWeightedGraph
    {
        private readonly Dictionary<string, GraphNode> _nodes = [];
        private readonly Dictionary<(string, string), int> _connections = [];
        private readonly Func<GraphNode, GraphNode, int> _costFunction;

        public WeightedGraph(Func<GraphNode, GraphNode, int>? costFunction = null)
        {
            _costFunction = costFunction ?? DefaultCostFunction;
        }

        public int Cost(GraphNode from, GraphNode to)
        {
            return _costFunction(from, to);
        }

        public bool AddConnection(GraphNode from, GraphNode to, int cost)
        {
            _nodes[from.Name] = from;
            _nodes[to.Name] = to;

            _connections[(from.Name, to.Name)] = cost;

            return true;
        }

        public IEnumerable<GraphNode> Neighbours(GraphNode node)
        {
            List<GraphNode> neighbours = [];

            foreach (string name in _connections
                .Where(c => c.Key.Item1 == node.Name)
                .Select(c=>c.Key.Item2))
            {
                neighbours.Add(Node(name)!);
            }

            return neighbours;
        }

        public GraphNode? Node(string name)
        {
            if (_nodes.TryGetValue(name, out GraphNode? node))
                return node;

            return null;
        }

        private int DefaultCostFunction(GraphNode from, GraphNode to)
        {
            if (_connections.TryGetValue((from.Name, to.Name), out var cost))
                return cost;
            else
                return int.MaxValue;
        }

        public string ToUML()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("@startuml");

            List<string> lines = [];
            foreach (var kvp in _connections)
                lines.Add($"({kvp.Key.Item1}) --> ({kvp.Key.Item2}) : {kvp.Value}");
            lines.Sort();

            foreach (var line in lines)
                sb.AppendLine(line);

            sb.AppendLine("@enduml");

            return sb.ToString();
        }
    }
}
