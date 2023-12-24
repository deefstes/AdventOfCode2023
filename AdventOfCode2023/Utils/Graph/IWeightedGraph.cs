namespace AdventOfCode2023.Utils.Graph
{
    public interface IWeightedGraph<TNode> where TNode : IEquatable<TNode>, IComparable<TNode>
    {
        IEnumerable<TNode> Nodes();
        IEnumerable<(TNode, TNode, int)> Connections();
        int Cost(TNode from, TNode to);
        IEnumerable<TNode> Neighbours(TNode node);
        TNode? Node(string name);
    }
}