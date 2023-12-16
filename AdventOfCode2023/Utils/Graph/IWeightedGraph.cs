namespace AdventOfCode2023.Utils.Graph
{
    public interface IWeightedGraph
    {
        IEnumerable<GraphNode> Nodes();
        IEnumerable<(GraphNode, GraphNode, int)> Connections();
        int Cost(GraphNode from, GraphNode to);
        IEnumerable<GraphNode> Neighbours(GraphNode node);
        GraphNode? Node(string name);
    }
}