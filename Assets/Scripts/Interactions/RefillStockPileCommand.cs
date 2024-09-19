using Game.Board;
using Game.Cards;

namespace Game.Interactions
{
    public sealed class RefillStockPileCommand : ICommand
    {
        private readonly StockPile _stockPile;
        private readonly WastePile _wastePile;

        public RefillStockPileCommand(StockPile stockPile, WastePile wastePile)
        {
            _stockPile = stockPile;
            _wastePile = wastePile;
        }

        public void Execute(out bool success)
        {
            success = false;
            
            if (_stockPile.CardCount == 0 && _wastePile.CardCount > 0)
            {
                success = true;

                (PlayingCard card, var _) = _wastePile.RemoveTopCard();
                while (card != null)
                {
                    _stockPile.AddCard(card);
                    card = _wastePile.CardCount > 0 ? _wastePile.RemoveTopCard().Item1 : null;
                }
            }
        }

        public void Undo()
        {
            (PlayingCard card, var _) = _stockPile.RemoveTopCard();
            while (card != null)
            {
                _wastePile.AddCard(card);
                card = _stockPile.CardCount > 0 ? _stockPile.RemoveTopCard().Item1 : null;
            }
        }
    }
}