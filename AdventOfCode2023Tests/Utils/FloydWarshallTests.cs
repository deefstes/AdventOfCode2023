using AdventOfCode2023.Utils.Graph;
using AdventOfCode2023.Utils.Pathfinding;
using NUnit.Framework;
using System.Text;

namespace AdventOfCode2023.Tests.Utils
{
    [TestFixture()]
    public class FloydWarshallTests
    {
        [Test()]
        public void FloydWarshallTest()
        {
            // Arrange
            var graph = new WeightedGraph<string>();
            graph.AddConnection("1", "3", -2);
            graph.AddConnection("3", "4", 2);
            graph.AddConnection("4", "2", -1);
            graph.AddConnection("2", "1", 4);
            graph.AddConnection("2", "3", 3);

            // Act
            var fw = new FloydWarshall<string>(
                graph: graph,
                start: "1",
                finish: "4");

            var nodes = graph.Nodes().ToList();
            nodes.Sort((x, y) => string.Compare(x, y));
            long[,] dists = new long[nodes.Count, nodes.Count];

            for (int y = 0; y < nodes.Count; y++)
                for (int x = 0; x < nodes.Count; x++)
                    dists[x, y] = fw.DistancesMap[(nodes[x], nodes[y])];

            StringBuilder sb = new();
            var longestNodeName = graph.Nodes().Select(n => n.Length).Max();
            var longestDistance = fw.DistancesMap.Select(d=>d.Value.ToString().Length).Max();
            for (int y = 0; y < nodes.Count; y++)
            {
                sb.Append(nodes[y].PadLeft(longestNodeName) + ": ");
                for (int x = 0; x < nodes.Count; x++)
                    sb.Append(dists[y,x].ToString().PadLeft(longestDistance+1));
                sb.AppendLine();
            }

            // Assert
            Assert.That(sb.ToString(), Is.EqualTo("1:   0 -1 -2  0\r\n"
                                                + "2:   4  0  2  4\r\n"
                                                + "3:   5  1  0  2\r\n"
                                                + "4:   3 -1  1  0\r\n"));
            Assert.That(string.Join(',', fw.Path), Is.EqualTo("1,3,4"));
        }
    }
}