﻿using AdventOfCode2023.Day23;
using NUnit.Framework;

namespace AdventOfCode2023.Tests.Day23
{
    [TestFixture()]
    public class SolverTests
    {
        [Test()]
        public void Part1Test()
        {
            Solver solver = new();
            var rsp = solver.Part1(File.ReadAllText($"Day23\\sample.txt"));

            Assert.That(rsp, Is.EqualTo("94"));
        }

        [Test()]
        public void Part2Test()
        {
            Solver solver = new();
            var rsp = solver.Part2(File.ReadAllText($"Day23\\sample.txt"));

            Assert.That(rsp, Is.EqualTo("154"));
        }
    }
}