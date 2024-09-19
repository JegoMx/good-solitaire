using Game.Cards;
using Game.Utility;

namespace Game.Board
{
    public sealed class WastePile : Pile
    {
        public override void AddCard(PlayingCard card)
        {
            if (_cardsInPile.TryPeek(out var oldTopCard))
            {
                oldTopCard.SetColliderEnabled(false);
            }

            base.AddCard(card);

            card.SetIsFaceUp(true);
            card.transform.position = transform.position;
        }
        public override bool CanMoveCard(PlayingCard card, Pile targetPile)
        {
            switch (targetPile)
            {
                case FoundationPile foundationPile:

                    // If card is Ace and foundation is empty, ok
                    if (card.Denomination == Denomination.Ace && targetPile.CardCount == 0)
                        return true;

                    // If card is next denomination and same suit, ok
                    if (targetPile.TryPeek(out PlayingCard topFoundationCard))
                    {
                        bool isSameSuit = card.Suit == topFoundationCard.Suit;
                        bool isNextHigherDenomination = ((int)topFoundationCard.Denomination + 1) == (int)card.Denomination;
                        if (isSameSuit && isNextHigherDenomination)
                            return true;
                    }

                    return false;

                case TableauPile tableauPile:

                    // If card is King and other pile is empty, ok
                    if (card.Denomination == Denomination.King && targetPile.CardCount == 0)
                        return true;

                    // If card is different color and one denomination lower, ok
                    if (targetPile.TryPeek(out PlayingCard topTableauCard))
                    {
                        bool isDifferentColor = card.Suit.IsColorDifferentFrom(topTableauCard.Suit);
                        bool isNextLowerDenomination = ((int)card.Denomination + 1) == (int)topTableauCard.Denomination;
                        if (isDifferentColor && isNextLowerDenomination)
                            return true;
                    }

                    return false;

                default:
                    return false;
            }
        }
    }
}