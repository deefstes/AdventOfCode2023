using AdventOfCode2023.Day07;
using NUnit.Framework;

namespace AdventOfCode2023.Tests.Day07
{
    [TestFixture()]
    public class SolverTests
    {
        [Test()]
        public void Part1Test()
        {
            Solver solver = new();
            var rsp = solver.Part1(File.ReadAllText($"Day07\\sample.txt"));

            Assert.That(rsp, Is.EqualTo("6440"));
        }

        [Test()]
        public void Part2Test()
        {
            Solver solver = new();
            var rsp = solver.Part2(File.ReadAllText($"Day07\\sample.txt"));

            Assert.That(rsp, Is.EqualTo("5905"));
        }
    }
}