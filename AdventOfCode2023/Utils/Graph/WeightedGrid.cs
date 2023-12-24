using AdventOfCode2023.Utils.Pathfinding;
using System.Text;

namespace AdventOfCode2023.Utils.Graph
{
    public class WeightedGrid<TNode> : IWeightedGraph<TNode> where TNode : IEquatable<TNode>, IComparable<TNode>
    {
        public readonly int Width;
        public readonly int Height;
        private readonly Dictionary<Coordinates, TNode> _nodes = [];
        private readonly Func<TNode, TNode, int> _costFunction;

        public WeightedGrid(TNode[,] arr, Func<TNode, TNode, int>? costFunction = null)
        {
            Width = arr.GetLength(0);
            Height = arr.GetLength(1);
            _costFunction = costFunction ?? DefaultCostFunction;

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    _nodes[new Coordinates(x, y)] = arr[x, y];
        }

        public int Cost(TNode from, TNode to)
        {
            return _costFunction(from, to);
        }

        public IEnumerable<TNode> Neighbours(TNode node)
        {
            var coords = _nodes.FirstOrDefault(x => node.Equals(x.Value)).Key;

            foreach (var dir in new List<Direction> { Direction.North, Direction.East, Direction.South, Direction.West })
            {
                var newCoords = coords.Move(dir);
                if (!newCoords.InBounds(Width, Height))
                    continue;

                if (_nodes.TryGetValue(newCoords, out var neighbour))
                    if (neighbour != null)
                        yield return neighbour;
            }
        }

        public TNode? Node(string coordsString)
        {
            if (!int.TryParse(coordsString.Split(',')[0], out var x))
                return default;
            if (!int.TryParse(coordsString.Split(',')[1], out var y))
                return default;

            if (_nodes.TryGetValue(new(x, y), out var node))
                return node;

            return default;
        }

        public bool DeleteNode(Coordinates coords)
        {
            if (_nodes.ContainsKey(coords))
                _nodes.Remove(coords);
            else
                return false;

            return true;
        }

        public bool SetNode(Coordinates coords, TNode node)
        {
            if (_nodes.ContainsKey(coords))
                _nodes[coords] = node;
            else
                return false;

            return true;
        }

        public string Draw(Func<TNode?, string> renderFunc, IPathFinder<TNode>? pathFinder = null)
        {
            StringBuilder sb = new();

            for (var y = 0; y < Width; y++)
            {
                for (var x = 0; x < Height; x++)
                {
                    Coordinates coords = new(x, y);
                    _nodes.TryGetValue(coords, out var node);
                    if (!_nodes.ContainsKey(coords)) { sb.Append(renderFunc(default)); }
                    else if (pathFinder != null && pathFinder.Path.Count != 0 && node != null && pathFinder.Path.First().Equals(node)) { sb.Append('S'); }
                    else if (pathFinder != null && pathFinder.Path.Count != 0 && node != null && pathFinder.Path.Last().Equals(node)) { sb.Append('F'); }
                    else if (pathFinder != null && pathFinder.Path.Count != 0 && node != null && pathFinder.Path.Contains(_nodes[coords])) { sb.Append('*'); }
                    else { sb.Append(renderFunc(_nodes[coords])); }
                    sb.Append(" ");
                }
                sb.AppendLine();
            }

            return sb.ToString().Replace(" \r\n", "\r\n");
        }

        public string DrawDistMap(BreadthFirst<TNode> bfs)
        {
            StringBuilder sb = new();
            var cellWidth = bfs.DistancesMap.Values.Max().ToString().Length;

            for (var y = 0; y < Width; y++)
            {
                for (var x = 0; x < Height; x++)
                {
                    if (x != 0)
                        sb.Append(" ");

                    if (_nodes.TryGetValue(new(x, y), out TNode? node))
                    {
                        if (bfs.DistancesMap.TryGetValue(node, out var distVal))
                            sb.Append(distVal.ToString().PadLeft(cellWidth));
                        else
                            sb.Append(new string('?', cellWidth));
                    }
                    else
                        sb.Append(new string('X', cellWidth));
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private int DefaultCostFunction(TNode from, TNode to)
        {
            var fromCoords = _nodes.FirstOrDefault(x => from.Equals(x.Value)).Key;
            var toCoords = _nodes.FirstOrDefault(x => to.Equals(x.Value)).Key;

            if (fromCoords.NeighboursCardinal().Contains(toCoords))
                return 1;

            return int.MaxValue;
        }

        public IEnumerable<TNode> Nodes()
        {
            foreach (var node in _nodes.Values)
                yield return node;
        }

        public IEnumerable<(TNode, TNode, int)> Connections()
        {
            foreach (var node in _nodes.Values)
                foreach (var neighbour in Neighbours(node))
                    yield return (node, neighbour, Cost(node, neighbour));
        }
    }
}
