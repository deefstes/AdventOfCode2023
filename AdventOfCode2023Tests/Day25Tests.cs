﻿using AdventOfCode2023.Day25;
using NUnit.Framework;

namespace AdventOfCode2023.Tests.Day25
{
    [TestFixture()]
    public class SolverTests
    {
        [Test()]
        public void Part1Test()
        {
            Solver solver = new();
            var rsp = solver.Part1(File.ReadAllText($"Day25\\sample.txt"));

            Assert.That(rsp, Is.EqualTo("54"));
        }

        [Test()]
        public void Part2Test()
        {
            Solver solver = new();
            var rsp = solver.Part2(File.ReadAllText($"Day25\\sample.txt"));

            Assert.That(rsp, Is.EqualTo("No Part 2"));
        }
    }
}