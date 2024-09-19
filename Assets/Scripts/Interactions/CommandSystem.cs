using Game.Board;
using Game.Cards;
using System;
using System.Collections.Generic;

namespace Game.Interactions
{
    public static class CommandSystem
    {
        #region Public

        public static event Action OnMoveMade;
        public static event Action OnMoveUnmade;

        public static void ClearCommandHistory() => _commandHistory.Clear();

        public static void Undo()
        {
            if (_commandHistory.Count > 0)
            {
                var commandToUndo = _commandHistory.Pop();
                commandToUndo.Undo();

                OnMoveUnmade?.Invoke();
            }
        }

        public static bool TryMoveCard(PlayingCard card, Pile source, Pile target)
        {
            var moveCommand = new MoveCardCommand(card, source, target);
            moveCommand.Execute(out bool success);

            if (success)
            {
                _commandHistory.Push(moveCommand);
                OnMoveMade?.Invoke();
            }

            return success;
        }

        public static bool TryDrawStockPileCard(PlayingCard card, StockPile stockPile, WastePile wastePile)
        {
            var drawStockPileCardCommand = new DrawStockPileCardCommand(card, stockPile, wastePile);
            drawStockPileCardCommand.Execute(out bool success);

            if (success)
            {
                _commandHistory.Push(drawStockPileCardCommand);
                OnMoveMade?.Invoke();
            }

            return success;
        }

        public static bool TryRefillStockPile(StockPile stockPile, WastePile wastePile)
        {
            var refillStockPileCommand = new RefillStockPileCommand(stockPile, wastePile);
            refillStockPileCommand.Execute(out bool success);

            if (success)
            {
                _commandHistory.Push(refillStockPileCommand);
                OnMoveMade?.Invoke();
            }

            return success;
        }

        #endregion

        #region Private

        private readonly static Stack<ICommand> _commandHistory = new();

        #endregion
    }
}