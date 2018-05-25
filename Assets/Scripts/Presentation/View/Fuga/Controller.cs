using CAFU.Core.Presentation.View;
using Mogura.Presentation.Presenter;

namespace Mogura.Presentation.View.Fuga
{
    public class Controller : Controller<Controller, FugaPresenter, FugaPresenter.Factory>
    {
        protected override void OnStart()
        {
            base.OnStart();
        }
    }

    public static class ViewExtension
    {
        public static FugaPresenter GetPresenter(this IView view)
        {
            return Controller.Instance.Presenter;
        }
    }
}