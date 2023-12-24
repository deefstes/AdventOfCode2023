using AdventOfCode2023.Utils.Graph;
using NUnit.Framework;

namespace AdventOfCode2023.Tests.Utils
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

        [Test()]
        [TestCase(Direction.North, true, 1, Direction.NorthWest)]
        [TestCase(Direction.North, true, 2, Direction.West)]
        [TestCase(Direction.North, true, 3, Direction.SouthWest)]
        [TestCase(Direction.North, true, 4, Direction.South)]
        [TestCase(Direction.North, true, 5, Direction.SouthEast)]
        [TestCase(Direction.North, true, 6, Direction.East)]
        [TestCase(Direction.North, true, 7, Direction.NorthEast)]
        [TestCase(Direction.North, true, 8, Direction.North)]
        [TestCase(Direction.South, false, 1, Direction.SouthWest)]
        [TestCase(Direction.South, false, 2, Direction.West)]
        [TestCase(Direction.South, false, 3, Direction.NorthWest)]
        [TestCase(Direction.South, false, 4, Direction.North)]
        [TestCase(Direction.South, false, 5, Direction.NorthEast)]
        [TestCase(Direction.South, false, 6, Direction.East)]
        [TestCase(Direction.South, false, 7, Direction.SouthEast)]
        [TestCase(Direction.South, false, 8, Direction.South)]
        public void DirectionsTest(Direction input, bool left, int halfSteps, Direction expected)
        {
            Direction response = left?input.TurnLeft(halfSteps):input.TurnRight(halfSteps);

            Assert.That(response, Is.EqualTo(expected));
        }
    }
}