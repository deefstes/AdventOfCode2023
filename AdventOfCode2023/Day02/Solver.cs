using AdventOfCode2023.Utils;

namespace AdventOfCode2023.Day02
{
    public class Solver : ISolver
    {
        public string Part1(string input)
        {
            List<int> validGames = new List<int>();

            foreach(var line in input.AsList())
            {
                var valid = true;
                var game = int.Parse(line.Split(":")[0].Split(" ")[1]);
                var combos = line.Split(":")[1].Split(";");

                foreach(var combo in combos)
                {
                    if (!valid)
                        continue;

                    foreach (var colCount in combo.TrimStart(' ').Split(", "))
                    {
                        if (!valid)
                            continue;

                        var num = int.Parse(colCount.Split(' ')[0]);
                        var colour = colCount.Split(' ')[1];
                        switch (colour)
                        {
                            case "red":
                                if (num > 12)
                                {
                                    valid = false;
                                    continue;
                                }
                                break;
                            case "green":
                                if (num > 13)
                                {
                                    valid = false;
                                    continue;
                                }
                                break;
                            case "blue":
                                if (num > 14)
                                {
                                    valid = false;
                                    continue;
                                }
                                break;
                        }
                    }
                }

                if (valid)
                    validGames.Add(game);
            }

            return validGames.Sum().ToString();
        }

        public string Part2(string input)
        {
            var lines = input.Split("\r\n");
            List<int> gamePowers = new List<int>();

            foreach (var line in lines)
            {
                var game = int.Parse(line.Split(":")[0].Split(" ")[1]);
                var combos = line.Split(":")[1].Split(";");
                int maxRed = 0;
                int maxGreen = 0;
                int maxBlue = 0;

                foreach (var combo in combos)
                {
                    foreach (var colCount in combo.TrimStart(' ').Split(", "))
                    {
                        var num = int.Parse(colCount.Split(' ')[0]);
                        var colour = colCount.Split(' ')[1];
                        switch (colour)
                        {
                            case "red":
                                maxRed = Math.Max(maxRed, num);
                                break;
                            case "green":
                                maxGreen = Math.Max(maxGreen, num);
                                break;
                            case "blue":
                                maxBlue = Math.Max(maxBlue, num);
                                break;
                        }
                    }
                }

                gamePowers.Add(maxRed*maxGreen*maxBlue);
            }

            return gamePowers.Sum().ToString();
        }
    }
}
