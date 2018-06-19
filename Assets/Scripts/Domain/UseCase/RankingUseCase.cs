using System.Linq;
using CAFU.Core.Domain.UseCase;
using UnityEngine;
using UnityEngine.UI;
using Mogura.Domain.Model;
using Mogura.Domain.Repository;

namespace Mogura.Domain.UseCase
{
    public interface IRankingUseCase : IUseCase
    {
    }

    public class RankingUseCase : IRankingUseCase
    {
        public class Factory : DefaultUseCaseFactory<RankingUseCase>
        {
            protected override void Initialize(RankingUseCase instance)
            {
                base.Initialize(instance);
            }
        }

        public void InitList(Transform parentTransform, RectTransform listItemPrefab)
        {
            var result = ScoreRepository.GetScoreEntities(Ranking.Size);
            foreach (var r in result)
            {
                var listItem= Object.Instantiate(listItemPrefab) as RectTransform;
                listItem.SetParent(parentTransform, false);

                var listText = listItem.GetComponentsInChildren<Text>().Where(x => x.name == "ScoreNum").ToList()[0];
                listText.text = r.ScoreNum.ToString();
            }
        }
    }
}