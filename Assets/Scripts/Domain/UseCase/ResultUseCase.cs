using System;
using System.Linq;
using System.Xml;
using CAFU.Core.Domain.UseCase;
using Mogura.Data.Entity;
using Mogura.Domain.Model;
using Mogura.Domain.Repository;
using Mogura.Domain.Translator;
using UnityEngine;

namespace Mogura.Domain.UseCase
{
    public interface IResultUseCase : IUseCase
    {
    }

    public class ResultUseCase : IResultUseCase
    {
        public class Factory : DefaultUseCaseFactory<ResultUseCase>
        {
            protected override void Initialize(ResultUseCase instance)
            {
                base.Initialize(instance);
            }
        }

        public void SaveIfRankIn(int scoreNum)
        {
            var scoreEntities = ScoreRepository.GetScoreEntities(Ranking.Size);
            var currentScore = new Score(scoreNum);
            var ranking = ScoreTranslator.TranslateAsRanking(scoreEntities);
            var rank = ranking.GetRank(currentScore);
            var rankedIn = rank != -1;
            
            if (!rankedIn)
            {
                return;
            }

            // TODO: 連結リストのほうが挿入が楽かな？
            var currentScoreEntity = ScoreTranslator.TranslateAsScoreEntity(currentScore);
            var updatedScoreEntities = scoreEntities.ToList();
            
            updatedScoreEntities.Insert(rank, currentScoreEntity);
            ScoreRepository.SaveScore(updatedScoreEntities.Take(5).ToArray());
            
            Debug.Log("-----result-----");
            // TODO: remove debug log
            var result = ScoreRepository.GetScoreEntities(Ranking.Size);
            foreach (var r in result)
            {
                Debug.Log(r.ScoreNum);
            }
        }
    }
}