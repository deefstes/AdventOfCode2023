using AdventOfCode2023.Utils.Graph;
using AdventOfCode2023.Utils.Pathfinding;
using NUnit.Framework;

namespace AdventOfCode2023.Utils.Tests
{
    [TestFixture()]
    public class BreadthFirstTest
    {
        [Test()]
        public void BreadthFirstTest_Valid_Route()
        {
            // Arrange
            var grid = new WeightedGrid(10, 10);

            grid.DeleteNode(new(1, 7));
            grid.DeleteNode(new(1, 8));
            grid.DeleteNode(new(2, 7));
            grid.DeleteNode(new(2, 8));
            grid.DeleteNode(new(3, 7));
            grid.DeleteNode(new(3, 8));

            grid.SetNodeValue(new(3, 4), 5);
            grid.SetNodeValue(new(3, 5), 5);
            grid.SetNodeValue(new(4, 1), 5);
            grid.SetNodeValue(new(4, 2), 5);
            grid.SetNodeValue(new(4, 3), 5);
            grid.SetNodeValue(new(4, 4), 5);
            grid.SetNodeValue(new(4, 5), 5);
            grid.SetNodeValue(new(4, 6), 5);
            grid.SetNodeValue(new(4, 7), 5);
            grid.SetNodeValue(new(4, 8), 5);
            grid.SetNodeValue(new(5, 1), 5);
            grid.SetNodeValue(new(5, 2), 5);
            grid.SetNodeValue(new(5, 3), 5);
            grid.SetNodeValue(new(5, 4), 5);
            grid.SetNodeValue(new(5, 5), 5);
            grid.SetNodeValue(new(5, 6), 5);
            grid.SetNodeValue(new(5, 7), 5);
            grid.SetNodeValue(new(5, 8), 5);
            grid.SetNodeValue(new(6, 2), 5);
            grid.SetNodeValue(new(6, 3), 5);
            grid.SetNodeValue(new(6, 4), 5);
            grid.SetNodeValue(new(6, 5), 5);
            grid.SetNodeValue(new(6, 6), 5);
            grid.SetNodeValue(new(6, 7), 5);
            grid.SetNodeValue(new(7, 3), 5);
            grid.SetNodeValue(new(7, 4), 5);
            grid.SetNodeValue(new(7, 5), 5);

            // Act
            var bfs = new BreadthFirst(
                graph: grid,
                start: "1,4",
                finish: "8,5");

            // Assert
            Assert.That(grid.Draw(bfs), Is.EqualTo("1111111111\r\n"
                                                 + "1111551111\r\n"
                                                 + "1111555111\r\n"
                                                 + "1111555511\r\n"
                                                 + "1S*******1\r\n"
                                                 + "11155555F1\r\n"
                                                 + "1111555111\r\n"
                                                 + "1###555111\r\n"
                                                 + "1###551111\r\n"
                                                 + "1111111111\r\n"));
        }
        [Test()]
        public void BreadthFirstTest_No_Route()
        {
            // Arrange
            var grid = new WeightedGrid(10, 10);

            grid.DeleteNode(new(1, 7));
            grid.DeleteNode(new(1, 8));
            grid.DeleteNode(new(2, 7));
            grid.DeleteNode(new(2, 8));
            grid.DeleteNode(new(3, 7));
            grid.DeleteNode(new(3, 8));

            // Fence in starting position
            grid.DeleteNode(new(0, 2));
            grid.DeleteNode(new(1, 2));
            grid.DeleteNode(new(2, 2));
            grid.DeleteNode(new(3, 2));
            grid.DeleteNode(new(3, 3));
            grid.DeleteNode(new(3, 4));
            grid.DeleteNode(new(3, 5));
            grid.DeleteNode(new(3, 6));
            grid.DeleteNode(new(2, 6));
            grid.DeleteNode(new(1, 6));
            grid.DeleteNode(new(0, 6));

            grid.SetNodeValue(new(3, 4), 5);
            grid.SetNodeValue(new(3, 5), 5);
            grid.SetNodeValue(new(4, 1), 5);
            grid.SetNodeValue(new(4, 2), 5);
            grid.SetNodeValue(new(4, 3), 5);
            grid.SetNodeValue(new(4, 4), 5);
            grid.SetNodeValue(new(4, 5), 5);
            grid.SetNodeValue(new(4, 6), 5);
            grid.SetNodeValue(new(4, 7), 5);
            grid.SetNodeValue(new(4, 8), 5);
            grid.SetNodeValue(new(5, 1), 5);
            grid.SetNodeValue(new(5, 2), 5);
            grid.SetNodeValue(new(5, 3), 5);
            grid.SetNodeValue(new(5, 4), 5);
            grid.SetNodeValue(new(5, 5), 5);
            grid.SetNodeValue(new(5, 6), 5);
            grid.SetNodeValue(new(5, 7), 5);
            grid.SetNodeValue(new(5, 8), 5);
            grid.SetNodeValue(new(6, 2), 5);
            grid.SetNodeValue(new(6, 3), 5);
            grid.SetNodeValue(new(6, 4), 5);
            grid.SetNodeValue(new(6, 5), 5);
            grid.SetNodeValue(new(6, 6), 5);
            grid.SetNodeValue(new(6, 7), 5);
            grid.SetNodeValue(new(7, 3), 5);
            grid.SetNodeValue(new(7, 4), 5);
            grid.SetNodeValue(new(7, 5), 5);

            // Act
            var bfs = new BreadthFirst(
                graph: grid,
                start: "1,4",
                finish: "8,5");

            // Assert
            Assert.That(grid.Draw(bfs), Is.EqualTo("1111111111\r\n"
                                                 + "1111551111\r\n"
                                                 + "####555111\r\n"
                                                 + "111#555511\r\n"
                                                 + "111#555511\r\n"
                                                 + "111#555511\r\n"
                                                 + "####555111\r\n"
                                                 + "1###555111\r\n"
                                                 + "1###551111\r\n"
                                                 + "1111111111\r\n"));
        }
    }
}