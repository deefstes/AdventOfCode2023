using AdventOfCode2023.Day19;
using NUnit.Framework;

namespace AdventOfCode2023.Tests.Day19
{
    [TestFixture()]
    public class SolverTests
    {
        [Test()]
        public void Part1Test()
        {
            Solver solver = new();
            var rsp = solver.Part1(File.ReadAllText($"Day19\\sample.txt"));

            Assert.That(rsp, Is.EqualTo("19114"));
        }

        [Test()]
        public void Part2Test()
        {
            Solver solver = new();
            var rsp = solver.Part2(File.ReadAllText($"Day19\\sample.txt"));

            Assert.That(rsp, Is.EqualTo("167409079868000"));
        }
    }
}