namespace AdventOfCode2023.Day24
{
    using AdventOfCode2023.Utils;
    using AdventOfCode2023.Utils.Graph;
    using MathNet.Numerics.LinearAlgebra;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Solver : ISolver
    {
        public string Part1(string input)
        {
            var lines = input.AsList();
            List<HailStone> hailStones = [];
            foreach(var line in lines)
            {
                hailStones.Add(new HailStone(line, ignoreZ: true));
            }

            var xRange = hailStones.Count > 5 ? (200000000000000, 400000000000000) : (7L, 27L);
            var intersectionCount = 0;

            for (int i = 0; i < hailStones.Count; i++)
            {
                var hailStone1 = hailStones[i];
                for (int j = i+1; j < hailStones.Count; j++)
                { 
                    var hailStone2 = hailStones[j];

                    if (hailStone1.WillCollide2D(hailStone2, xRange.Item1, xRange.Item2))
                        intersectionCount++;
                }
            }

            return intersectionCount.ToString();
        }

        public string Part2(string input)
        {
            var lines = input.AsList();
            List<HailStone> hailStones = [];
            foreach (var line in lines)
            {
                hailStones.Add(new HailStone(line));
            }

            var (timeDelta1, x1, y1, z1) = FindIntersection(hailStones[0], hailStones[1], hailStones[2]);
            var (timeDelta2, x2, y2, z2) = FindIntersection(hailStones[0], hailStones[1], hailStones[3]);

            var velocityX = Math.Round((x2 - x1) / (timeDelta2 - timeDelta1));
            var velocityY = Math.Round((y2 - y1) / (timeDelta2 - timeDelta1));
            var velocityZ = Math.Round((z2 - z1) / (timeDelta2 - timeDelta1));

            var x = x1 - timeDelta1 * velocityX;
            var y = y1 - timeDelta1 * velocityY;
            var z = z1 - timeDelta1 * velocityZ;

            var total = (long)(x + y + z);
            return total.ToString();
        }

        private class HailStone
        {
            public Coordinates Position;
            public Coordinates Velocity;
            /// <summary>
            /// Coefficients (m and c) in linear equation f(t) = m.t + c
            /// </summary>
            public readonly (double, double, double)[] TimeFuncCoeficients;

            public HailStone(string input, bool ignoreZ = false)
            {
                var e = input.Replace(" ", "").Split('@');
                var p = e[0].Split(',').Select(long.Parse).ToList();
                var v = e[1].Split(',').Select(long.Parse).ToList();

                Position = new(p[0], p[1], ignoreZ ? 0 : p[2]);
                Velocity = new(v[0], v[1], ignoreZ ? 0 : v[2]);

                TimeFuncCoeficients = new (double, double, double)[2];
                var pX = Utils.DiscoverPolynomial([(Position.X, 0), (Position.X + Velocity.X, 1)]);
                var pY = Utils.DiscoverPolynomial([(Position.Y, 0), (Position.Y + Velocity.Y, 1)]);
                var pZ = Utils.DiscoverPolynomial([(Position.Z, 0), (Position.Z + Velocity.Z, 1)]);

                TimeFuncCoeficients[0] = (pX[0], pY[0], pZ[0]); // m
                TimeFuncCoeficients[1] = (pX[1], pY[1], pZ[1]); // c
            }

            public float SlopeNoZ() => (float)Velocity.Y / (float)Velocity.X;

            public bool WillCollide2D(HailStone other, long minTest, long maxTest)
            {
                var slope = SlopeNoZ();
                var otherSlope = other.SlopeNoZ();

                if (slope == otherSlope) return false;  //parallel case

                var commonX = ((otherSlope * other.Position.X) - (slope * Position.X) + Position.Y - other.Position.Y) / (otherSlope - slope);
                if (commonX < minTest || commonX > maxTest) return false;

                var commonY = (slope * (commonX - Position.X)) + Position.Y;
                if (commonY < minTest || commonY > maxTest) return false;

                return IsFuture(commonX, commonY) && other.IsFuture(commonX, commonY);
            }

            public bool IsFuture(float x, float y)
            {
                if (Velocity.X < 0 && Position.X < x) return false;
                if (Velocity.X > 0 && Position.X > x) return false;
                if (Velocity.Y < 0 && Position.Y < y) return false;
                if (Velocity.Y > 0 && Position.Y > y) return false;

                return true;
            }
        }

        (double timeDelta, double x, double y, double z) FindIntersection(HailStone ref1, HailStone ref2, HailStone target)
        {
            var displacementFirst = ref2.Position - ref1.Position;
            var displacementSecond = target.Position - ref1.Position;
            var velocityDiffFirst = ref2.Velocity - ref1.Velocity;
            var velocityDiffSecond = target.Velocity - ref1.Velocity;

            // Find intersection of plane and line:
            // https://math.stackexchange.com/a/3584405

            Matrix<double> originalMatrix = Matrix<double>.Build.DenseOfArray(new double[,]
            {
                { displacementFirst.X, velocityDiffFirst.X, -velocityDiffSecond.X },
                { displacementFirst.Y, velocityDiffFirst.Y, -velocityDiffSecond.Y },
                { displacementFirst.Z, velocityDiffFirst.Z, -velocityDiffSecond.Z }
            });
            Vector<double> vect = Vector<double>.Build.DenseOfArray([displacementSecond.X, displacementSecond.Y, displacementSecond.Z]);

            var inverseMatrix = originalMatrix.Inverse();
            var multiplied = inverseMatrix.Multiply(vect);

            var t2 = multiplied[2];
            var posAt2x = target.Position.X + t2 * target.Velocity.X;
            var posAt2y = target.Position.Y + t2 * target.Velocity.Y;
            var posAt2z = target.Position.Z + t2 * target.Velocity.Z;
            return (t2, posAt2x, posAt2y, posAt2z);
        }
    }
}
