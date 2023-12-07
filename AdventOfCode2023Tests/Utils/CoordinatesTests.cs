using AdventOfCode2023.Utils.Graph;
using NUnit.Framework;

namespace AdventOfCode2023.Utils.Tests
{
    [TestFixture()]
    public class CoordinatesTests
    {
        [Test()]
        [TestCase(0, 0, 0, 0, true, TestName = "EqualsTest where equals")]
        [TestCase(0, 0, 0, 1, false, TestName = "EqualsTest where different")]
        public void EqualsTest(int x1, int y1, int x2, int y2, bool expectedEquals)
        {
            var n1 = new Coordinates(x1, y1);
            var n2 = new Coordinates(x2, y2);

            var equals = n1.Equals(n2);
            Assert.That(equals, Is.EqualTo(expectedEquals));
        }
    }
}