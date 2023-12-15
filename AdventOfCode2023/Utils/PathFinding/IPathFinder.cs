using AdventOfCode2023.Utils.Graph;

namespace AdventOfCode2023.Utils.Pathfinding
{
    public interface IPathFinder
    {
        public bool HasSolution { get; }
        public int TotalCost { get; }
        public List<string> Path { get; }
    }
}
