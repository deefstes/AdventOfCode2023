using AdventOfCode2023.Day06;
using NUnit.Framework;

namespace AdventOfCode2023.Tests.Day06
{
    [TestFixture()]
    public class SolverTests
    {
        [Test()]
        public void Part1Test()
        {
            Solver solver = new();
            var rsp = solver.Part1(File.ReadAllText($"Day06\\sample.txt"));

            Assert.That(rsp, Is.EqualTo("288"));
        }

        [Test()]
        public void Part2Test()
        {
            Solver solver = new();
            var rsp = solver.Part2(File.ReadAllText($"Day06\\sample.txt"));

            Assert.That(rsp, Is.EqualTo("71503"));
        }

        [Test()]
        [TestCase(7, 9, 4)]
        [TestCase(15, 40, 8)]
        [TestCase(30, 200, 9)]
        [TestCase(71530, 940200, 71503)]
        public void ClosedFormTest(long time, long distance, long wins)
        {
            Assert.That(Solver.ClosedForm(time, distance), Is.EqualTo(wins));
        }
    }
}