using AdventOfCode2023.Day01;
using NUnit.Framework;

namespace AdventOfCode2023.Tests.Day01
{
    [TestFixture()]
    public class SolverTests
    {
        [Test()]
        public void Part1Test()
        {
            Solver solver = new();
            var rsp = solver.Part1(File.ReadAllText($"Day01\\sample1.txt"));

            Assert.That(rsp, Is.EqualTo("142"));
        }

        [Test()]
        public void Part2Test()
        {
            Solver solver = new();
            var rsp = solver.Part2(File.ReadAllText($"Day01\\sample2.txt"));

            Assert.That(rsp, Is.EqualTo("281"));
        }
    }
}