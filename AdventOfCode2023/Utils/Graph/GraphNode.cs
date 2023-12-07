using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Utils.Graph
{
    public class GraphNode(Coordinates coords, int value, string name)
    {
        public readonly Coordinates Coords = coords;
        public int Value = value;
        public string Name = name;

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if (obj is GraphNode other)
            {
                return Coords.Equals(other.Coords)
                    && Value == other.Value
                    && Name == other.Name;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Coords, Value, Name);
        }
    }
}
