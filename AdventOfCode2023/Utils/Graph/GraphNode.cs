using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AdventOfCode2023.Utils.Graph
{
    public class GraphNode(string name, int value = 1, Coordinates? coords = null) : IEquatable<GraphNode>, IComparable<GraphNode>
    {
        public readonly Coordinates? Coords = coords;
        public int Value = value;
        public string Name = name;

        public int CompareTo(GraphNode? other)
        {
            if (other == null)
                return 1;

            return Value.CompareTo(other.Value);
        }

        public bool Equals(GraphNode? other)
        {
            if (other == null)
                return false;

            return Equals(Coords, other.Coords)
                && Value == other.Value
                && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Coords, Value, Name);
        }

        public static GraphNode[,] GridFromIntGrid(int[,] grid)
        {
            var width = grid.GetLength(0);
            var height = grid.GetLength(1);
            var result = new GraphNode[width, height];

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    result[x, y] = new($"{x},{y}", grid[x, y], new(x, y));

            return result;
        }
    }
}
