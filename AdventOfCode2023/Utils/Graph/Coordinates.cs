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
