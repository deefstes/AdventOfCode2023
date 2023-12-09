namespace AdventOfCode2023.Day08
{
    using AdventOfCode2023.Utils;

    public class Solver : ISolver
    {
        public string Part1(string input)
        {
            var steps = input.AsList()[0].ToList();

            Dictionary<string, NetworkNode> nodes = []; 

            foreach (string line in input.AsList().Skip(2))
            {
                var lineParts = line.Replace("(", "").Replace(")", "").Replace(" ", "").Split('=');
                var neighbours = lineParts[1].Split(',');

                nodes[lineParts[0]] = new NetworkNode(lineParts[0], neighbours[0], neighbours[1]);
            }

            NetworkNode nextNode = nodes["AAA"];
            char nextStep = steps[0];
            var length = 0;

            while (nextNode.name != "ZZZ")
            {
                length++;
                nextNode = nodes[nextNode.neighbours[nextStep]];
                nextStep = steps[length % steps.Count];
            }

            return length.ToString();
        }

        public string Part2(string input)
        {
            var steps = input.AsList()[0].ToList();

            Dictionary<string, NetworkNode> nodes = [];

            foreach (string line in input.AsList().Skip(2))
            {
                var lineParts = line.Replace("(", "").Replace(")", "").Replace(" ", "").Split('=');
                var neighbours = lineParts[1].Split(',');

                nodes[lineParts[0]] = new NetworkNode(lineParts[0], neighbours[0], neighbours[1]);
            }

            List<NetworkNode> nextNodes = nodes.Where(n => n.Key.EndsWith('A')).Select(n=>n.Value).ToList();
            long[] values = new long[nextNodes.Count];

            for (int nodeInd = 0; nodeInd < nextNodes.Count; nodeInd++)
            {
                char nextStep = steps[0];
                var length = 0;

                while (!nextNodes[nodeInd].name.EndsWith('Z'))
                {
                    length++;
                    nextNodes[nodeInd] = nodes[nextNodes[nodeInd].neighbours[nextStep]];
                    nextStep = steps[length % steps.Count];
                }

                values[nodeInd] = length;
            }

            return Utils.Lcm(values).ToString();
        }

        private class NetworkNode
        {
            public string name;
            public Dictionary<char, string> neighbours = [];

            public NetworkNode(string name, string left, string right)
            {
                this.name = name;
                neighbours['L'] = left;
                neighbours['R'] = right;
            }
        }
    }
}
