using Game.Cards;

namespace Game.Utility
{
    public static class SuitExtensions
    {
        public static bool IsColorDifferentFrom(this Suit suitA, Suit suitB) => suitA.IsRed() == suitB.IsBlack();
        private static bool IsRed(this Suit suit) => suit == Suit.Diamonds || suit == Suit.Hearts;
        private static bool IsBlack(this Suit suit) => suit == Suit.Clubs || suit == Suit.Spades;
    }
}