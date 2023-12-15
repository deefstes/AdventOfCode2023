namespace AdventOfCode2023.Day15
{
    using System.Collections.Generic;
    using System.Linq;

    public class Solver : ISolver
    {
        public string Part1(string input)
        {
            var inputs = input.Split(',');
            List<int> hashes = [];

            foreach(string str in inputs)
            {
                hashes.Add(FunnyHash(str));
            }

            return hashes.Sum().ToString();
        }

        public string Part2(string input)
        {
            var inputs = input.Split(',');
            Dictionary<long, List<Lens>> boxes = [];

            foreach (var step in inputs)
            {
                var label = new string(step.TakeWhile(c => char.IsLetter(c)).ToArray());
                var operation = step.Substring(label.Length, 1);
                var focalLen = step.Substring(label.Length + 1);

                var box = FunnyHash(label);
                Lens? item;
                switch (operation)
                {
                    case "-":
                        if (boxes.ContainsKey(box))
                        {
                            item = boxes[box].SingleOrDefault(i => i.Label == label);
                            if (item != null)
                            {
                                boxes[box].Remove(item);
                            }
                        }
                        break;
                    case "=":
                        if (!boxes.ContainsKey(box))
                        {
                            boxes.Add(box, []);
                        }
                        item = boxes[box].SingleOrDefault(i => i.Label == label);
                        if (item == null)
                        {
                            boxes[box].Add(new Lens { Label = label, Value = int.Parse(focalLen) });
                        }
                        else
                        {
                            item.Value = int.Parse(focalLen);
                        }
                        break;
                }
            }

            long power = 0;
            foreach (var box in boxes)
            {
                int i = 1;
                foreach (var item in box.Value)
                {
                    power += (box.Key + 1) * i * item.Value;
                    i++;
                }
            }

            return power.ToString();
        }

        private static int FunnyHash(string input)
        {
            var hash = 0;
            foreach(char c in input)
            {
                hash += c;
                hash *= 17;
                hash %= 256;
            }

            return hash;
        }
        private class Lens
        {
            public required string Label { get; set; }
            public int Value { get; set; }
        }
    }
}
