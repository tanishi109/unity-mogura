using CAFU.Core.Presentation.Presenter;
using Mogura.Domain.UseCase;

namespace Mogura.Presentation.Presenter
{
    public class InitPresenter : IPresenter
    {
        public class Factory : DefaultPresenterFactory<InitPresenter>
        {
            protected override void Initialize(InitPresenter instance)
            {
                base.Initialize(instance);
            }
        }

        public static void ToTop()
        {
            LoadingOrderUseCase.FromTo("MoguraInit", "MoguraTop");
        }
    }
}