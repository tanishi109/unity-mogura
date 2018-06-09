using System;
using System.Linq;
using CAFU.Core.Domain.Model;
using Mogura.Data.DataStore;
using UnityEngine;

namespace Mogura.Domain.Model
{
    public interface IRanking : IModel
    {
    }

    public class Ranking : IRanking
    {
        public Score[] Scores;
        public const int Size = 5;
        
        public Ranking(Score[] scores)
        {
            Scores = scores;
        }
        
        public int GetRank (Score score)
        {
            return Array.FindIndex(Scores, x => score.ScoreNum >= x.ScoreNum);
        }
    }
}