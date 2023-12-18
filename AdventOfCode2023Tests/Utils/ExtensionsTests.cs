using AdventOfCode2023.Utils;
using AdventOfCode2023.Utils.Graph;
using NUnit.Framework;

namespace AdventOfCode2023Tests.Utils
{
    public class ExtensionsTests
    {
        [Test()]
        public void TopologicalSortTest()
        {
            var nodes = new HashSet<int>(new[] { 7, 5, 3, 8, 11, 2, 9, 10 });
            var edges = new HashSet<(int, int)>(
                new[] {
                    (7, 11),
                    (7, 8),
                    (5, 11),
                    (3, 8),
                    (3, 10),
                    (11, 2),
                    (11, 9),
                    (11, 10),
                    (8, 9)});

            var ret = ExtensionMethods.TopologicalSort(
                nodes,
                edges
            );

            Assert.That(ret, Is.EqualTo(new[] { 7, 5, 11, 2, 3, 10, 8, 9 }));
        }

        [Test()]
        public void TopologicalSortOnGraphTest()
        {
            var graph = new WeightedGraph();
            graph.AddConnection(new("7"), new("11"), 1);
            graph.AddConnection(new("7"), new("8"), 1);
            graph.AddConnection(new("5"), new("11"), 1);
            graph.AddConnection(new("3"), new("8"), 1);
            graph.AddConnection(new("3"), new("10"), 1);
            graph.AddConnection(new("11"), new("2"), 1);
            graph.AddConnection(new("11"), new("9"), 1);
            graph.AddConnection(new("11"), new("10"), 1);
            graph.AddConnection(new("8"), new("9"), 1);

            var ret = graph.TopologicalSort();

            Assert.That(ret.Select(n => n.Name), Is.EqualTo(new[] { "7", "5", "11", "2", "3", "10", "8", "9" }));
        }
    }
}
