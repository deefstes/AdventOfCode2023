namespace AdventOfCode2023.Utils.Graph
{
    public interface IWeightedGraph
    {
        int Cost(GraphNode from, GraphNode to);
        IEnumerable<GraphNode> Neighbors(GraphNode node);
        GraphNode? Node(Coordinates coordinates);
    }
}