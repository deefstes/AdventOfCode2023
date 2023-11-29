using AdventOfCode2023.Day17;
using NUnit.Framework;

namespace AdventOfCode2023.Tests.Day17
{
    [TestFixture()]
    public class SolverTests
    {
        [Test()]
        public void Part1Test()
        {
            Solver solver = new();
            var rsp = solver.Part1(File.ReadAllText($"Day17\\sample.txt"));

            Assert.That(rsp, Is.EqualTo("Not yet implemented"));
        }

        [Test()]
        public void Part2Test()
        {
            Solver solver = new();
            var rsp = solver.Part2(File.ReadAllText($"Day17\\sample.txt"));

            Assert.That(rsp, Is.EqualTo("Not yet implemented"));
        }
    }
}