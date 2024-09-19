using Game.Board;
using Game.Cards;
using System;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms.Impl;

namespace Game.Interactions
{
    public sealed class MoveCardCommand : ICommand
    {
        private readonly PlayingCard _card;
        private readonly Pile _sourcePile;
        private readonly Pile _targetPile;

        private readonly List<PlayingCard> _cardsToMove = new();
        private readonly List<CommandSideEffect> _sideEffects = new();

        private int _scoreChange;

        public MoveCardCommand(PlayingCard card, Pile sourcePile, Pile targetPile)
        {
            _card = card;
            _sourcePile = sourcePile;
            _targetPile = targetPile;
        }

        public void Execute(out bool success)
        {
            success = false;

            if (_sourcePile.CanMoveCard(_card, _targetPile))
            {
                success = true;
             
                while (true)
                {
                    (PlayingCard cardToMove, CommandSideEffect sideEffect) = _sourcePile.RemoveTopCard();
                    _cardsToMove.Add(cardToMove);
                    _sideEffects.Add(sideEffect);

                    if (cardToMove == _card)
                        break;
                }

                for (int i = _cardsToMove.Count - 1; i >= 0; i--)
                {
                    var card = _cardsToMove[i];
                    _targetPile.AddCard(card);
                }

                _scoreChange = Scoring.Resolver.GetCardMovedScore(_sourcePile, _targetPile);
                Scoring.HandleScoreChanged(_scoreChange);
            }
        }

        public void Undo()
        {
            while (true)
            {
                // We disregard the side effect when undo-ing since there is no redo functionality
                (PlayingCard cardToMove, CommandSideEffect _) = _targetPile.RemoveTopCard();
                if (cardToMove == _card)
                    break;
            }

            for (int i = 0; i < _sideEffects.Count; i++)
            {
                if (_sideEffects[i] == null) continue;
                _sideEffects[i].UndoAction?.Invoke();
            }

            for (int i = _cardsToMove.Count - 1; i >= 0; i--)
            {
                var card = _cardsToMove[i];
                _sourcePile.AddCard(card);
            }

            Scoring.HandleScoreChanged(-_scoreChange);
        }
    }
}