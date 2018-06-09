using CAFU.Core.Data.Entity;

namespace Mogura.Data.Entity
{
    public interface IScoreEntity : IEntity
    {
    }

    public class ScoreEntity : IScoreEntity
    {
        public int ScoreNum;
//        public string Date; // 達成日

        public ScoreEntity(int scoreNum)
        {
            ScoreNum = scoreNum;
        }
    }
}