using AdventOfCode2023.Utils.Graph;
using AdventOfCode2023.Utils.Pathfinding;
using NUnit.Framework;
using System.Text;

namespace AdventOfCode2023.Utils.Tests
{
    [TestFixture()]
    public class FloydWarshallTests
    {
        [Test()]
        public void FloydWarshallTest()
        {
            // Arrange
            var graph = new WeightedGraph();
            graph.AddConnection(new("1"), new("3"), -2);
            graph.AddConnection(new("3"), new("4"), 2);
            graph.AddConnection(new("4"), new("2"), -1);
            graph.AddConnection(new("2"), new("1"), 4);
            graph.AddConnection(new("2"), new("3"), 3);

            // Act
            var fw = new FloydWarshall(
                graph: graph,
                start: "1",
                finish: "4");

            var nodes = graph.Nodes().ToList();
            nodes.Sort((x, y) => string.Compare(x.Name, y.Name));
            int[,] dists = new int[nodes.Count, nodes.Count];

            for (int y = 0; y < nodes.Count; y++)
                for (int x = 0; x < nodes.Count; x++)
                    dists[x, y] = fw.DistancesMap[(nodes[x].Name, nodes[y].Name)];

            StringBuilder sb = new();
            var longestNodeName = graph.Nodes().Select(n => n.Name.Length).Max();
            var longestDistance = fw.DistancesMap.Select(d=>d.Value.ToString().Length).Max();
            for (int y = 0; y < nodes.Count; y++)
            {
                sb.Append(nodes[y].Name.PadLeft(longestNodeName) + ": ");
                for (int x = 0; x < nodes.Count; x++)
                    sb.Append(dists[y,x].ToString().PadLeft(longestDistance+1));
                sb.AppendLine();
            }

            // Assert
            Assert.That(sb.ToString(), Is.EqualTo("1:   0 -1 -2  0\r\n"
                                                + "2:   4  0  2  4\r\n"
                                                + "3:   5  1  0  2\r\n"
                                                + "4:   3 -1  1  0\r\n"));
        }
    }
}