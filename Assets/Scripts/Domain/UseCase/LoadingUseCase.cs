using System;
using System.Security.Cryptography.X509Certificates;
using CAFU.Core.Domain.UseCase;
using CAFU.Routing.Domain.UseCase;
using Mogura.Domain.Model;
using UniRx;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mogura.Domain.UseCase
{
    public interface ILoadingUseCase : IUseCase
    {
    }

    public class LoadingUseCase : ILoadingUseCase
    {
        public class Factory : DefaultUseCaseFactory<LoadingUseCase>
        {
            protected override void Initialize(LoadingUseCase instance)
            {
                base.Initialize(instance);
            }
        }

        public void TransitScene()
        {
            var routing = new RoutingUseCase.Factory().Create();
            
            routing.LoadSceneAsObservable(LoadingOrder.To, LoadSceneMode.Additive)
                .Subscribe(result =>
                {
                    if (LoadingOrder.Overlay == false)
                    {
                        routing.UnloadScene(LoadingOrder.From);
                    }
                    routing.UnloadScene("MoguraLoading");
                }, error => { }, () => { });
        }
    }
}