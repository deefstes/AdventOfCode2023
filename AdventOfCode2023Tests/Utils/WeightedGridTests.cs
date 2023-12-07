using AdventOfCode2023.Utils.Graph;
using NUnit.Framework;

namespace AdventOfCode2023.Utils.Tests
{
    [TestFixture()]
    public class WeightedGridTests
    {
        [Test()]
        public void DefaultCostFunctionTest()
        {
            var grid = new WeightedGrid(10, 10);
            GraphNode node1 = new(new(0, 1), 2, "node1");
            GraphNode node2 = new(new(0, 2), 4, "node2");
            GraphNode node3 = new(new(0, 3), 6, "node3");

            grid.SetNodeValue(node1.Coords, node1.Value);
            grid.SetNodeValue(node2.Coords, node2.Value);
            grid.SetNodeValue(node3.Coords, node3.Value);

            Assert.That(grid.Cost(node1, node2), Is.EqualTo(4));
            Assert.That(grid.Cost(node2, node3), Is.EqualTo(6));
        }

        [Test()]
        public void CustomCostFunctionTest()
        {
            var grid = new WeightedGrid(10, 10, CustomCostFunction);
            GraphNode node1 = new(new(0, 1), 2, "node1");
            GraphNode node2 = new(new(0, 2), 4, "node2");
            GraphNode node3 = new(new(0, 3), 6, "node3");

            grid.SetNodeValue(node1.Coords, node1.Value);
            grid.SetNodeValue(node2.Coords, node2.Value);
            grid.SetNodeValue(node3.Coords, node3.Value);

            Assert.That(grid.Cost(node1, node2), Is.EqualTo(2));
            Assert.That(grid.Cost(node2, node3), Is.EqualTo(2));
        }

        private int CustomCostFunction(GraphNode a, GraphNode b)
        {
            return b.Value - a.Value;
        }

        [Test()]
        public void DrawByValuesTest_Default_Grid()
        {
            var grid = new WeightedGrid(10, 10, CustomCostFunction);
            var str = grid.DrawByValues();
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
            var grid = new WeightedGrid(10, 10, CustomCostFunction);
            for (int i = 0; i < 10; i++)
                grid.DeleteNode(new(i, i));

            var str = grid.DrawByValues();
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
            var grid = new WeightedGrid(10, 10, CustomCostFunction);
            int inc = 1;
            for (int y = 0; y < 10; y++)
                for (int x = 0; x < 10; x++)
                    grid.SetNodeValue(new(x, y), inc++);

            for (int i = 0; i < 10; i++)
                grid.DeleteNode(new(i, 10 - i - 1));

            var str = grid.DrawByValues();
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
            var grid = new WeightedGrid(10, 10, CustomCostFunction);
            int inc = 1;
            for (int y = 0; y < 10; y++)
                for (int x = 0; x < 10; x++)
                    grid.SetNodeName(new(x, y), (inc++).ToString().PadLeft(3, '0'));

            for (int i = 0; i < 10; i++)
                grid.DeleteNode(new(i, 10 - i - 1));

            var str = grid.DrawByNames();
            Assert.That(str, Is.EqualTo("001 002 003 004 005 006 007 008 009 XXX\r\n"
                                      + "011 012 013 014 015 016 017 018 XXX 020\r\n"
                                      + "021 022 023 024 025 026 027 XXX 029 030\r\n"
                                      + "031 032 033 034 035 036 XXX 038 039 040\r\n"
                                      + "041 042 043 044 045 XXX 047 048 049 050\r\n"
                                      + "051 052 053 054 XXX 056 057 058 059 060\r\n"
                                      + "061 062 063 XXX 065 066 067 068 069 070\r\n"
                                      + "071 072 XXX 074 075 076 077 078 079 080\r\n"
                                      + "081 XXX 083 084 085 086 087 088 089 090\r\n"
                                      + "XXX 092 093 094 095 096 097 098 099 100\r\n"));
        }
    }
}