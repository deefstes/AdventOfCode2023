using AdventOfCode2023.Day08;
using NUnit.Framework;

namespace AdventOfCode2023.Tests.Day08
{
    [TestFixture()]
    public class SolverTests
    {
        [Test()]
        public void Part1Test()
        {
            Solver solver = new();
            var rsp = solver.Part1(File.ReadAllText($"Day08\\sample1.txt"));

            Assert.That(rsp, Is.EqualTo("2"));
        }

        [Test()]
        public void Part2Test()
        {
            Solver solver = new();
            var rsp = solver.Part2(File.ReadAllText($"Day08\\sample2.txt"));

            Assert.That(rsp, Is.EqualTo("6"));
        }
    }
}