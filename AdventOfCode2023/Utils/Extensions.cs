namespace AdventOfCode2023.Utils
{
    using AdventOfCode2023.Utils.Graph;
    using System.Collections.Generic;

    public static class ExtensionMethods
    {
        public static List<Coordinates> Offset(this List<Coordinates> path, Coordinates? delta = null)
        {
            List<Coordinates> result = [];

            delta ??= new(0, 0);
            if (delta.IsZero())
            {
                var deltaX = -path.Min(n=>n.X);
                var deltaY = -path.Min(n=>n.Y);

                delta = new Coordinates(deltaX, deltaY);
            }

            var xDir = delta.X > 0 ? Direction.East : Direction.West;
            var yDir = delta.Y > 0 ? Direction.South : Direction.North;

            foreach (var node in path)
            {
                result.Add(node.Move(xDir, delta.X).Move(yDir, delta.Y));
            }

            return result;
        }

        public static List<Coordinates> GetEnclosed(this List<Coordinates> path, int gridWidth, int gridHeight)
        {
            List<Coordinates> toBeChecked = [];
            List<Coordinates> enclosed = [];

            for (int x = 0; x < gridWidth; x++)
                for (int y = 0; y < gridHeight; y++)
                {
                    var testCell = new Coordinates(x, y);
                    if (path.Contains(testCell))
                        continue;

                    // Count path crossing to left margin
                    var crossings = 0;
                    var movedCell = testCell;
                    for (int i = x; i >= 0; i--)
                    {
                        var onPath = path.Contains(movedCell);
                        movedCell = movedCell.Move(Direction.West);
                        if (!onPath && path.Contains(movedCell))
                            crossings++;
                    }

                    if (crossings%2 == 1)
                        enclosed.Add(testCell);
                }

            return enclosed;
        }

        public static long CountOutside(this List<Coordinates> path, long gridWidth, long gridHeight)
        {
            var outside = FloodFill(path, new((int)gridWidth-1, (int)gridHeight-1), gridWidth, gridHeight);
            return outside.Count();
        }

        public static List<Coordinates> FloodFill(this List<Coordinates> path, Coordinates start, long gridWidth, long gridHeight)
        {
            var queue = new Queue<Coordinates>();
            var used = new Dictionary<Coordinates, SearchPathItem<Coordinates>>();

            queue.Enqueue(start);
            used.Add(start, new SearchPathItem<Coordinates>(start, 0, null));

            List<Coordinates> result = [];

            while (queue.Count > 0)
            {
                var cur = queue.Dequeue();
                var curItem = used[cur];

                result.Add(curItem.State);

                if (curItem.Distance >= long.MaxValue)
                    continue;

                foreach (var next in cur.NeighboursCardinal().Where(n=>!path.Contains(n)).Where(n=>n.InBounds((int)gridWidth, (int)gridHeight)))
                {
                    if (used.ContainsKey(next))
                        continue;

                    used.Add(next, new SearchPathItem<Coordinates>(next, curItem.Distance + 1, curItem));
                    queue.Enqueue(next);
                }
            }

            return result;
        }

        public record SearchPathItem<TState>(TState State, long Distance, SearchPathItem<TState>? Prev)
        {
            public IEnumerable<TState> PathBack()
            {
                for (var c = this; c != null; c = c.Prev)
                    yield return c.State;
            }

            public IEnumerable<TState> Path() => PathBack().Reverse();
        }

        /// <summary>
        /// This function uses the coordinate method for traverse area calculation.
        /// See the last method in the following article:
        /// http://gis.washington.edu/phurvitz/courses/esrm304/lectures/2009/Hurvitz/procedures/area.html
        /// </summary>
        /// <param name="points">
        /// List of coordinates describing the polygon. Make sure the loop is closed, ie. the first
        /// and last points in the list should be identical.
        /// </param>
        /// <returns>Area subscribed by polygon including the unit width of the polygon border</returns>
        public static long TraverseAreaCalc(this List<Coordinates> points)
        {
            var solids = 0L;
            var dashes = 0L;
            var border = 0L;
            for (int i = 0; i < points.Count - 1; i++)
            {
                solids += (long)points[i].X * points[i + 1].Y;
                dashes += (long)points[i].Y * points[i + 1].X;
                border += points[i].ManhattanDistanceTo(points[i + 1]);
            }

            return (Math.Abs(solids - dashes) + border) / 2 + 1;
        }
    }
}
