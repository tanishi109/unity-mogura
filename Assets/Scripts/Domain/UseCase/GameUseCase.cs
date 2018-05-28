using System.Runtime.InteropServices;
using CAFU.Core.Domain.UseCase;
using UnityEngine;

namespace Mogura.Domain.UseCase
{
    public interface IGameUseCase : IUseCase
    {
    }

    public class GameUseCase : IGameUseCase
    {
        public class Factory : DefaultUseCaseFactory<GameUseCase>
        {
            protected override void Initialize(GameUseCase instance)
            {
                base.Initialize(instance);
            }
        }
        
        public void SpawnMogura()
        {
            Debug.Log("spwn mogura");
        }
    }
}