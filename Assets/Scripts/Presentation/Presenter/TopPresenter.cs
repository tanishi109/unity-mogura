using CAFU.Core.Presentation.Presenter;
using CAFU.Music.Domain.UseCase;
using CAFU.Music.Presentation.Presenter;
using Mogura.Constants;
using Mogura.Domain.UseCase;

namespace Mogura.Presentation.Presenter
{
    public class TopPresenter : IPresenter, IMusicPresenter<MusicName>
    {
        public class Factory : DefaultPresenterFactory<TopPresenter>
        {
            protected override void Initialize(TopPresenter instance)
            {
                base.Initialize(instance);
                instance.MusicUseCase = new MusicUseCase<MusicName>.Factory().Create();
            }
        }
        
        public IMusicUseCase<MusicName> MusicUseCase { get; private set; }

        public void GotoGameScene()
        {
            LoadingOrderUseCase.FromTo("MoguraTop", "MoguraGame");
        }

        public void GotoRankingScene()
        {
            LoadingOrderUseCase.FromTo("MoguraTop", "MoguraRanking");
        }
    }
}

