using CAFU.Core.Presentation.View;
using Mogura.Presentation.Presenter;

namespace Mogura.Presentation.View.Top
{
    public class Controller : Controller<Controller, TopPresenter, TopPresenter.Factory>
    {
        protected override void OnStart()
        {
            base.OnStart();
        }

        public void GotoGameScene()
        {
            this.GetPresenter().GotoGameScene();
        }

        public void GotoRankingScene()
        {
            this.GetPresenter().GotoRankingScene();
        }
    }

    public static class ViewExtension
    {
        public static TopPresenter GetPresenter(this IView view)
        {
            return Controller.Instance.Presenter;
        }
    }
}