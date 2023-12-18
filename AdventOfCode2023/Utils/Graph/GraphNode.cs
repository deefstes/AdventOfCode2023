using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AdventOfCode2023.Utils.Graph
{
    public class GraphNode(string name, int value = 1, Coordinates? coords = null) : IEquatable<GraphNode>
    {
        public readonly Coordinates? Coords = coords;
        public int Value = value;
        public string Name = name;

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
    }
}
