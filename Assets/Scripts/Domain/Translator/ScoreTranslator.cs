using System.Linq;
using Mogura.Domain.Model;
using Mogura.Data.Entity;
using UnityEngine;

namespace Mogura.Domain.Translator
{
    public class ScoreTranslator
    {
        public static Ranking TranslateAsRanking(ScoreEntity[] scoreEntities)
        {
            var scores = scoreEntities
                .Select(TranslateAsScore)
                .ToArray();
            var ranking = new Ranking(scores);
            
            return ranking;
        }
        
        public static Score TranslateAsScore(ScoreEntity scoreEntity)
        {
            var score = new Score(scoreEntity.ScoreNum);

            return score;
        }
        
        public static ScoreEntity TranslateAsScoreEntity(Score score)
        {
            var scoreEntity = new ScoreEntity(score.ScoreNum);

            return scoreEntity;
        }
    }
}