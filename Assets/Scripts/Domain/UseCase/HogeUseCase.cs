using CAFU.Core.Domain.UseCase;
using CAFU.Routing.Domain.UseCase;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mogura.Domain.UseCase
{
    public interface IHogeUseCase : IUseCase
    {
    }

    public class HogeUseCase : IHogeUseCase
    {
        public class Factory : DefaultUseCaseFactory<HogeUseCase>
        {
            protected override void Initialize(HogeUseCase instance)
            {
                base.Initialize(instance);
            }
        }

        public void ChangeScene()
        {
            var routing = new RoutingUseCase();
            routing.LoadScene("MoguraFuga", LoadSceneMode.Single);
//            SceneManager.LoadScene("MoguraFuga", LoadSceneMode.Single);
        }
    }
}