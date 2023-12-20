namespace AdventOfCode2023.Day19
{
    using AdventOfCode2023.Utils;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Solver : ISolver
    {
        public string Part1(string input)
        {
            Dictionary<string, Workflow> workFlows = [];
            List<Part> parts = [];
            List<Part> accepted = [];
            List<Part> rejected = [];

            foreach (var line in input.AsList().Where(s => !string.IsNullOrEmpty(s)))
            {
                if (line.StartsWith('{'))
                    parts.Add(new Part(line));
                else
                {
                    var wf = new Workflow(line);
                    workFlows[wf.Name] = wf;
                }
            }

            foreach (var part in parts)
            {
                var nextWorkFlow = "in";
                while (nextWorkFlow != "A" && nextWorkFlow != "R")
                {
                    nextWorkFlow = workFlows[nextWorkFlow].Process(part);
                    switch (nextWorkFlow)
                    {
                        case "A":
                            accepted.Add(part); break;
                        case "R":
                            rejected.Add(part); break;
                    }
                }
            }

            return accepted.Sum(p => p.Ratings.Sum(r => r.Value)).ToString();
        }

        public string Part2(string input)
        {
            Dictionary<string, Workflow> workFlows = [];
            List<Part> parts = [];
            List<Part> accepted = [];
            List<Part> rejected = [];

            foreach (var line in input.AsList().Where(s => !string.IsNullOrEmpty(s)))
            {
                if (line.StartsWith('{'))
                    parts.Add(new Part(line));
                else
                {
                    var wf = new Workflow(line);
                    workFlows[wf.Name] = wf;
                }
            }

            var validRanges = FindAccepted(new RangePart(), workFlows, workFlows["in"], 0);
            validRanges = validRanges.DistinctBy(r => r.ToString()).ToList();

            var total = 0L;
            foreach (var v in validRanges)
            {
                var x = v.Ratings['x'].high - v.Ratings['x'].low + 1;
                var m = v.Ratings['m'].high - v.Ratings['m'].low + 1;
                var a = v.Ratings['a'].high - v.Ratings['a'].low + 1;
                var s = v.Ratings['s'].high - v.Ratings['s'].low + 1;
                total += (long)x * m * a * s;
            }

            return total.ToString();
        }

        private List<RangePart> FindAccepted(RangePart rangePart, Dictionary<string, Workflow> workFlows, Workflow workflow, int depth)
        {
            depth++;
            List<RangePart> validParts = [];

            foreach (var rule in workflow.Rules)
            {
                var ratings = rangePart.Ratings.ToDictionary(entry => entry.Key, entry => entry.Value);
                switch (rule.Operator)
                {
                    case '>':
                        ratings[rule.Category] = (rule.Value + 1, ratings[rule.Category].high);
                        break;
                    case '<':
                        ratings[rule.Category] = (ratings[rule.Category].low, rule.Value - 1);
                        break;
                    case '=':
                        if (rule.Destination == "A")
                            validParts.Add(rangePart);
                        break;
                }

                if (rule.Category != 'Z' && ratings[rule.Category].low >= ratings[rule.Category].high)
                    break;

                var tempPart = new RangePart();
                tempPart.Ratings = ratings;
                switch (rule.Destination)
                {
                    case "A":
                        validParts.Add(tempPart);
                        break;
                    case "R":
                        break;
                    default:
                        validParts.AddRange(FindAccepted(tempPart, workFlows, workFlows[rule.Destination], depth));
                        break;
                }

                switch (rule.Operator)
                {
                    case '>':
                        rangePart.Ratings[rule.Category] = (rangePart.Ratings[rule.Category].low, rule.Value);
                        break;
                    case '<':
                        rangePart.Ratings[rule.Category] = (rule.Value, rangePart.Ratings[rule.Category].high);
                        break;
                }
            }

            return validParts;
        }

        private class Workflow
        {
            public string Name;
            public List<WorkflowRule> Rules = [];

            public Workflow(string input)
            {
                var elems = input.Split('{', StringSplitOptions.RemoveEmptyEntries);
                Name = elems[0];

                foreach (var rule in elems[1].TrimEnd('}').Split(','))
                    Rules.Add(new WorkflowRule(rule));
            }

            public string Process(Part part)
            {
                foreach (var rule in Rules)
                {
                    if (rule.Satisfy(part))
                        return rule.Destination;
                }

                throw new ArgumentException();
            }
        }

        private class WorkflowRule
        {
            public char Category;
            public char Operator;
            public int Value;
            public string Destination;

            public WorkflowRule(string input)
            {
                var split1 = input.Split(new char[] { '<', '>' });
                if (split1.Length == 1)
                {
                    Category = 'Z';
                    Operator = '=';
                    Value = 0;
                    Destination = input;
                }
                else
                {
                    var split2 = split1[1].Split(':');

                    Category = split1[0][0];
                    Operator = input[1];
                    Value = int.Parse(split2[0]);
                    Destination = split2[1];
                }
            }

            public bool Satisfy(Part part)
            {
                if (Category == 'Z')
                    return true;

                if (part.Ratings[Category] == -1)
                    return true;

                switch (Operator)
                {
                    case '>':
                        if (part.Ratings[Category] > Value)
                            return true;
                        break;
                    case '<':
                        if (part.Ratings[Category] < Value)
                            return true;
                        break;
                }

                return false;
            }
        }

        private class Part
        {
            public Dictionary<char, int> Ratings = [];

            public Part(string input)
            {
                var elems = input.TrimStart('{').TrimEnd('}').Split(',');
                foreach (var a in elems)
                {
                    var attr = a.Split('=')[0][0];
                    var val = int.Parse(a.Split('=')[1]);
                    Ratings[attr] = val;
                }
            }
        }

        private class RangePart
        {
            public Dictionary<char, (int low, int high)> Ratings = [];

            public RangePart()
            {
                Ratings['x'] = (1, 4000);
                Ratings['m'] = (1, 4000);
                Ratings['a'] = (1, 4000);
                Ratings['s'] = (1, 4000);
            }

            public new string ToString()
            {
                return $"x:{Ratings['x'].low}-{Ratings['x'].high}, m:{Ratings['m'].low}-{Ratings['m'].high}, a:{Ratings['a'].low}-{Ratings['a'].high}, s:{Ratings['s'].low}-{Ratings['s'].high}, ";
            }
        }

    }
}
