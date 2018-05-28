using CAFU.Core.Domain.UseCase;
using CAFU.Routing.Domain.UseCase;
using UnityEngine.SceneManagement;

namespace Mogura.Domain.UseCase
{
    public interface ITopUseCase : IUseCase
    {
    }

    public class TopUseCase : ITopUseCase
    {
        public class Factory : DefaultUseCaseFactory<TopUseCase>
        {
            protected override void Initialize(TopUseCase instance)
            {
                base.Initialize(instance);
            }
        }

        public void GotoGameScene()
        {
            var routing = new RoutingUseCase.Factory().Create();
            routing.LoadScene("MoguraGame", LoadSceneMode.Single);
        }
    }
}