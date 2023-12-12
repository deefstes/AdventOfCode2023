namespace AdventOfCode2023.Day09
{
    using AdventOfCode2023.Utils;
    using System.Collections.Generic;
    using System.Linq;

    public class Solver : ISolver
    {
        public string Part1(string input)
        {
            long total = 0;
            foreach (var line in input.AsList())
            {
                var nextNumber = GetNextNumber(line.Split(' ').Select(long.Parse).ToList());
                total += nextNumber;
            }

            return total.ToString();
        }

        public string Part2(string input)
        {
            long total = 0;
            foreach (var line in input.AsList())
            {
                var nextNumber = GetNextNumber(line.Split(' ').Select(long.Parse).ToList(), true);
                total += nextNumber;
            }

            return total.ToString();
        }

        private long GetNextNumber(List<long> numbers, bool left = false)
        {
            Stack<List<long>> newLists = [];
            newLists.Push(numbers);
            var lastList = numbers;
            bool foundZeros = false;

            while (!foundZeros) 
            {
                List<long> newList = [];

                for (int i = 0; i < lastList.Count - 1; i++)
                {
                    newList.Add(lastList[i + 1] - lastList[i]);
                }
                newLists.Push(newList);
                lastList = newList;
                foundZeros = newList.All(n => n == 0);
            }

            long newNumber = 0;
            bool popped = true;
            while (popped)
            {
                popped = newLists.TryPop(out List<long> poppedList);
                if (popped && poppedList != null)
                {
                    if (!left)
                        newNumber = poppedList.Last() + newNumber;
                    else
                        newNumber = poppedList.First() - newNumber;
                }
            }

            return newNumber;
        }
    }
}
