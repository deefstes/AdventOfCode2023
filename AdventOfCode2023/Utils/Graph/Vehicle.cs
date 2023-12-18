namespace AdventOfCode2023.Utils.Graph
{
    public class Vehicle(Coordinates coords, Direction dir, List<Coordinates> path)
    {
        public readonly Coordinates Coords = coords;
        public readonly Direction Dir = dir;
        public readonly List<Coordinates> Path = path;

        public Vehicle Move(long steps = 1)
        {
            var pos = Coords;
            for (long i = 0; i < steps; i++)
            {
                pos = pos.Move(Dir);
                if (!Path.Contains(pos))
                    Path.Add(pos);
            }
            return new Vehicle(pos, Dir, Path);
        }

        public Vehicle TurnLeft(int halfSteps = 2)
        {
            return new Vehicle(Coords, Dir.TurnLeft(halfSteps), Path);
        }

        public Vehicle TurnRight(int halfSteps = 2)
        {
            return new Vehicle(Coords, Dir.TurnRight(halfSteps), Path);
        }

        public Vehicle SetDirection(Direction direction)
        {
            return new Vehicle(Coords, direction, Path);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Coords, Dir);
        }
    }
}
