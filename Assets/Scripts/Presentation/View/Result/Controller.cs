using CAFU.Core.Presentation.View;
using Mogura.Presentation.Presenter;

namespace Mogura.Presentation.View.Result
{
    public class Controller : Controller<Controller, ResultPresenter, ResultPresenter.Factory>
    {
        protected override void OnStart()
        {
            base.OnStart();
        }
    }

    public static class ViewExtension
    {
        public static ResultPresenter GetPresenter(this IView view)
        {
            return Controller.Instance.Presenter;
        }
    }
}