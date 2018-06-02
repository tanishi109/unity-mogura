using CAFU.Core.Presentation.Presenter;
using Mogura.Domain.UseCase;

namespace Mogura.Presentation.Presenter
{
    public class LoadingPresenter : IPresenter
    {
        public class Factory : DefaultPresenterFactory<LoadingPresenter>
        {
            protected override void Initialize(LoadingPresenter instance)
            {
                base.Initialize(instance);
            }
        }

        public void TransitScene()
        {
            var uc = new LoadingUseCase();
            uc.TransitScene();
        }
    }
}