using AdventOfCode2023.Day09;
using NUnit.Framework;

namespace AdventOfCode2023.Tests.Day09
{
    [TestFixture()]
    public class SolverTests
    {
        [Test()]
        public void Part1Test()
        {
            Solver solver = new();
            var rsp = solver.Part1(File.ReadAllText($"Day09\\sample.txt"));

            Assert.That(rsp, Is.EqualTo("114"));
        }

        [Test()]
        public void Part2Test()
        {
            Solver solver = new();
            var rsp = solver.Part2(File.ReadAllText($"Day09\\sample.txt"));

            Assert.That(rsp, Is.EqualTo("2"));
        }
    }
}