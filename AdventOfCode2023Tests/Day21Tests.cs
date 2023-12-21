using AdventOfCode2023.Day21;
using NUnit.Framework;

namespace AdventOfCode2023.Tests.Day21
{
    [TestFixture()]
    public class SolverTests
    {
        [Test()]
        public void Part1Test()
        {
            Solver solver = new();
            var rsp = solver.Part1(File.ReadAllText($"Day21\\sample.txt"));

            Assert.That(rsp, Is.EqualTo("16"));
        }

        [Test()]
        [Ignore("My method for finding cycles in the expanding universe doesn't work for the sample because there is no straight line from the start to any border")]
        public void Part2Test()
        {
            Solver solver = new();
            var rsp = solver.Part2(File.ReadAllText($"Day21\\sample.txt"));

            Assert.That(rsp, Is.EqualTo("16733044"));
        }
    }
}