using CAFU.Core.Presentation.View;
using Mogura.Presentation.Presenter;
using CAFU.Music.Presentation.Presenter;
using Mogura.Constants;

namespace Mogura.Presentation.View.Top
{
    public class Controller : Controller<Controller, TopPresenter, TopPresenter.Factory>
    {
        protected override void OnStart()
        {
            base.OnStart();
            
            this.GetPresenter().PlayMusic(MusicName.Top, true, true);
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
