namespace AdventOfCode2023.Utils
{
    using AdventOfCode2023.Utils.Graph;
    using System.Collections.Generic;
    using System.Text;

    public static class ExtensionMethods
    {
        public static string Format(this TimeSpan timeSpan)
        {
            if ((int)timeSpan.TotalHours > 0)
                return $"{(int)timeSpan.TotalHours}h " +
                       $"{timeSpan.Minutes}m " +
                       $"{timeSpan.Seconds}.{timeSpan.Milliseconds:D3}s";

            if ((int)timeSpan.TotalMinutes > 0)
                return $"{timeSpan.Minutes}m " +
                       $"{timeSpan.Seconds}.{timeSpan.Milliseconds:D3}s";

            if ((int)timeSpan.TotalSeconds > 0)
                return $"{timeSpan.Seconds}.{timeSpan.Milliseconds:D3}s";

            if ((int)timeSpan.TotalMilliseconds > 0)
                return $"{timeSpan.Milliseconds}.{timeSpan.Microseconds:D3}ms";

            return $"{timeSpan.Microseconds}μs";
        }

        /// <summary>
        /// Convert a string containing newlines to IList<string>, dropping all leading and trailing whitespace in the list and also in each individual string in the list
        /// </summary>
        /// <param name="input">string containing newlines</param>
        /// <returns>IList<string></returns>
        public static IList<string> AsList(this string input)
        {
            return input
                .Trim()
                .Replace("\r", "")
                .Split('\n')
                .ToList()
                .Select(s => s.Trim())
                .ToList();
        }

        public static char[,] AsGrid(this string input)
        {
            var lines = input.AsList();
            var height = lines.Count;
            var width = lines.Max(s => s.Length);

            var grid = new char[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    grid[x, y] = lines[y][x];
                }
            }

            return grid;
        }

        public static int[,] AsIntGrid(this string input)
        {
            var lines = input.AsList();
            var height = lines.Count;
            var width = lines.Max(s => s.Length);

            var grid = new int[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    grid[x, y] = lines[y][x] - '0';
                }
            }

            return grid;
        }

        public static string ColToString(this char[,] grid, int col)
        {
            StringBuilder sb = new();
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                sb.Append(grid[col, y]);
            }

            return sb.ToString();
        }

        public static string RowToString(this char[,] grid, int row)
        {
            StringBuilder sb = new();
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                sb.Append(grid[x, row]);
            }

            return sb.ToString();
        }

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
                result.Add(node.Move(xDir, (int)delta.X).Move(yDir, (int)delta.Y));
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

        /// <summary>
        /// Topological Sorting (Kahn's algorithm) 
        /// </summary>
        /// <remarks>https://en.wikipedia.org/wiki/Topological_sorting</remarks>
        /// <param name="graph">Directed acyclic graph</param>
        /// <returns>Sorted nodes in topological order.</returns>
        public static List<TNode> TopologicalSort<TNode>(this WeightedGraph<TNode> graph) where TNode : IComparable<TNode>, IEquatable<TNode>
        {
            HashSet<TNode> nodes = [];
            foreach (var node in graph.Nodes())
                nodes.Add(node);

            HashSet<(TNode, TNode)> edges = [];
            foreach (var connection in graph.Connections())
                edges.Add((connection.Item1, connection.Item2));

            // Empty list that will contain the sorted elements
            var L = new List<TNode>();

            // Set of all nodes with no incoming edges
            var S = new HashSet<TNode>(nodes.Where(n => edges.All(e => e.Item2.Equals(n) == false)));

            // while S is non-empty do
            while (S.Count != 0)
            {
                //  remove a node n from S
                var n = S.First();
                S.Remove(n);

                // add n to tail of L
                L.Add(n);

                // for each node m with an edge e from n to m do
                foreach (var e in edges.Where(e => e.Item1.Equals(n)).ToList())
                {
                    var m = e.Item2;

                    // remove edge e from the graph
                    edges.Remove(e);

                    // if m has no other incoming edges then
                    if (edges.All(me => me.Item2.Equals(m) == false))
                    {
                        // insert m into S
                        S.Add(m);
                    }
                }
            }

            // if graph has edges then
            if (edges.Count != 0)
            {
                // return error (graph has at least one cycle)
                return [];
            }
            else
            {
                // return L (a topologically sorted order)
                return L;
            }
        }

        /// <summary>
        /// Topological Sorting (Kahn's algorithm) 
        /// </summary>
        /// <remarks>https://en.wikipedia.org/wiki/Topological_sorting</remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="nodes">All nodes of directed acyclic graph.</param>
        /// <param name="edges">All edges of directed acyclic graph.</param>
        /// <returns>Sorted nodes in topological order.</returns>
        public static List<T> TopologicalSort<T>(HashSet<T> nodes, HashSet<(T, T)> edges) where T : IEquatable<T>
        {
            // Empty list that will contain the sorted elements
            var L = new List<T>();

            // Set of all nodes with no incoming edges
            var S = new HashSet<T>(nodes.Where(n => edges.All(e => e.Item2.Equals(n) == false)));

            // while S is non-empty do
            while (S.Any())
            {

                //  remove a node n from S
                var n = S.First();
                S.Remove(n);

                // add n to tail of L
                L.Add(n);

                // for each node m with an edge e from n to m do
                foreach (var e in edges.Where(e => e.Item1.Equals(n)).ToList())
                {
                    var m = e.Item2;

                    // remove edge e from the graph
                    edges.Remove(e);

                    // if m has no other incoming edges then
                    if (edges.All(me => me.Item2.Equals(m) == false))
                    {
                        // insert m into S
                        S.Add(m);
                    }
                }
            }

            // if graph has edges then
            if (edges.Any())
            {
                // return error (graph has at least one cycle)
                return null;
            }
            else
            {
                // return L (a topologically sorted order)
                return L;
            }
        }
    }
}
