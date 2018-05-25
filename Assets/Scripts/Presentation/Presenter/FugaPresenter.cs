using CAFU.Core.Presentation.Presenter;

namespace Mogura.Presentation.Presenter
{
    public class FugaPresenter : IPresenter
    {
        public class Factory : DefaultPresenterFactory<FugaPresenter>
        {
            protected override void Initialize(FugaPresenter instance)
            {
                base.Initialize(instance);
            }
        }
    }
}