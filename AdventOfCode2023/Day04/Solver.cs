using AdventOfCode2023.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Day04
{
    public class Solver : ISolver
    {
        public string Part1(string input)
        {
            var totalScore = 0;
            var cards = input.AsList();

            foreach (string card in cards)
            {
                var cardNums = card.Split('|')[0]
                    .Split(':')[1]
                    .Trim()
                    .Split(' ')
                    .ToList()
                    .Where(n => n.Length > 0)
                    .Select(n => int.Parse(n));

                var elfNums = card
                    .Split('|')[1]
                    .Trim()
                    .Split(' ')
                    .ToList()
                    .Where(n => n.Length > 0)
                    .Select(n => int.Parse(n));

                var numMatch = cardNums.Intersect(elfNums).Count();
                int cardScore = 0;
                if (numMatch > 0)
                {
                    cardScore = (int)Math.Pow(2, numMatch - 1);
                }

                totalScore += cardScore;
            }

            return totalScore.ToString();
        }

        public string Part2(string input)
        {
            var cards = input.AsList();

            Dictionary<int, int> cardScores = [];
            Dictionary<int, int> cardPile = [];

            foreach (string card in cards)
            {
                var cardId = int.Parse(card.Split(':')[0].Split(' ').Last());

                var cardNums = card.Split('|')[0]
                    .Split(':')[1]
                    .Trim()
                    .Split(' ')
                    .ToList()
                    .Where(n => n.Length > 0)
                    .Select(n => int.Parse(n));

                var elfNums = card
                    .Split('|')[1]
                    .Trim()
                    .Split(' ')
                    .ToList()
                    .Where(n => n.Length > 0)
                    .Select(n => int.Parse(n));

                var numMatch = cardNums.Intersect(elfNums).Count();
                cardScores[cardId] = numMatch;
                cardPile[cardId] = 1;
            }

            for (int cardId = 1; cardId <= cards.Count; cardId++)
            {
                cardPile = AddDictionaries(cardPile, CalcWinnings(cardPile, cardScores, cardId));
            }

            return cardPile.Sum(c => c.Value).ToString();
        }

        private static Dictionary<int,int> CalcWinnings(Dictionary<int, int> cardPile, Dictionary<int, int> scores, int fromId)
        {
            Dictionary<int, int> newCards = [];

            for (int i = 1; i <= scores[fromId]; i++)
            {
                newCards[fromId + i] = cardPile[fromId];
            }

            return newCards;
        }

        private static Dictionary<int, int> AddDictionaries(Dictionary<int, int> d1, Dictionary<int, int> d2)
        {
            foreach (KeyValuePair<int, int> d in d2)
            {
                if (d1.ContainsKey(d.Key))
                {
                    d1[d.Key] += d.Value;
                }
                else
                    d1[d.Key] = d.Value;
            }

            return d1;
        }
    }
}
