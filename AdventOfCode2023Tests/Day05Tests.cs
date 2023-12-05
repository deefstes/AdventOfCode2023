using AdventOfCode2023.Day05;
using NUnit.Framework;

namespace AdventOfCode2023.Tests.Day05
{
    [TestFixture()]
    public class SolverTests
    {
        [Test()]
        public void Part1Test()
        {
            Solver solver = new();
            var rsp = solver.Part1(File.ReadAllText($"Day05\\sample.txt"));

            Assert.That(rsp, Is.EqualTo("35"));
        }

        [Test()]
        public void Part2Test()
        {
            Solver solver = new();
            var rsp = solver.Part2(File.ReadAllText($"Day05\\sample.txt"));

            Assert.That(rsp, Is.EqualTo("46"));
        }
    }
}