using AdventOfCode2023.Utils.Graph;

namespace AdventOfCode2023.Utils.Pathfinding
{
    public interface IPathFinder<TNode> where TNode : IEquatable<TNode>, IComparable<TNode>
    {
        public bool HasSolution { get; }
        public int TotalCost { get; }
        public List<TNode> Path { get; }
    }
}
