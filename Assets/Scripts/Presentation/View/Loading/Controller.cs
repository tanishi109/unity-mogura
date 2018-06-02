using CAFU.Core.Presentation.View;
using Mogura.Presentation.Presenter;

namespace Mogura.Presentation.View.Loading
{
    public class Controller : Controller<Controller, LoadingPresenter, LoadingPresenter.Factory>
    {
        protected override void OnStart()
        {
            base.OnStart();
            TransitScene();
        }

        public void TransitScene()
        {
            this.GetPresenter().TransitScene();
        }
    }

    public static class ViewExtension
    {
        public static LoadingPresenter GetPresenter(this IView view)
        {
            return Controller.Instance.Presenter;
        }
    }
}