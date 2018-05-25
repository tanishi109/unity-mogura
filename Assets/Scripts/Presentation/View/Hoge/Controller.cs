using CAFU.Core.Presentation.View;
using Mogura.Presentation.Presenter;
using UnityEngine;

namespace Mogura.Presentation.View.Hoge
{
    public class Controller : Controller<Controller, HogePresenter, HogePresenter.Factory>
    {
        protected override void OnStart()
        {
            base.OnStart();
        }
        
        public void ChangeScene() {
            this.GetPresenter().DoHoge();
        }
    }

    public static class ViewExtension
    {
        public static HogePresenter GetPresenter(this IView view)
        {
            return Controller.Instance.Presenter;
        }
    }
    
}