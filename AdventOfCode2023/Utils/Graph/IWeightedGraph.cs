namespace AdventOfCode2023.Utils.Graph
{
    public interface IWeightedGraph
    {
        int Cost(GraphNode from, GraphNode to);
        IEnumerable<GraphNode> Neighbours(GraphNode node);
        GraphNode? Node(string name);
    }
}