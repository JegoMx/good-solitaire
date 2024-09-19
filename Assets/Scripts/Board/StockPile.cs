using Game.Cards;
using Game.Interactions;
using UnityEngine;

namespace Game.Board
{
    public sealed class StockPile : Pile
    {
        public override void AddCard(PlayingCard card)
        {
            if (_cardsInPile.TryPeek(out var oldTopCard))
            {
                oldTopCard.SetColliderEnabled(false);
                oldTopCard.SetIsFaceUp(false);
            }

            base.AddCard(card);

            card.SetIsFaceUp(false);
            card.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        }

        public override (PlayingCard, CommandSideEffect) RemoveTopCard()
        {
            var result = base.RemoveTopCard();

            // Stockpile is a special pile, where even the top card should be face down
            if (_cardsInPile.TryPeek(out var newTopCard))
                newTopCard.SetIsFaceUp(false);

            return result;
        }

        public override bool CanMoveCard(PlayingCard card, Pile targetPile)
        {
            switch (targetPile)
            {
                case WastePile wastePile:

                    return CardCount > 0;

                default:
                    return false;
            }
        }
    }
}