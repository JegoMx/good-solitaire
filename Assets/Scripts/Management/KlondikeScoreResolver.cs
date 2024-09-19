using Game.Board;
using UnityEngine;

namespace Game.Management
{
    public sealed class KlondikeScoreResolver : IScoreResolver
    {
        public int GetCardMovedScore(Pile oldPile, Pile newPile)
        {
            if (oldPile is TableauPile && newPile is FoundationPile)
                return 10;

            if (oldPile is WastePile && newPile is FoundationPile)
                return 10;

            if (oldPile is WastePile && newPile is TableauPile)
                return 5;

            if (oldPile is FoundationPile && newPile is TableauPile)
                return -15;

            return 0;
        }

        public int GetTableauCardFlippedScore() => 5;
    
        public int GetTimedScore(float totalSeconds)
        {
            return Mathf.CeilToInt(700000 / totalSeconds);
        }
    }
}