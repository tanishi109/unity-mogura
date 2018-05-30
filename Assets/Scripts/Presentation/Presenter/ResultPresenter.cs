using CAFU.Core.Presentation.Presenter;
using CAFU.Routing.Domain.UseCase;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            var routing = new RoutingUseCase.Factory().Create();
            routing.LoadScene("MoguraGame", LoadSceneMode.Single);
        }
        
        public static void GoBackTop()
        {
            var routing = new RoutingUseCase.Factory().Create();
            routing.LoadScene("MoguraTop", LoadSceneMode.Single);
        }
    }
}