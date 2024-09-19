using Game.Cards;
using Game.Utility;

namespace Game.Board
{
    public sealed class FoundationPile : Pile
    {
        public override void AddCard(PlayingCard card)
        {
            if (_cardsInPile.TryPeek(out var oldTopCard))
                oldTopCard.SetColliderEnabled(false);

            base.AddCard(card);

            card.transform.position = transform.position;
        }

        public override bool CanMoveCard(PlayingCard card, Pile targetPile)
        {
            switch (targetPile)
            {
                case FoundationPile foundationPile:

                    if (foundationPile == this)
                        return false;

                    if (card.Denomination == Denomination.Ace && targetPile.CardCount == 0)
                        return true;

                    return false;

                case TableauPile tableauPile:

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