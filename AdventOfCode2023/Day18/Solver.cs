namespace AdventOfCode2023.Day18
{
    using AdventOfCode2023.Utils;
    using AdventOfCode2023.Utils.Graph;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Solver : ISolver
    {
        public string Part1(string input)
        {
            var digs = input
                .AsList()
                .Select(x => new DigLine(x[0], int.Parse(x.Split(' ')[1])))
                .ToList();

            var volume = Solve(digs);
            return volume.ToString();
        }

        public string Part2(string input)
        {
            var digs = input
                .AsList()
                .Select(x =>
                {
                    var elems = x.Split(' ');
                    var color = elems[2].TrimStart('(').TrimEnd(')');
                    var distance = (int)Convert.ToInt64(color[1..^1], 16);
                    var dir = color[^1] switch
                    {
                        '0' => 'R',
                        '1' => 'D',
                        '2' => 'L',
                        '3' => 'U',
                        _ => throw new NotImplementedException(),
                    };

                    return new DigLine(dir, distance);
                })
                .ToList();

            var volume = Solve(digs);
            return volume.ToString();
        }

        private long Solve(List<DigLine> digs)
        {
            List<Coordinates> path = [];
            Coordinates pos = new(0, 0);

            foreach (var dig in digs)
            {
                path.Add(pos);
                pos = pos.Move(dig.Direction, dig.Distance);
            }
            path.Add(path[0]);

            return path.TraverseAreaCalc();
        }

        private class DigLine
        {
            public Direction Direction;
            public int Distance;

            public DigLine(char direction, int distance)
            {
                switch (direction)
                {
                    case 'U':
                        Direction = Direction.North; break;
                    case 'R':
                        Direction = Direction.East; break;
                    case 'D':
                        Direction = Direction.South; break;
                    case 'L':
                        Direction = Direction.West; break;
                }

                Distance = distance;
            }
        }
    }
}
