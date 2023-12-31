﻿using AdventOfCode2023.Day11;
using NUnit.Framework;

namespace AdventOfCode2023.Tests.Day11
{
    [TestFixture()]
    public class SolverTests
    {
        [Test()]
        public void Part1Test()
        {
            Solver solver = new();
            var rsp = solver.Part1(File.ReadAllText($"Day11\\sample.txt"));

            Assert.That(rsp, Is.EqualTo("374"));
        }

        [Test()]
        public void Part2Test()
        {
            Solver solver = new();
            var rsp = solver.Part2(File.ReadAllText($"Day11\\sample.txt"));

            Assert.That(rsp, Is.EqualTo("82000210"));
        }
    }
}