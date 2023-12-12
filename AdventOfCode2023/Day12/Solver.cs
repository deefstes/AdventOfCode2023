namespace AdventOfCode2023.Day12
{
    using AdventOfCode2023.Utils;

    public class Solver : ISolver
    {
        public string Part1(string input)
        {
            var lines = input.AsList();

            var total = 0L;
            foreach(var line in lines)
            {
                var parts = line.Split(' ');
                var springs = parts[0];
                var numbers = parts[1];

                var damagedGroups = numbers.Split(',').Select(int.Parse).ToArray();

                total += CountSolutions(springs, damagedGroups);
            }

            return total.ToString();
        }

        public string Part2(string input)
        {
            var lines = input.AsList();

            var total = 0L;
            foreach (var line in lines)
            {
                var parts = line.Split(' ');
                var springs = Unfold(parts[0], '?', 5);
                var numbers = Unfold(parts[1], ',', 5);

                var damagedGroups = numbers.Split(',').Select(int.Parse).ToArray();

                total += CountSolutions(springs, damagedGroups);
            }

            return total.ToString();
        }
        private static long CountSolutions(string springsLine, int[] damagedGroups)
        {
            return RecursiveCount(springsLine, damagedGroups, springsLine.Length, damagedGroups.Length, 0, []);
        }

        private static long RecursiveCount(string springsLine, int[] damagedGroups, int linePos, int damagedGroupsPos, int consumed, Dictionary<(int, int, int), long> cache)
        {
            if (linePos == 0)
                return damagedGroupsPos == 0 || damagedGroupsPos == 1 && consumed == damagedGroups[damagedGroupsPos - 1] ? 1 : 0;

            // If current state has already been calculated, return cached result
            if (cache.TryGetValue((linePos, damagedGroupsPos, consumed), out var cached))
                return cached;

            // Calculate result if last spring is damaged or assumed to be damaged
            var resultIfDamaged = 0L;
            if (springsLine[linePos - 1] == '#' ||
                springsLine[linePos - 1] == '?')
            {
                if (damagedGroupsPos == 0 || consumed == damagedGroups[damagedGroupsPos - 1])
                    resultIfDamaged = 0;
                else
                    resultIfDamaged = RecursiveCount(springsLine, damagedGroups, linePos - 1, damagedGroupsPos, consumed + 1, cache);
            };

            // Calculate result if last spring is operational or assumed to be operational
            var resultIfOperational = 0L;
            if (springsLine[linePos - 1] == '.' ||
                springsLine[linePos - 1] == '?')
            {
                if (damagedGroupsPos == 0 || consumed == 0)
                    resultIfOperational = RecursiveCount(springsLine, damagedGroups, linePos - 1, damagedGroupsPos, consumed, cache);
                else if (consumed == damagedGroups[damagedGroupsPos - 1])
                    resultIfOperational = RecursiveCount(springsLine, damagedGroups, linePos - 1, damagedGroupsPos - 1, 0, cache);
            };

            var curResult = resultIfOperational + resultIfDamaged;
            cache[(linePos, damagedGroupsPos, consumed)] = curResult;
            return curResult;
        }

        private string Unfold(string input, char separator, int n)
        {
            List<string> strings = [];
            for (int i=0; i<n;i++)
                strings.Add(input);

            return string.Join(separator, strings);
        }
    }
}
