using CAFU.Core.Presentation.Presenter;
using Mogura.Domain.UseCase;
using CAFU.Routing.Domain.UseCase;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Mogura.Presentation.Presenter
{
    public class GamePresenter : IPresenter
    {
        private TimerUseCase _ucTimer;
        
        public class Factory : DefaultPresenterFactory<GamePresenter>
        {
            protected override void Initialize(GamePresenter instance)
            {
                base.Initialize(instance);
            }
        }

        public void InitTimer(float sec)
        {
            _ucTimer = new TimerUseCase.Factory().Create();
            // TODO: Initializeの中で引数付きで呼べたらCreate()だけにできるんだけどな
            _ucTimer.Init(sec);
        }

        public string UpdateTimer()
        {
            _ucTimer.Update();
            return _ucTimer.GetText();
        }
        
        public static void SpawnMogura(Vector3 min, Vector3 max)
        {
            var uc = new MoguraUseCase();
            uc.Spawn(min, max);
        }
        
        public static int BeatMogura(GameObject obj)
        {
            var uc = new MoguraUseCase();
            return uc.Beat(obj);
        }

        public void GotoResult()
        {
            var routing = new RoutingUseCase.Factory().Create();
            routing.LoadScene("MoguraResult", LoadSceneMode.Additive);
        }
    }
}