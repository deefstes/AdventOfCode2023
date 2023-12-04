using AdventOfCode2023.Utils;
using AdventOfCode2023.Utils.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Day03
{
    public class Solver : ISolver
    {
        public string Part1(string input)
        {
            var engineGrid = new EngineGrid(input.AsList());

            return engineGrid.Sum().ToString();
        }

        public string Part2(string input)
        {
            var engineGrid = new EngineGrid(input.AsList());

            return engineGrid.GearTotal().ToString();
        }
        public readonly struct Location(int x, int y)
        {
            public readonly int x = x;
            public readonly int y = y;
        }

        private class Number
        {
            public int value;
            public int length;
            public Location location;

            public Number(int value, int length, Location location)
            {
                this.value = value;
                this.length = length;
                this.location = location;
            }
        }

        private class Symbol
        {
            public char symbol;
            public Location location;

            public Symbol(char symbol, Location location)
            {
                this.symbol = symbol;
                this.location = location;
            }
        }

        private class EngineGrid
        {
            public Dictionary<Location, Number> numbers = [];
            public Dictionary<Location, Symbol> symbols = [];

            public EngineGrid(IList<string> input)
            {
                Number? curNumber = null;

                for (int y=0; y < input.Count; y++)
                {
                    for (int x = 0; x < input[y].Length; x++)
                    {
                        if (char.IsDigit(input[y][x]))
                        {
                            if (curNumber == null)
                                curNumber = new Number(input[y][x] - '0', 1, new(x, y));
                            else
                            {
                                curNumber.value = curNumber.value * 10 + input[y][x] - '0';
                                curNumber.length++;
                            }
                            continue;
                        }

                        if (input[y][x] != '.')
                        {
                            symbols[new(x, y)] = new(input[y][x], new(x, y));
                        }

                        if (curNumber != null)
                        {
                            numbers[curNumber.location] = curNumber;
                            curNumber = null;
                        }
                    }

                    if (curNumber != null)
                    {
                        numbers[curNumber.location] = curNumber;
                        curNumber = null;
                    }
                }

                if (curNumber != null)
                {
                    numbers[curNumber.location] = curNumber;
                    curNumber = null;
                }
            }

            public int Sum()
            {
                List<int> sum = [];

                foreach (Number number in numbers.Values)
                {
                    var val = number.value;

                    // Check if number touches symbol

                    // left
                    if (symbols.ContainsKey(new(number.location.x - 1, number.location.y)))
                    {
                        sum.Add(val);
                        continue;
                    }

                    // right
                    if (symbols.ContainsKey(new(number.location.x + number.length, number.location.y)))
                    {
                        sum.Add(val);
                        continue;
                    }

                    for (int x = number.location.x - 1; x <= number.location.x + number.length; x++)
                    {
                        // above
                        if (symbols.ContainsKey(new(x, number.location.y - 1)))
                        {
                            sum.Add(val);
                            break;
                        }

                        // below
                        if (symbols.ContainsKey(new(x, number.location.y + 1)))
                        {
                            sum.Add(val);
                            break;
                        }
                    }
                }

                return sum.Sum();
            }

            public int GearTotal()
            {
                List<int> sum = [];

                foreach(Symbol symbol in symbols.Values.Where(s => s.symbol == '*'))
                {
                    List<int> values = [];

                    // Check if symbol touches number
                    for (int y = -1; y <= 1; y++)
                    {
                        for (int x = -3; x <= 1; x++)
                        {
                            if (numbers.TryGetValue(new(symbol.location.x + x, symbol.location.y + y), out Number? num))
                            {
                                if (x >= -1 || (x < -1 && num.length >= -x))
                                {
                                    values.Add(num.value);
                                }
                            }
                        }
                    }

                    if (values.Count == 2)
                    {
                        sum.Add(values.First() * values.Last());
                    }
                    else
                    {
                        Console.WriteLine($"Not adding gear ({symbol.location.x},{symbol.location.y} with {values.Count} neighbouring parts");
                    }
                }

                return sum.Sum();
            }
        }
    }
}
