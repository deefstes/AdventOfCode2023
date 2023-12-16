using AdventOfCode2023.Utils.Graph;
using NUnit.Framework;

namespace AdventOfCode2023.Utils.Tests
{
    [TestFixture()]
    public class WeightedGraphTests
    {
        [Test()]
        public void WeightedGraphTest()
        {
            var graph = new WeightedGraph();
            graph.AddConnection(new("1"), new("2"), 1);
            graph.AddConnection(new("1"), new("3"), 4);
            graph.AddConnection(new("3"), new("2"), 2);
            graph.AddConnection(new("4"), new("3"), 3);
            graph.AddConnection(new("0"), new("4"), 8);
            graph.AddConnection(new("0"), new("3"), 7);
            graph.AddConnection(new("0"), new("1"), 3);

            var output = graph.ToUML();

            Assert.That(output, Is.EqualTo("@startuml\r\n"
                                         + "(0) --> (1) : 3\r\n"
                                         + "(0) --> (3) : 7\r\n"
                                         + "(0) --> (4) : 8\r\n"
                                         + "(1) --> (2) : 1\r\n"
                                         + "(1) --> (3) : 4\r\n"
                                         + "(3) --> (2) : 2\r\n"
                                         + "(4) --> (3) : 3\r\n"
                                         + "@enduml\r\n"));
        }
    }
}