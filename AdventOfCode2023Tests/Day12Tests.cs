﻿using AdventOfCode2023.Day12;
using NUnit.Framework;

namespace AdventOfCode2023.Tests.Day12
{
    [TestFixture()]
    public class SolverTests
    {
        [Test()]
        public void Part1Test()
        {
            Solver solver = new();
            var rsp = solver.Part1(File.ReadAllText($"Day12\\sample.txt"));

            Assert.That(rsp, Is.EqualTo("21"));
        }

        [Test()]
        public void Part2Test()
        {
            Solver solver = new();
            var rsp = solver.Part2(File.ReadAllText($"Day12\\sample.txt"));

            Assert.That(rsp, Is.EqualTo("525152"));
        }
    }
}