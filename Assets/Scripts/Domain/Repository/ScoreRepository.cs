using System.Linq;
using CAFU.Core.Domain.Repository;
using Mogura.Data.DataStore;
using Mogura.Data.Entity;

namespace Mogura.Domain.Repository
{
    public interface IScoreRepository : IRepository
    {
    }

    public class ScoreRepository : IScoreRepository
    {
        public class Factory : DefaultRepositoryFactory<ScoreRepository>
        {
            protected override void Initialize(ScoreRepository instance)
            {
                base.Initialize(instance);
            }
        }

        public static ScoreEntity GetScoreEntity(int nth)
        {
            var ds = new ScoreDataStore();
            
            return ds.GetScoreEntity(nth);
        }

        public static ScoreEntity[] GetScoreEntities(uint num)
        {
            var scoreEntities = new ScoreEntity[num];
            foreach (var nth in Enumerable.Range(0, (int)num))
            {
                scoreEntities[nth] = ScoreRepository.GetScoreEntity(nth);
            }

            return scoreEntities;
        }
        
        public static void SaveScore(ScoreEntity[] scoreEntities)
        {
            var ds = new ScoreDataStore();
            
            for(var index = 0; index < scoreEntities.Length; ++index)
            {
                var scoreEntity = scoreEntities[index];
                ds.SaveScore(scoreEntity, index);
            }
        }
    }
}