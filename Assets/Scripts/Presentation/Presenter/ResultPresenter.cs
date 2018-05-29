using CAFU.Core.Presentation.Presenter;

namespace Mogura.Presentation.Presenter
{
    public class ResultPresenter : IPresenter
    {
        public class Factory : DefaultPresenterFactory<ResultPresenter>
        {
            protected override void Initialize(ResultPresenter instance)
            {
                base.Initialize(instance);
            }
        }
    }
}