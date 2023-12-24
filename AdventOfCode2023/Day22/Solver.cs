namespace AdventOfCode2023.Day22
{
    using AdventOfCode2023.Utils;
    using AdventOfCode2023.Utils.Graph;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    public class Solver : ISolver
    {
        public string Part1(string input)
        {
            var lines = input.AsList();
            BrickStack brickStack = new BrickStack();

            foreach (var line in lines)
            {
                brickStack.AddBrick(new(line));
            }

            //Console.WriteLine(brickStack.PrintX());

            brickStack.AllFallDown();

            //Console.WriteLine(brickStack.PrintX());

            var nonSupportingBricks = brickStack.Bricks.Where(b => brickStack.CanDisintegrate(b));

            return nonSupportingBricks.Count().ToString();
        }

        public string Part2(string input)
        {
            var lines = input.AsList();
            BrickStack brickStack = new BrickStack();

            foreach (var line in lines)
            {
                brickStack.AddBrick(new(line));
            }

            //Console.WriteLine(brickStack.PrintX());

            brickStack.AllFallDown();

            //Console.WriteLine(brickStack.PrintX());

            var totalCascade = brickStack.CalcCascades();

            return totalCascade.ToString();
        }

        private class Brick : IEquatable<Brick>
        {
            public Coordinates Coords1;
            public Coordinates Coords2;
            public char? Name = null;

            public Brick(Coordinates coords1, Coordinates coords2, char? name)
            {
                Coords1 = coords1;
                Coords2 = coords2;
                Name = name;
            }

            public Brick(string input)
            {
                var corners = input.Split('~');
                var c1 = corners[0].Split(',');
                var c2 = corners[1].Split(',');

                Coords1 = new Coordinates(int.Parse(c1[0]), int.Parse(c1[1]), int.Parse(c1[2]));
                Coords2 = new Coordinates(int.Parse(c2[0]), int.Parse(c2[1]), int.Parse(c2[2]));

                if (c2.Length > 3)
                    Name = c2[3][0];
            }

            public void MoveDown(int dist = 1)
            {
                Coords1 = new Coordinates(Coords1.X, Coords1.Y, Coords1.Z - dist);
                Coords2 = new Coordinates(Coords2.X, Coords2.Y, Coords2.Z - dist);
            }

            public int LowestPoint()
            {
                return (int)Math.Min(Coords1.Z, Coords2.Z);
            }

            public int HighestPoint()
            {
                return (int)Math.Max(Coords1.Z, Coords2.Z);
            }

            public IEnumerable<Coordinates> FootPrint()
            {
                var z = LowestPoint();
                for (int x = (int)Math.Min(Coords1.X, Coords2.X); x <= Math.Max(Coords1.X, Coords2.X); x++)
                    for (int y = (int)Math.Min(Coords1.Y, Coords2.Y); y <= Math.Max(Coords1.Y, Coords2.Y); y++)
                        yield return new(x, y, z);
            }

            public IEnumerable<Coordinates> HeadPrint()
            {
                var z = HighestPoint();
                for (int x = (int)Math.Min(Coords1.X, Coords2.X); x <= Math.Max(Coords1.X, Coords2.X); x++)
                    for (int y = (int)Math.Min(Coords1.Y, Coords2.Y); y <= Math.Max(Coords1.Y, Coords2.Y); y++)
                        yield return new(x, y, z);
            }

            public bool Equals(Brick? other)
            {
                if (other is null) return false;
                if (ReferenceEquals(this, other)) return true;
                return Coords1 == other.Coords1 && Coords2 == other.Coords2;
            }

            public string ToString()
            {
                string name = Name.ToString() ?? "";
                return $"{Name.ToString() ?? ""}|{Coords1}~{Coords2}";
            }
        }

        private class BrickStack
        {
            public List<Brick> Bricks = [];

            public void AddBrick(Brick brick)
            {
                Bricks.Add(brick);
            }

            public void AllFallDown()
            {
                // Iterate over bricks from low to high and move them down until they settle
                foreach (var brick in Bricks.OrderBy(b => b.LowestPoint()))
                {
                    while (true)
                    {
                        if (brick.LowestPoint() == 1)
                            break;

                        //if (SupportingBricks(brick).Any())
                        var movedFootPrint = brick.FootPrint().Select(c => new Coordinates(c.X, c.Y, c.Z - 1));
                        var sameHeight = Bricks
                            .Where(b => b.HighestPoint() == movedFootPrint.First().Z);
                        var collisions = sameHeight
                            .Where(b => b.HeadPrint().Intersect(movedFootPrint).Any());
                        if (collisions.Any())
                            break;

                        brick.MoveDown();
                    }
                }
            }

            public IEnumerable<Brick> SupportedBricks(Brick brick)
            {
                var movedHeadPrint = brick.HeadPrint().Select(c => new Coordinates(c.X, c.Y, c.Z + 1));
                var sameHeight = Bricks
                            .Where(b => b.LowestPoint() == movedHeadPrint.First().Z);
                var collisions = sameHeight
                    .Where(b => b.FootPrint().Intersect(movedHeadPrint).Any());

                return collisions;
            }

            public IEnumerable<Brick> SupportingBricks(Brick brick)
            {
                var movedFootPrint = brick.FootPrint().Select(c => new Coordinates(c.X, c.Y, c.Z - 1));
                var sameHeight = Bricks
                            .Where(b => b.HighestPoint() == movedFootPrint.First().Z);
                var collisions = sameHeight
                    .Where(b => b.HeadPrint().Intersect(movedFootPrint).Any());

                return collisions;
            }

            public bool CanDisintegrate(Brick brick)
            {
                foreach (var supported in SupportedBricks(brick))
                {
                    if (SupportingBricks(supported).Count() == 1)
                        return false;
                }

                return true;
            }

            public int CalcCascades()
            {
                Dictionary<Brick, List<Brick>> supportedBy = [];

                foreach (var brick in Bricks.OrderBy(b => b.HighestPoint()))
                {
                    var supporters = SupportingBricks(brick).ToList();
                    supportedBy[brick] = supporters;
                }

                var res = 0;
                foreach (var brick in Bricks.OrderBy(b => b.HighestPoint()))
                {
                    var falls = new[] { brick }.ToHashSet();
                    foreach (var other in Bricks.Where(b => b.LowestPoint() > brick.HighestPoint()).OrderBy(b => b.HighestPoint()))
                    {
                        if (supportedBy[other].Any() && falls.IsSupersetOf(supportedBy[other]))
                            falls.Add(other);
                    }

                    res += falls.Count - 1;

                }

                return res;
            }

            public string PrintX()
            {
                StringBuilder sb = new StringBuilder();

                var minX = (int)Bricks.Min(b => Math.Min(b.Coords1.X, b.Coords2.X));
                var maxX = (int)Bricks.Max(b => Math.Min(b.Coords1.X, b.Coords2.X));

                for (var z = Bricks.Select(b => b.HighestPoint()).Max(); z > 0; z--)
                {
                    char[] c = new char[maxX - minX + 1];
                    for (int i = 0; i < c.Length; i++)
                        c[i] = '.';

                    foreach (var brick in Bricks.Where(b => b.HighestPoint() >= z && b.LowestPoint() <= z))
                        for (var x = Math.Min(brick.Coords1.X, brick.Coords2.X); x <= Math.Max(brick.Coords1.X, brick.Coords2.X); x++)
                            c[x] = c[x] != '.' ? '?' : brick.Name ?? 'X';
                    sb.Append(new string(c));
                    sb.AppendLine($" {z}");
                }
                sb.AppendLine($"{new string('-', maxX - minX + 1)} 0");

                return sb.ToString();
            }

            public string PrintY()
            {
                StringBuilder sb = new StringBuilder();

                var minY = (int)Bricks.Min(b => Math.Min(b.Coords1.Y, b.Coords2.Y));
                var maxY = (int)Bricks.Max(b => Math.Min(b.Coords1.Y, b.Coords2.Y));

                for (var z = Bricks.Select(b => b.HighestPoint()).Max(); z > 0; z--)
                {
                    char[] c = new char[maxY - minY + 1];
                    for (int i = 0; i < c.Length; i++)
                        c[i] = '.';

                    foreach (var brick in Bricks.Where(b => b.HighestPoint() >= z && b.LowestPoint() <= z))
                        for (var y = Math.Min(brick.Coords1.Y, brick.Coords2.Y); y <= Math.Max(brick.Coords1.Y, brick.Coords2.Y); y++)
                            c[y] = c[y] != '.' ? '?' : brick.Name ?? 'Y';
                    sb.Append(new string(c));
                    sb.AppendLine($" {z}");
                }
                sb.AppendLine($"{new string('-', maxY - minY + 1)} 0");

                return sb.ToString();
            }
        }
    }
}
