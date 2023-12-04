using AdventOfCode2023.Day03;
using NUnit.Framework;

namespace AdventOfCode2023.Tests.Day03
{
    [TestFixture()]
    public class SolverTests
    {
        [Test()]
        public void Part1Test()
        {
            Solver solver = new();
            var rsp = solver.Part1(File.ReadAllText($"Day03\\sample.txt"));

            Assert.That(rsp, Is.EqualTo("4361"));
        }

        [Test()]
        public void Part2Test()
        {
            Solver solver = new();
            var rsp = solver.Part2(File.ReadAllText($"Day03\\sample.txt"));

            Assert.That(rsp, Is.EqualTo("467835"));
        }
    }
}