using CAFU.Core.Presentation.Presenter;
using UnityEngine;
using Mogura.Domain.UseCase;

namespace Mogura.Presentation.Presenter
{
    public class RankingPresenter : IPresenter
    {
        public class Factory : DefaultPresenterFactory<RankingPresenter>
        {
            protected override void Initialize(RankingPresenter instance)
            {
                base.Initialize(instance);
            }
        }

        public void InitList(Transform parentTransform, RectTransform listItemPrefab)
        {
            var uc = new RankingUseCase();
            
            uc.InitList(parentTransform, listItemPrefab);
        }

        public void GotoTopScene()
        {
            LoadingOrderUseCase.FromTo("MoguraRanking", "MoguraTop");
        }
    }
}