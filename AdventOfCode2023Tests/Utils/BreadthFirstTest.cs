using AdventOfCode2023.Utils;
using AdventOfCode2023.Utils.Graph;
using AdventOfCode2023.Utils.Pathfinding;
using NUnit.Framework;

namespace AdventOfCode2023.Tests.Utils
{
    [TestFixture()]
    public class BreadthFirstTest
    {
        [Test()]
        public void BreadthFirstTest_Valid_Route()
        {
            // Arrange
            var input = "1111111111\r\n"
                      + "1111551111\r\n"
                      + "1111555111\r\n"
                      + "1111555511\r\n"
                      + "1115555511\r\n"
                      + "1115555511\r\n"
                      + "1111555111\r\n"
                      + "1000555111\r\n"
                      + "1000551111\r\n"
                      + "1111111111\r\n";
            var gridNodes = GraphNode.GridFromIntGrid(input.AsIntGrid());
            var grid = new WeightedGrid<GraphNode>(gridNodes);

            grid.DeleteNode(new(1, 7));
            grid.DeleteNode(new(1, 8));
            grid.DeleteNode(new(2, 7));
            grid.DeleteNode(new(2, 8));
            grid.DeleteNode(new(3, 7));
            grid.DeleteNode(new(3, 8));

            // Act
            var bfs = new BreadthFirst<GraphNode>(
                graph: grid,
                start: gridNodes[1, 4],
                finish: gridNodes[8, 5]);

            // Assert
            Assert.That(
                grid.Draw(n => n?.Value.ToString() ?? "#", bfs),
                Is.EqualTo("1111111111\r\n"
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
            var input = "1111111111\r\n"
                      + "1111551111\r\n"
                      + "1111555111\r\n"
                      + "1111555511\r\n"
                      + "1115555511\r\n"
                      + "1115555511\r\n"
                      + "1111555111\r\n"
                      + "1000555111\r\n"
                      + "1000551111\r\n"
                      + "1111111111\r\n";
            var gridNodes = GraphNode.GridFromIntGrid(input.AsIntGrid());
            var grid = new WeightedGrid<GraphNode>(gridNodes);

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

            // Act
            var bfs = new BreadthFirst<GraphNode>(
                graph: grid,
                start: gridNodes[1, 4],
                finish: gridNodes[8, 5]);

            // Assert
            Assert.That(
                grid.Draw(n => n?.Value.ToString() ?? "#", bfs),
                Is.EqualTo("1111111111\r\n"
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