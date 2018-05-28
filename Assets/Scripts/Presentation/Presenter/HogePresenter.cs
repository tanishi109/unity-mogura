using CAFU.Core.Presentation.Presenter;
using Mogura.Domain.UseCase;
using UnityEngine;

namespace Mogura.Presentation.Presenter
{
    public class HogePresenter : IPresenter
    {
        public class Factory : DefaultPresenterFactory<HogePresenter>
        {
            protected override void Initialize(HogePresenter instance)
            {
                base.Initialize(instance);
            }
        }

        public void ChangeScene()
        {
            var uc = new HogeUseCase();
            uc.ChangeScene();
        }
    }
}