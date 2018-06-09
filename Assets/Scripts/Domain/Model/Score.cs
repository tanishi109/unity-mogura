using CAFU.Core.Domain.Model;

namespace Mogura.Domain.Model
{
    public interface IScore : IModel
    {
    }

    public class Score : IScore
    {
        public int ScoreNum;

        public Score(int scoreNum)
        {
            ScoreNum = scoreNum;
        }
    }
}