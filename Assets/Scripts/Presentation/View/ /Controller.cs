using CAFU.Core.Presentation.View;
using Mogura.Presentation.Presenter;

namespace Mogura.Presentation.View. 
{
    public class Controller : Controller<Controller,  Presenter,  Presenter.Factory>
    {
        protected override void OnStart()
        {
            base.OnStart();
        }
    }

    public static class ViewExtension
    {
        public static  Presenter GetPresenter(this IView view)
        {
            return Controller.Instance.Presenter;
        }
    }
}