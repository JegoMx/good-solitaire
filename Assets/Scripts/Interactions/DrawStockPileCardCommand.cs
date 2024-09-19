using Game.Board;
using Game.Cards;

namespace Game.Interactions
{
    public sealed class DrawStockPileCardCommand : ICommand
    {
        private readonly PlayingCard _card;
        private readonly StockPile _stockPile;
        private readonly WastePile _wastePile;

        public DrawStockPileCardCommand(PlayingCard card, StockPile stockPile, WastePile wastePile)
        {
            _card = card;
            _stockPile = stockPile;
            _wastePile = wastePile;
        }

        public void Execute(out bool success)
        {
            success = false;

            if (_stockPile.CardCount > 0)
            {
                success = true;

                _stockPile.RemoveTopCard();
                _wastePile.AddCard(_card);
            }
        }

        public void Undo()
        {
            _wastePile.RemoveTopCard();
            _stockPile.AddCard(_card);
        }
    }
}