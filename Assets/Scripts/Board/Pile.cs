using Game.Cards;
using Game.Interactions;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Board
{
    public abstract class Pile : MonoBehaviour
    {
        #region Public

        public int CardCount => _cardsInPile.Count;

        public abstract bool CanMoveCard(PlayingCard card, Pile targetPile);

        public virtual void AddCard(PlayingCard card)
        {
            if (_cardsInPile.TryPeek(out var oldTopCard))
                oldTopCard.NextCardInPile = card;

            _cardsInPile.Push(card);
            card.SetSortingOrder(_cardsInPile.Count);
            card.SetColliderEnabled(true);
            card.Pile = this;

            AdjustCollider();
        }

        public virtual (PlayingCard, CommandSideEffect) RemoveTopCard()
        {
            CommandSideEffect sideEffect = null;

            var card = _cardsInPile.Pop();
            card.Pile = null;
            card.NextCardInPile = null;

            if (_cardsInPile.TryPeek(out PlayingCard newTopCard))
            {
                int scoreFromFlipping = 0;
                if (!newTopCard.IsFaceUp && this is TableauPile)
                {
                    scoreFromFlipping = Scoring.Resolver.GetTableauCardFlippedScore();
                    Scoring.HandleScoreChanged(scoreFromFlipping);
                }

                bool sideEffectIsFaceUp = newTopCard.IsFaceUp;
                bool sideEffectIsColliderEnabled = newTopCard.IsColliderEnabled;
                var sideEffectCard = newTopCard;
                var sideEffectNextCard = newTopCard.NextCardInPile;

                sideEffect = new CommandSideEffect(() =>
                {
                    sideEffectCard.SetIsFaceUp(sideEffectIsFaceUp);
                    sideEffectCard.SetColliderEnabled(sideEffectIsColliderEnabled);
                    sideEffectCard.NextCardInPile = sideEffectNextCard;
                    Scoring.HandleScoreChanged(-scoreFromFlipping);
                });

                newTopCard.SetIsFaceUp(true);
                newTopCard.SetColliderEnabled(true);
                newTopCard.NextCardInPile = null;
            }

            AdjustCollider();

            return (card, sideEffect);
        }

        public virtual void ClearPile()
        {
            _cardsInPile.Clear();

            AdjustCollider();
        }

        public virtual bool TryPeek(out PlayingCard card)
        {
            bool success = _cardsInPile.TryPeek(out card);
            return success;
        }

        #endregion

        #region Protected

        protected readonly Stack<PlayingCard> _cardsInPile = new();

        [Tooltip("An optional collider that will be enabled if the pile is empty.")]
        [SerializeField]
        protected Collider2D _collider;

        protected void AdjustCollider()
        {
            if (_collider == null) return;

            bool enabled = CardCount == 0;
            _collider.enabled = enabled;
        }

        #endregion
    }
}