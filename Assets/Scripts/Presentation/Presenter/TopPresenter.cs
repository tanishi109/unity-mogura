using CAFU.Core.Presentation.Presenter;
using Mogura.Domain.UseCase;

namespace Mogura.Presentation.Presenter
{
    public class TopPresenter : IPresenter
    {
        public class Factory : DefaultPresenterFactory<TopPresenter>
        {
            protected override void Initialize(TopPresenter instance)
            {
                base.Initialize(instance);
            }
        }

        public void GotoGameScene()
        {
            var uc = new TopUseCase();
            uc.GotoGameScene();
        }
    }
}