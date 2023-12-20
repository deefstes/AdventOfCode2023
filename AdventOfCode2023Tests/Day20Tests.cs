using AdventOfCode2023.Day20;
using NUnit.Framework;

namespace AdventOfCode2023.Tests.Day20
{
    [TestFixture()]
    public class SolverTests
    {
        [Test()]
        [TestCase($"Day20\\sample1.txt", "32000000")]
        [TestCase($"Day20\\sample2.txt", "11687500")]
        public void Part1Test(string input, string expected)
        {
            Solver solver = new();
            var rsp1 = solver.Part1(File.ReadAllText(input));

            Assert.That(rsp1, Is.EqualTo(expected));
        }

        [Test()]
        public void Part2Test()
        {
            Solver solver = new();
            var rsp = solver.Part2(File.ReadAllText($"Day20\\sample2.txt"));

            Assert.That(rsp, Is.EqualTo("1"));
        }
    }
}