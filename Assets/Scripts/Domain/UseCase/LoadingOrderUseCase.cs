using CAFU.Core.Domain.UseCase;
using CAFU.Routing.Domain.UseCase;
using Mogura.Domain.Model;
using UnityEngine.SceneManagement;

namespace Mogura.Domain.UseCase
{
    public interface ILoadingOrderUseCase : IUseCase
    {
    }

    public class LoadingOrderUseCase : ILoadingOrderUseCase
    {
        public class Factory : DefaultUseCaseFactory<LoadingOrderUseCase>
        {
            protected override void Initialize(LoadingOrderUseCase instance)
            {
                base.Initialize(instance);
            }
        }

        public static void FromTo(string from, string to, bool overlay = false)
        {
            LoadingOrder.From = from;
            LoadingOrder.To = to;
            LoadingOrder.Overlay = overlay;
            var routing = new RoutingUseCase.Factory().Create();
            routing.LoadScene("MoguraLoading", LoadSceneMode.Additive);
        }
    }
}