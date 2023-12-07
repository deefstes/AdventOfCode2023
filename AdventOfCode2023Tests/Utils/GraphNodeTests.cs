using AdventOfCode2023.Utils.Graph;
using NUnit.Framework;

namespace AdventOfCode2023.Utils.Tests
{
    [TestFixture()]
    public class GraphNodeTests
    {
        [Test()]
        [TestCase(0, 0, 0, 0, 0, 0, true, TestName = "EqualsTest where equals")]
        [TestCase(0, 0, 0, 1, 1, 1, false, TestName = "EqualsTest where differ on everything")]
        [TestCase(0, 0, 0, 1, 1, 0, false, TestName = "EqualsTest where differ on location")]
        [TestCase(0, 0, 0, 0, 0, 1, false, TestName = "EqualsTest where differ on value")]
        public void EqualsTest(int x1, int y1, int value1, int x2, int y2, int value2, bool expectedEquals)
        {
            var n1 = new GraphNode(new(x1, y1), value1, $"{x1},{y1}");
            var n2 = new GraphNode(new(x2, y2), value2, $"{x2},{y2}");

            var equals = n1.Equals(n2);
            Assert.That(equals, Is.EqualTo(expectedEquals));
        }
    }
}