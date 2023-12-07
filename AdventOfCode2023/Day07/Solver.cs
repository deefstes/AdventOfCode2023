using AdventOfCode2023.Utils;
using System.Data;

namespace AdventOfCode2023.Day07
{
    public class Solver : ISolver
    {
        public string Part1(string input)
        {
            List<Hand> hands = [];

            foreach (var line in input.AsList())
                hands.Add(new Hand(line));

            hands.Sort();

            long sum = 0;
            for (int i = 0; i < hands.Count; i++)
            {
                Hand hand = hands[i];
                sum += hand.Bid * (i + 1);
            }

            return sum.ToString();
        }

        public string Part2(string input)
        {
            List<Hand> hands = [];
            List<Hand> orderedHands = [];

            foreach (var line in input.AsList())
                hands.Add(new Hand(line, true));

            hands.Sort();

            long sum = 0;
            for (int i = 0; i < hands.Count; i++)
            {
                Hand hand = hands[i];
                sum += hand.Bid * (i + 1);
            }

            return sum.ToString();
        }

        private class Hand : IComparable<Hand>
        {
            public int Bid { get; }
            public HandType Type { get; }
            public char[] Cards { get; } = new char[5];
            public int[] CardValues { get; } = new int[5];

            private static int CardValue(char card, bool useJokers = false)
            {
                switch (card)
                {
                    case 'A': return 14;
                    case 'K': return 13;
                    case 'Q': return 12;
                    case 'J':
                        if (useJokers)
                            return 0;
                        else return 11;
                    case 'T': return 10;
                    default: return card - '0';
                }
            }

            public Hand(string input, bool withJokers = false)
            {
                Bid = int.Parse(input.Split(' ')[1].Trim());
                for (int i = 0; i < 5; i++)
                    Cards[i] = input.Split(' ')[0][i];

                Type = EvaluateHandType(Cards, withJokers);

                for (int i=0; i < Cards.Length; i++)
                    CardValues[i] = CardValue(Cards[i], withJokers);
            }

            private HandType EvaluateHandType(char[] cards, bool useJokers = false)
            {
                if (useJokers)
                {
                    var maxHandType = HandType.HighCard;
                    var maxValue = (int)HandType.HighCard;

                    if (cards.ToList().Contains('J'))
                    {
                        foreach (var card in new[] { 'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2' })
                        {
                            var replacedHand = SwopJokers(cards, card);
                            var type = EvaluateHandType(replacedHand, false);
                            if ((int)type < maxValue) // smaller than operator because the enum is defined in descending order
                            {
                                maxValue = (int)type;
                                maxHandType = type;
                            }
                        }
                    }
                    else
                        return EvaluateHandType(cards, false);

                    return maxHandType;
                }

                // Without jokers
                Dictionary<char, int> cardGroups = [];

                foreach (var c in cards)
                {
                    if (cardGroups.TryGetValue(c, out int value))
                        cardGroups[c]++;
                    else
                        cardGroups[c] = 1;
                }

                if (cardGroups.ContainsValue(5))
                    return HandType.FiveOfAKind;
                else if (cardGroups.ContainsValue(4))
                    return HandType.FourOfAKind;
                else if (cardGroups.ContainsValue(3) && cardGroups.ContainsValue(2))
                    return HandType.FullHouse;
                else if (cardGroups.ContainsValue(3))
                    return HandType.ThreeOfAKind;
                else if (cardGroups.Values.Count(x => x == 2) == 2)
                    return HandType.TwoPair;
                else if (cardGroups.ContainsValue(2))
                    return HandType.OnePair;
                return HandType.HighCard;
            }

            private static char[] SwopJokers(char[] cards, char newCard)
            {
                var newCards = new char[cards.Length];

                for (int i = 0; i < cards.Length; i++)
                {
                    if (cards[i] == 'J')
                        newCards[i] = newCard;
                    else
                        newCards[i] = cards[i];
                }

                return newCards;
            }

            public int CompareTo(Hand? other)
            {
                if (other is null)
                    return 1;

                if (Type < other.Type)
                    return 1;

                if (Type > other.Type)
                    return -1;

                for (int i = 0; i < Cards.Length;i++)
                {
                    if (CardValues[i] > other.CardValues[i])
                        return 1;

                    if (CardValues[i] < other.CardValues[i])
                        return -1;
                }

                return 0;
            }
        }

        private enum HandType
        {
            FiveOfAKind,
            FourOfAKind,
            FullHouse,
            ThreeOfAKind,
            TwoPair,
            OnePair,
            HighCard
        }
    }
}
