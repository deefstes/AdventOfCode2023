using AdventOfCode2023.Utils.Graph;
using NUnit.Framework;

namespace AdventOfCode2023.Tests.Utils
{
    [TestFixture()]
    public class WeightedGraphTests
    {
        [Test()]
        public void WeightedGraphTest()
        {
            var graph = new WeightedGraph<string>();
            graph.AddConnection("1", "2", 1);
            graph.AddConnection("1", "3", 4);
            graph.AddConnection("3", "2", 2);
            graph.AddConnection("4", "3", 3);
            graph.AddConnection("0", "4", 8);
            graph.AddConnection("0", "3", 7);
            graph.AddConnection("0", "1", 3);

            var output = graph.ToUML(n => n ?? "?");

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