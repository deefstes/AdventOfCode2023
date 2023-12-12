namespace AdventOfCode2023.Day01
{
    using AdventOfCode2023.Utils;

    public class Solver : ISolver
    {
        public string Part1(string input)
        {
            return GetValues(input, false).Sum().ToString();
        }

        public string Part2(string input)
        {
            return GetValues(input, true).Sum().ToString();
        }
        static List<int> GetValues(string input, bool includeWords)
        {
            var vals = new List<int>();

            foreach (var line in input.AsList())
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                List<int> digits = ExtractDigits(line, includeWords);

                vals.Add(digits.First() * 10 + digits.Last());
            }

            return vals;
        }

        static List<int> ExtractDigits(string input, bool includeWords)
        {
            List<int> digits = [];

            while (input.Length > 0)
            {
                var d = ExtractDigitFromFront(ref input, includeWords);
                if (d >= 0)
                    digits.Add(d);
            }

            return digits;
        }

        static int ExtractDigitFromFront(ref string input, bool includeWords)
        {
            input = input.ToLower();

            if (char.IsDigit(input[0]))
            {
                int d = input[0] - '0';
                input = input[1..];
                return d;
            }

            if (includeWords)
            {
                foreach (KeyValuePair<string, int> digit in DigitNames)
                {
                    if (input.StartsWith(digit.Key))
                    {
                        input = input[1..];
                        return digit.Value;
                    }
                }
            }

            input = input[1..];
            return -1;
        }

        static readonly Dictionary<string, int> DigitNames = new()
        {
            {"one", 1},
            {"two", 2},
            {"three", 3},
            {"four", 4},
            {"five", 5},
            {"six", 6},
            {"seven", 7},
            {"eight", 8},
            {"nine", 9}
        };
    }
}
