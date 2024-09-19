using Game.Cards;
using Game.Utility;
using UnityEngine;

namespace Game.Board
{
    public sealed class TableauPile : Pile
    {
        [SerializeField] 
        Vector2 _startingLocalPos;

        [SerializeField]
        float _verticalOffsetFaceDown;

        [SerializeField] 
        float _verticalOffsetFaceUp;

        public override void AddCard(PlayingCard card)
        {
            Vector3 newCardTargetPos = transform.TransformPoint(_startingLocalPos);
            float offset = 0;

            if (_cardsInPile.TryPeek(out PlayingCard oldTopCard))
            {
                newCardTargetPos = oldTopCard.transform.position;
                offset = oldTopCard.IsFaceUp ? _verticalOffsetFaceUp : _verticalOffsetFaceDown;
                newCardTargetPos.y += offset;

                oldTopCard.SetColliderEnabled(oldTopCard.IsFaceUp);
            }

            base.AddCard(card);

            card.transform.SetPositionAndRotation(newCardTargetPos, Quaternion.identity);
        }

        public override bool CanMoveCard(PlayingCard card, Pile targetPile)
        {
            switch (targetPile)
            {
                case FoundationPile foundationPile:

                    // Can never put multiple cards down
                    if (card.NextCardInPile != null)
                        return false;

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

                    // Don't allow pile to same pile
                    if (tableauPile == this) return false;

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