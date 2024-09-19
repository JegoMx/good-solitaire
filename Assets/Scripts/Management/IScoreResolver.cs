using Game.Board;

namespace Game.Management
{
    public interface IScoreResolver
    {
        public int GetCardMovedScore(Pile oldPile, Pile newPile);
        public int GetTableauCardFlippedScore();
        public int GetTimedScore(float totalSeconds);
    }
}