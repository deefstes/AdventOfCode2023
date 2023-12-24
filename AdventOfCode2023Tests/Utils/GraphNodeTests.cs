using AdventOfCode2023.Utils.Graph;
using NUnit.Framework;

namespace AdventOfCode2023.Tests.Utils
{
    [TestFixture()]
    public class GraphNodeTests
    {
        [Test()]
        [TestCase(0, 0, 0, 0, 0, 0, true, TestName = "EqualsTest where equals")]
        [TestCase(0, 0, 0, 1, 1, 1, false, TestName = "EqualsTest where differ on everything")]
        [TestCase(0, 0, 0, 1, 1, 0, false, TestName = "EqualsTest where differ on location")]
        [TestCase(0, 0, 0, 0, 0, 1, false, TestName = "EqualsTest where differ on value")]
        [TestCase(-1, -1, 0, -1, -1, 0, true, TestName = "EqualsTest where both coords are null")]
        [TestCase(-1, -1, 0, 0, 0, 0, false, TestName = "EqualsTest where first coords is null")]
        [TestCase(0, 0, 0, -1, -1, 0, false, TestName = "EqualsTest where second coords is null")]
        public void EqualsTest(int x1, int y1, int value1, int x2, int y2, int value2, bool expectedEquals)
        {
            Coordinates? c1 = x1 >= 0 ? new(x1, y1) : null;
            Coordinates? c2 = x2 >= 0 ? new(x2, y2) : null;
            var n1 = new GraphNode($"{x1},{y1}", value1, c1);
            var n2 = new GraphNode($"{x2},{y2}", value2, c2);

            var equals = n1.Equals(n2);
            Assert.That(equals, Is.EqualTo(expectedEquals));
        }
    }
}