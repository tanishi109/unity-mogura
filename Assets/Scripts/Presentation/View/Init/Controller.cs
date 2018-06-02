using Mogura.Presentation.Presenter;
using CAFU.Core.Presentation.View;

namespace Mogura.Presentation.View.MoguraInit
{
    public class Controller : Controller<Controller, InitPresenter, InitPresenter.Factory>
    {
        protected override void OnStart()
        {
            base.OnStart();
            ToTop();
        }

        public void ToTop()
        {
            InitPresenter.ToTop();
        }
    }

    public static class ViewExtension
    {
        public static InitPresenter GetPresenter(this IView view)
        {
            return Controller.Instance.Presenter;
        }
    }
}