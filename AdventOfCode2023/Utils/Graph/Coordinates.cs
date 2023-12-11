namespace AdventOfCode2023.Utils.Graph
{
    public class Coordinates(int x, int y)
    {
        public readonly int X = x;
        public readonly int Y = y;

        public Coordinates Move(Direction direction)
        {
            return direction switch
            {
                Direction.North => new Coordinates(X, Y - 1),
                Direction.South => new Coordinates(X, Y + 1),
                Direction.East => new Coordinates(X + 1, Y),
                Direction.West => new Coordinates(X - 1, Y),
                Direction.NorthEast => new Coordinates(X + 1, Y - 1),
                Direction.NorthWest => new Coordinates(X - 1, Y - 1),
                Direction.SouthEast => new Coordinates(X + 1, Y + 1),
                Direction.SouthWest => new Coordinates(X - 1, Y + 1),
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
    }

    public enum Direction
    {
        North, South, East, West,
        NorthEast, NorthWest, SouthEast, SouthWest
    }
}
