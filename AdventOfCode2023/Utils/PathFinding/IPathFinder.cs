using AdventOfCode2023.Utils.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Utils.Pathfinding
{
    public interface IPathFinder
    {
        public bool HasSolution { get; }
        public int TotalCost { get; }
        public List<Coordinates> Path { get; }
    }
}
