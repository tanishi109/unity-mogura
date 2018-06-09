using CAFU.Core.Data.DataStore;
using Mogura.Data.Entity;
using UnityEngine;

namespace Mogura.Data.DataStore
{
    public interface IScoreDataStore : IDataStore
    {
    }

    public class ScoreDataStore : IScoreDataStore
    {
        private readonly string[] _rankingKeys = new string[5]{"HighScore1", "HighScore2", "HighScore3", "HighScore4", "HighScore5"};
        
        public class Factory : DefaultDataStoreFactory<ScoreDataStore>
        {
            protected override void Initialize(ScoreDataStore instance)
            {
                base.Initialize(instance);
            }
        }

        public ScoreEntity GetScoreEntity(int nth)
        {
            var json = PlayerPrefs.GetString(_rankingKeys[nth]);
            var scoreEntity = JsonUtility.FromJson<ScoreEntity>(json) ?? new ScoreEntity(0);

            return scoreEntity;
        }

        public void SaveScore(ScoreEntity scoreEntity, int nth)
        {
            if (nth < 0 || _rankingKeys.Length - 1 < nth)
            {
                Debug.LogWarning("SaveScore index is invalid.");
                return;
            }
            var json = JsonUtility.ToJson(scoreEntity);
            PlayerPrefs.SetString(_rankingKeys[nth], json);
        }
    }
}