using AdventOfCode2023.Day10;
using NUnit.Framework;

namespace AdventOfCode2023.Tests.Day10
{
    [TestFixture()]
    public class SolverTests
    {
        [Test()]
        public void Part1Test()
        {
            Solver solver = new();
            var rsp = solver.Part1(File.ReadAllText($"Day10\\sample1.txt"));

            Assert.That(rsp, Is.EqualTo("8"));
        }

        [Test()]
        public void Part2Test()
        {
            Solver solver = new();

            var rsp = solver.Part2(File.ReadAllText($"Day10\\sample2.txt"));
            Assert.That(rsp, Is.EqualTo("10"));

            rsp = solver.Part2(File.ReadAllText($"Day10\\sample3.txt"));
            Assert.That(rsp, Is.EqualTo("4"));

            rsp = solver.Part2(File.ReadAllText($"Day10\\sample4.txt"));
            Assert.That(rsp, Is.EqualTo("4"));
        }
    }
}