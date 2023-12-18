namespace AdventOfCode2023.Utils.Graph
{
    public class Vehicle(Coordinates coords, Direction dir)
    {
        public readonly Coordinates Coords = coords;
        public readonly Direction Dir = dir;

        public Vehicle Move(int steps = 1)
        {
            return new Vehicle(Coords.Move(Dir, steps), Dir);
        }

        public Vehicle TurnLeft(int halfSteps = 2)
        {
            return new Vehicle(Coords, Dir.TurnLeft(halfSteps));
        }

        public Vehicle TurnRight(int halfSteps = 2)
        {
            return new Vehicle(Coords, Dir.TurnRight(halfSteps));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Coords, Dir);
        }
    }
}
