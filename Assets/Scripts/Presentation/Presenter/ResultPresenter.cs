using CAFU.Core.Presentation.Presenter;
using Mogura.Domain.Model;
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

        public static void SaveIfRankIn(int score)
        {
            var uc = new ResultUseCase();
            uc.SaveIfRankIn(score);
        }
        
        public static void RetryGame()
        {
            var routing = new CAFU.Routing.Domain.UseCase.RoutingUseCase.Factory().Create();
            routing.UnloadScene("MoguraGame");
            LoadingOrderUseCase.FromTo("MoguraResult", "MoguraGame");
        }
        
        public static void GoBackTop()
        {
            // TODO: シーンを出る時にoverkayしてるGameシーンを必ずUnloadしないといけないのどうにかしたい
            var routing = new CAFU.Routing.Domain.UseCase.RoutingUseCase.Factory().Create();
            routing.UnloadScene("MoguraGame");
            LoadingOrderUseCase.FromTo("MoguraResult", "MoguraTop");
        }
    }
}