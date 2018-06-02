using CAFU.Core.Presentation.Presenter;

namespace Mogura.Presentation.Presenter
{
    public class  Presenter : IPresenter
    {
        public class Factory : DefaultPresenterFactory< Presenter>
        {
            protected override void Initialize( Presenter instance)
            {
                base.Initialize(instance);
            }
        }
    }
}