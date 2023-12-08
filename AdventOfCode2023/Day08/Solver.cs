using AdventOfCode2023.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Day08
{
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

            List<NetworkNode> nextNodes = nodes.Where(n => n.Key.EndsWith("A")).Select(n=>n.Value).ToList();
            char nextStep = steps[0];
            var length = 0;

            while (nextNodes.Count(n=>!n.name.EndsWith("Z")) > 0)
            {
                length++;
                List<NetworkNode> newNodes = [];

                foreach (NetworkNode nextNode in nextNodes)
                {
                    var nn = nodes[nextNode.neighbours[nextStep]];
                    newNodes.Add(nn);
                }
                nextStep = steps[length % steps.Count];
                nextNodes = newNodes;
            }

            return length.ToString();
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
