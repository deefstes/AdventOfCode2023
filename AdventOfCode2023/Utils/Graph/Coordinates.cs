namespace AdventOfCode2023.Utils.Graph
{
    public class Coordinates(int x, int y)
    {
        public readonly int X = x;
        public readonly int Y = y;

        public bool IsZero()
        {
            return X == 0 && Y == 0;
        }

        public Coordinates Move(Direction direction, int distance = 1)
        {
            return direction switch
            {
                Direction.North => new Coordinates(X, Y - distance),
                Direction.South => new Coordinates(X, Y + distance),
                Direction.East => new Coordinates(X + distance, Y),
                Direction.West => new Coordinates(X - distance, Y),
                Direction.NorthEast => new Coordinates(X + distance, Y - distance),
                Direction.NorthWest => new Coordinates(X - distance, Y - distance),
                Direction.SouthEast => new Coordinates(X + distance, Y + distance),
                Direction.SouthWest => new Coordinates(X - distance, Y + distance),
                _ => throw new Exception("Invalid direction"),
            };
        }

        public bool InBounds(int width, int height)
        {
            if (X < 0)
                return false;
            if (Y < 0)
                return false;
            if (X >= width)
                return false;
            if (Y >= height)
                return false;

            return true;
        }

        public bool TouchesBorder(int width, int height)
        {
            return !(Move(Direction.North).InBounds(width, height) &&
                Move(Direction.East).InBounds(width, height) &&
                Move(Direction.South).InBounds(width, height) &&
                Move(Direction.West).InBounds(width, height)
                );
        }

        public Direction? DirectionTo(Coordinates other)
        {
            foreach (Direction dir in new[] {
                Direction.North,
                Direction.South,
                Direction.East,
                Direction.West,
                Direction.NorthEast,
                Direction.NorthWest,
                Direction.SouthEast,
                Direction.SouthWest})
            {
                if (this.Move(dir).Equals(other))
                    return dir;
            }

            return null;
        }

        public int ManhattanDistanceTo(Coordinates other)
        {
            return Math.Abs(other.X - X) + Math.Abs(other.Y - Y);
        }

        public int EuclidianDistanceTo(Coordinates other)
        {
            var dx = Math.Abs(other.X - X);
            var dy = Math.Abs(other.Y - Y);

            return (int)Math.Sqrt(dx * dx + dy * dy);
        }

        public List<Coordinates> Neighbours()
        {
            List<Coordinates> neighbours = [];

            neighbours.AddRange(NeighboursCardinal());
            neighbours.AddRange(NeighboursOrdinal());

            return neighbours;
        }

        public List<Coordinates> Neighbours(List<Direction> directions)
        {
            List<Coordinates> neighbours = [];

            foreach (Direction dir in directions)
                neighbours.Add(Move(dir));

            return neighbours;
        }

        public List<Coordinates> NeighboursCardinal()
        {
            List<Coordinates> neighbours = [];

            neighbours.Add(Move(Direction.North));
            neighbours.Add(Move(Direction.East));
            neighbours.Add(Move(Direction.South));
            neighbours.Add(Move(Direction.West));

            return neighbours;
        }

        public List<Coordinates> NeighboursOrdinal()
        {
            List<Coordinates> neighbours = [];

            neighbours.Add(Move(Direction.NorthEast));
            neighbours.Add(Move(Direction.NorthWest));
            neighbours.Add(Move(Direction.SouthEast));
            neighbours.Add(Move(Direction.SouthWest));

            return neighbours;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if (obj is Coordinates other)
            {
                return X == other.X && Y == other.Y;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }

    public enum Direction
    {
        North, South, East, West,
        NorthEast, NorthWest, SouthEast, SouthWest
    }

    public static class Extensions
    {
        public static Direction TurnLeft(this Direction dir, int halfSteps = 2)
        {
            if (halfSteps == 1)
            {
                switch (dir)
                {
                    case Direction.North: return Direction.NorthWest;
                    case Direction.NorthWest: return Direction.West;
                    case Direction.West: return Direction.SouthWest;
                    case Direction.SouthWest: return Direction.South;
                    case Direction.South: return Direction.SouthEast;
                    case Direction.SouthEast: return Direction.East;
                    case Direction.East: return Direction.NorthEast;
                    case Direction.NorthEast: return Direction.North;
                }
            }

            return dir.TurnLeft(halfSteps - 1).TurnLeft(1);
        }
        public static Direction TurnRight(this Direction dir, int halfSteps = 2)
        {
            if (halfSteps == 1)
            {
                switch (dir)
                {
                    case Direction.North: return Direction.NorthEast;
                    case Direction.NorthWest: return Direction.North;
                    case Direction.West: return Direction.NorthWest;
                    case Direction.SouthWest: return Direction.West;
                    case Direction.South: return Direction.SouthWest;
                    case Direction.SouthEast: return Direction.South;
                    case Direction.East: return Direction.SouthEast;
                    case Direction.NorthEast: return Direction.East;
                }
            }

            return dir.TurnRight(halfSteps - 1).TurnRight(1);
        }
    }
}
