namespace Day7.Data;

public class Hand
{
    public char[] Cards { get; }
    public int Bid { get; }
    
    public Hand(char[] cards, int bid)
    {
        Cards = cards;
        Bid = bid;
    }
    
    public static IComparer<Hand> ByStrengthAscending => new SortByStrengthAscendingComparer();
    public static IComparer<Hand> ByStrengthAscendingWithJoker => new SortByStrengthAscendingWithJokerComparer();

    private abstract class AbstractAscendingHandComparer : IComparer<Hand>
    {
        private const int CardsInHand = 5;
        protected abstract Dictionary<char, int> CardWeightings { get; }
        
        public int Compare(Hand? x, Hand? y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(null, y)) return 1;
            if (ReferenceEquals(x, null)) return -1;
            
            if (GetHandType(x) != GetHandType(y)) return GetHandType(x).CompareTo(GetHandType(y));
            
            for (int card = 0; card < CardsInHand; card++)
            {
                int xHighCard = CardWeightings[x.Cards[card]];
                int yHighCard = CardWeightings[y.Cards[card]];
            
                if (xHighCard == yHighCard) continue;
            
                return xHighCard.CompareTo(yHighCard);
            }

            return 0;
        }
        
        protected virtual HandType GetHandType(Hand hand)
        {
            List<IGrouping<char, char>> cardGroupings = hand.Cards.GroupBy(x => x).ToList();
            int pairCount = cardGroupings.Count(g => g.Count() == 2);
            
            return cardGroupings switch
            {
                _ when cardGroupings.Any(grp => grp.Count() == 5) => HandType.FiveOfAKind,
                _ when cardGroupings.Any(grp => grp.Count() == 4) => HandType.FourOfAKind,
                _ when cardGroupings.Any(grp => grp.Count() == 3) && pairCount == 1 => HandType.FullHouse,
                _ when cardGroupings.Any(grp => grp.Count() == 3) => HandType.ThreeOfAKind,
                _ when pairCount == 2 => HandType.TwoPairs,
                _ when pairCount == 1 => HandType.OnePair,
                _ => HandType.HighCard, 
            };
        }
    }

    private class SortByStrengthAscendingComparer : AbstractAscendingHandComparer
    {
        private static readonly Dictionary<char, int> CardWeights = new()
        {
            { 'A', 13 }, { 'K', 12 }, { 'Q', 11 }, { 'J', 10 }, { 'T', 9 }, { '9', 8 },
            { '8', 7 }, { '7', 6 }, { '6', 5 }, { '5', 4 }, { '4', 3 }, { '3', 2 }, { '2', 1 }
        };

        protected override Dictionary<char, int> CardWeightings => CardWeights;
    }

    private class SortByStrengthAscendingWithJokerComparer : AbstractAscendingHandComparer
    {
        private static readonly Dictionary<char, int> CardWeights = new()
        {
            { 'A', 13 }, { 'K', 12 }, { 'Q', 11 }, { 'T', 10 }, { '9', 9 }, { '8', 8 }, 
            { '7', 7 }, { '6', 6 }, { '5', 5 }, { '4', 4 }, { '3', 3 }, { '2', 2 }, { 'J', 1 }
        };

        protected override Dictionary<char, int> CardWeightings => CardWeights;
        protected override HandType GetHandType(Hand hand)
        {
            const char jokerCard = 'J';
            
            HandType handType = base.GetHandType(hand);
            
            int jokerCount = hand.Cards.Count(c => c == jokerCard);
            
            if (jokerCount > 0)
            {
                handType = handType switch
                {
                    HandType.FiveOfAKind or HandType.FourOfAKind or HandType.FullHouse => HandType.FiveOfAKind,
                    HandType.ThreeOfAKind => HandType.FourOfAKind,
                    HandType.TwoPairs => jokerCount == 1 ? HandType.FullHouse : HandType.FourOfAKind,
                    HandType.OnePair => HandType.ThreeOfAKind,
                    HandType.HighCard => HandType.OnePair,
                    _ => handType
                };
            }
        
            return handType;
        }
    }
}