using AdventOfCode2023.Day18;
using NUnit.Framework;

namespace AdventOfCode2023.Tests.Day18
{
    [TestFixture()]
    public class SolverTests
    {
        [Test()]
        public void Part1Test()
        {
            Solver solver = new();
            var rsp = solver.Part1(File.ReadAllText($"Day18\\sample.txt"));

            Assert.That(rsp, Is.EqualTo("62"));
        }

        [Test()]
        public void Part2Test()
        {
            Solver solver = new();
            var rsp = solver.Part2(File.ReadAllText($"Day18\\sample.txt"));

            Assert.That(rsp, Is.EqualTo("952408144115"));
        }
    }
}