using CAFU.Core.Presentation.Presenter;
using Mogura.Domain.UseCase;

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

        public static void RetryGame()
        {
            LoadingOrderUseCase.FromTo("MoguraResult", "MoguraGame");
        }
        
        public static void GoBackTop()
        {
            LoadingOrderUseCase.FromTo("MoguraResult", "MoguraTop");
        }
    }
}