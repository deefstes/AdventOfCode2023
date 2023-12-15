using AdventOfCode2023.Utils.Pathfinding;
using System.Text;

namespace AdventOfCode2023.Utils.Graph
{
    public class WeightedGrid : IWeightedGraph
    {
        public readonly int Width;
        public readonly int Height;
        private readonly Dictionary<Coordinates, GraphNode> _nodes = [];
        private readonly Func<GraphNode, GraphNode, int> _costFunction;

        public WeightedGrid(int width, int height, Func<GraphNode, GraphNode, int>? costFunction = null)
        {
            this.Width = width;
            this.Height = height;
            _costFunction = costFunction ?? DefaultCostFunction;

            for (int y = 0; y < this.Height; y++)
                for (int x = 0; x < this.Width; x++)
                    _nodes[new(x, y)] = new($"{x},{y}", 1, new(x, y));
        }

        public WeightedGrid(int[,] arr, Func<GraphNode, GraphNode, int>? costFunction = null)
        {
            Width = arr.GetLength(0);
            Height = arr.GetLength(1);
            _costFunction = costFunction ?? DefaultCostFunction;

            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    _nodes[new(x, y)] = new($"{x},{y}", arr[x, y], new(x, y));
        }

        public WeightedGrid(string[,] arr, Func<GraphNode, GraphNode, int>? costFunction = null)
        {
            Width = arr.GetLength(0);
            Height = arr.GetLength(1);
            _costFunction = costFunction ?? DefaultCostFunction;

            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    _nodes[new(x, y)] = new(arr[x, y], 1, new(x, y));
        }

        public int Cost(GraphNode from, GraphNode to)
        {
            return _costFunction(from, to);
        }

        public IEnumerable<GraphNode> Neighbours(GraphNode node)
        {
            List<GraphNode> neighbours = [];

            if (_nodes.TryGetValue(node.Coords!.Move(Direction.North), out GraphNode? neighbourNorth))
                neighbours.Add(neighbourNorth);

            if (_nodes.TryGetValue(node.Coords!.Move(Direction.East), out GraphNode? neighbourEast))
                neighbours.Add(neighbourEast);

            if (_nodes.TryGetValue(node.Coords!.Move(Direction.South), out GraphNode? neighbourSouth))
                neighbours.Add(neighbourSouth);

            if (_nodes.TryGetValue(node.Coords!.Move(Direction.West), out GraphNode? neighbourWest))
                neighbours.Add(neighbourWest);

            return neighbours;
        }

        public GraphNode? Node(string name)
        {
            if (_nodes.TryGetValue(new(int.Parse(name.Split(',')[0]), int.Parse(name.Split(',')[1])), out GraphNode? node))
                return node;

            return null;
        }

        public bool DeleteNode(Coordinates coords)
        {
            if (_nodes.ContainsKey(coords))
                _nodes.Remove(coords);
            else
                return false;

            return true;
        }

        public bool SetNode(GraphNode node)
        {
            _nodes[node.Coords!] = node;

            return true;
        }

        public bool SetNodeValue(Coordinates coords, int value)
        {
            if (_nodes.ContainsKey(coords))
                _nodes[coords].Value = value;
            else
                return false;

            return true;
        }

        public string Draw(IPathFinder pathFinder)
        {
            StringBuilder sb = new();

            for (var y = 0; y < 10; y++)
            {
                for (var x = 0; x < 10; x++)
                {
                    Coordinates coords = new(x, y);
                    if (!_nodes.ContainsKey(coords)) { sb.Append('#'); }
                    else if (pathFinder.Path.Count != 0 && pathFinder.Path.First().Equals(coords.ToString())) { sb.Append('S'); }
                    else if (pathFinder.Path.Count != 0 && pathFinder.Path.Last().Equals(coords.ToString())) { sb.Append('F'); }
                    else if (pathFinder.Path.Count != 0 && pathFinder.Path.Contains(coords.ToString())) { sb.Append('*'); }
                    else if (_nodes[coords].Value > 1) { sb.Append('+'); }
                    else { sb.Append('.'); }
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public string DrawByValues()
        {
            StringBuilder sb = new();
            var cellWidth = _nodes.Values.Select(node => node.Value).Max().ToString().Length;

            for (var y = 0; y < 10; y++)
            {
                for (var x = 0; x < 10; x++)
                {
                    if (x!=0)
                        sb.Append(" ");

                    if (_nodes.TryGetValue(new(x, y), out GraphNode? node))
                        sb.Append(node.Value.ToString().PadLeft(cellWidth));
                    else
                        sb.Append(new string('X', cellWidth));
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public string DrawByNames()
        {
            StringBuilder sb = new();
            var cellWidth = _nodes.Values.Select(node => node.Name.Length).Max();

            for (var y = 0; y < 10; y++)
            {
                for (var x = 0; x < 10; x++)
                {
                    if (x != 0)
                        sb.Append(" ");

                    if (_nodes.TryGetValue(new(x, y), out GraphNode? node))
                        sb.Append(node.Name.PadLeft(cellWidth));
                    else
                        sb.Append(new string('X', cellWidth));
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private int DefaultCostFunction(GraphNode from, GraphNode to)
        {
            if (_nodes.TryGetValue(to.Coords!, out GraphNode? node))
                return node?.Value ?? int.MaxValue;

            return int.MaxValue;
        }
    }
}
