using AdventOfCode2023.Utils.Graph;
using NUnit.Framework;

namespace AdventOfCode2023.Tests.Utils
{
    [TestFixture()]
    public class WeightedGridTests
    {
        [Test()]
        public void DefaultCostFunctionTest()
        {
            GraphNode[,] array = new GraphNode[1, 4];
            array[0, 1] = new("node1", 2, new(0, 1));
            array[0, 2] = new("node2", 4, new(0, 2));
            array[0, 3] = new("node3", 6, new(0, 3));

            var grid = new WeightedGrid<GraphNode>(array);

            Assert.That(grid.Cost(grid.Node("0,1")!, grid.Node("0,2")!), Is.EqualTo(1));
            Assert.That(grid.Cost(grid.Node("0,2")!, grid.Node("0,3")!), Is.EqualTo(1));
        }

        [Test()]
        public void CustomCostFunctionTest()
        {
            GraphNode[,] array = new GraphNode[1, 4];
            array[0, 1] = new("node1", 2, new(0, 1));
            array[0, 2] = new("node2", 4, new(0, 2));
            array[0, 3] = new("node3", 8, new(0, 3));

            var grid = new WeightedGrid<GraphNode>(array, CustomCostFunction);

            Assert.That(grid.Cost(grid.Node("0,1")!, grid.Node("0,2")!), Is.EqualTo(2));
            Assert.That(grid.Cost(grid.Node("0,2")!, grid.Node("0,3")!), Is.EqualTo(4));
        }

        private int CustomCostFunction(GraphNode a, GraphNode b)
        {
            return b.Value - a.Value;
        }

        [Test()]
        public void DrawByValuesTest_Default_Grid()
        {
            GraphNode[,] array = new GraphNode[10, 10];
            for (int x = 0; x < 10; x++)
                for (int y = 0; y < 10; y++)
                    array[x, y] = new($"{x},{y}", 1, new(x, y));

            var grid = new WeightedGrid<GraphNode>(array);

            var str = grid.Draw(n => n?.Value.ToString());
            Assert.That(str, Is.EqualTo("1 1 1 1 1 1 1 1 1 1\r\n"
                                      + "1 1 1 1 1 1 1 1 1 1\r\n"
                                      + "1 1 1 1 1 1 1 1 1 1\r\n"
                                      + "1 1 1 1 1 1 1 1 1 1\r\n"
                                      + "1 1 1 1 1 1 1 1 1 1\r\n"
                                      + "1 1 1 1 1 1 1 1 1 1\r\n"
                                      + "1 1 1 1 1 1 1 1 1 1\r\n"
                                      + "1 1 1 1 1 1 1 1 1 1\r\n"
                                      + "1 1 1 1 1 1 1 1 1 1\r\n"
                                      + "1 1 1 1 1 1 1 1 1 1\r\n"));
        }

        [Test()]
        public void DrawByValuesTest_With_Walls()
        {

            GraphNode[,] array = new GraphNode[10, 10];
            for (int x = 0; x < 10; x++)
                for (int y = 0; y < 10; y++)
                    array[x, y] = new($"{x},{y}", 1, new(x, y));

            var grid = new WeightedGrid<GraphNode>(array);
            for (int i = 0; i < 10; i++)
                grid.DeleteNode(new(i, i));

            var str = grid.Draw(n => n?.Value.ToString() ?? "X");
            Assert.That(str, Is.EqualTo("X 1 1 1 1 1 1 1 1 1\r\n"
                                      + "1 X 1 1 1 1 1 1 1 1\r\n"
                                      + "1 1 X 1 1 1 1 1 1 1\r\n"
                                      + "1 1 1 X 1 1 1 1 1 1\r\n"
                                      + "1 1 1 1 X 1 1 1 1 1\r\n"
                                      + "1 1 1 1 1 X 1 1 1 1\r\n"
                                      + "1 1 1 1 1 1 X 1 1 1\r\n"
                                      + "1 1 1 1 1 1 1 X 1 1\r\n"
                                      + "1 1 1 1 1 1 1 1 X 1\r\n"
                                      + "1 1 1 1 1 1 1 1 1 X\r\n"));
        }

        [Test()]
        public void DrawByValuesTest_With_Large_Values_And_Walls()
        {
            GraphNode[,] array = new GraphNode[10, 10];
            int inc = 1;
            for (int y = 0; y < 10; y++)
                for (int x = 0; x < 10; x++)
                    array[x, y] = new($"{x},{y}", inc++, new(x, y));

            var grid = new WeightedGrid<GraphNode>(array);

            for (int i = 0; i < 10; i++)
                grid.DeleteNode(new(i, 10 - i - 1));

            var str = grid.Draw(n => {
                var padLen = grid.Nodes().Max(n => n.Value).ToString().Length;
                return n?.Value.ToString().PadLeft(padLen) ?? new string('X', padLen);
            });
            Assert.That(str, Is.EqualTo("  1   2   3   4   5   6   7   8   9 XXX\r\n"
                                      + " 11  12  13  14  15  16  17  18 XXX  20\r\n"
                                      + " 21  22  23  24  25  26  27 XXX  29  30\r\n"
                                      + " 31  32  33  34  35  36 XXX  38  39  40\r\n"
                                      + " 41  42  43  44  45 XXX  47  48  49  50\r\n"
                                      + " 51  52  53  54 XXX  56  57  58  59  60\r\n"
                                      + " 61  62  63 XXX  65  66  67  68  69  70\r\n"
                                      + " 71  72 XXX  74  75  76  77  78  79  80\r\n"
                                      + " 81 XXX  83  84  85  86  87  88  89  90\r\n"
                                      + "XXX  92  93  94  95  96  97  98  99 100\r\n"));
        }

        [Test()]
        public void DrawByNamesTest()
        {
            GraphNode[,] array = new GraphNode[10, 10];
            int inc = 1;
            for (int y = 0; y < 10; y++)
                for (int x = 0; x < 10; x++)
                    array[x, y] = new($"{x},{y}", inc++, new(x, y));

            var grid = new WeightedGrid<GraphNode>(array);

            for (int i = 0; i < 10; i++)
                grid.DeleteNode(new(i, 10 - i - 1));

            var str = grid.Draw(n => n?.Name ?? "XXX");
            Assert.That(str, Is.EqualTo("0,0 1,0 2,0 3,0 4,0 5,0 6,0 7,0 8,0 XXX\r\n"
                                      + "0,1 1,1 2,1 3,1 4,1 5,1 6,1 7,1 XXX 9,1\r\n"
                                      + "0,2 1,2 2,2 3,2 4,2 5,2 6,2 XXX 8,2 9,2\r\n"
                                      + "0,3 1,3 2,3 3,3 4,3 5,3 XXX 7,3 8,3 9,3\r\n"
                                      + "0,4 1,4 2,4 3,4 4,4 XXX 6,4 7,4 8,4 9,4\r\n"
                                      + "0,5 1,5 2,5 3,5 XXX 5,5 6,5 7,5 8,5 9,5\r\n"
                                      + "0,6 1,6 2,6 XXX 4,6 5,6 6,6 7,6 8,6 9,6\r\n"
                                      + "0,7 1,7 XXX 3,7 4,7 5,7 6,7 7,7 8,7 9,7\r\n"
                                      + "0,8 XXX 2,8 3,8 4,8 5,8 6,8 7,8 8,8 9,8\r\n"
                                      + "XXX 1,9 2,9 3,9 4,9 5,9 6,9 7,9 8,9 9,9\r\n"));
        }
    }
}