namespace AdventOfCode2023.Day25
{
    using AdventOfCode2023.Utils;
    using AdventOfCode2023.Utils.Graph;
    using AdventOfCode2023.Utils.Pathfinding;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    public class Solver : ISolver
    {
        public string Part1(string input)
        {
            var lines = input.AsList();
            WeightedGraph<string> graph = new(directed: false);
            foreach (var line in lines)
            {
                var elems = line.Split(':');
                var node = elems[0];
                foreach (string otherNode in elems[1].Split(' ', StringSplitOptions.RemoveEmptyEntries))
                {
                    graph.AddConnection(node, otherNode, 1);
                }
            }

            Dictionary<string, int> traffic = [];
            Random r = new(1);
            var nodes = graph.Nodes().Order().ToArray();
            for (int i = 0; i < 100; i++)
            {
                var nodeStart = nodes[r.Next(nodes.Length)];
                var nodeFinish = nodes[r.Next(nodes.Length)];
                BreadthFirst<string> bfs = new BreadthFirst<string>(graph, nodeStart, nodeFinish, true);
                for (int step=1; step<bfs.Path.Count; step++)
                {
                    var key = CombineStrings(bfs.Path[step - 1], bfs.Path[step]);
                    if (traffic.TryGetValue(key, out int value))
                        traffic[key] = ++value;
                    else
                        traffic[key] = 1;
                }
            }

            var cuts = traffic.OrderByDescending(x => x.Value).Take(3);
            foreach (var cut in cuts)
                graph.DeleteConnection(cut.Key.Split(',')[0], cut.Key.Split(',')[1]);

            var newBfs = new BreadthFirst<string>(graph, cuts.First().Key.Split(',')[0], "xxx");
            var group1 = newBfs.DistancesMap.Count();
            var group2 = nodes.Count() - group1;

            return (group1 * group2).ToString();
        }

        public string Part2(string input)
        {
            return "No Part 2";
        }

        public static string CombineStrings(string str1, string str2)
        {
            var strings = new List<string>() { str1, str2 }.Order();

            return $"{strings.First()},{strings.Last()}";
        }
    }
}
