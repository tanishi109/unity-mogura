using CAFU.Core.Presentation.View;
using Mogura.Presentation.Presenter;
using UnityEngine;

namespace Mogura.Presentation.View.Ranking
{
    public class Controller : Controller<Controller, RankingPresenter, RankingPresenter.Factory>
    {
        public RectTransform ListItemPrefab;
    
        protected override void OnStart()
        {
            base.OnStart();

            var listItems = GameObject.Find("UI/Canvas/List/Items");
            this.GetPresenter().InitList(listItems.transform, ListItemPrefab);
        }
        
        public void GotoTopScene()
        {
            this.GetPresenter().GotoTopScene();
        }
    }

    public static class ViewExtension
    {
        public static RankingPresenter GetPresenter(this IView view)
        {
            return Controller.Instance.Presenter;
        }
    }
}