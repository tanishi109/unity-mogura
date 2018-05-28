using CAFU.Core.Presentation.View;
using Mogura.Presentation.Presenter;
using UnityEngine;
using UnityEngine.UI;

namespace Mogura.Presentation.View.Game
{
    public class Controller : Controller<Controller, GamePresenter, GamePresenter.Factory>
    {
        private Text _timerText;
        
        protected override void OnStart()
        {
            base.OnStart();
            _timerText = GameObject.Find("/UI/Canvas/Number").GetComponent<Text>();
            this.GetPresenter().InitTimer();
        }
        
        protected void Update()
        {
            UpdateTimer();
        }
        
        private void UpdateTimer()
        {
            var timerText = this.GetPresenter().UpdateTimer();
            _timerText.text = timerText;
        }
        
        public void SpawnMogura()
        {
            this.GetPresenter().SpawnMogura();
        }
    }

    public static class ViewExtension
    {
        public static GamePresenter GetPresenter(this IView view)
        {
            return Controller.Instance.Presenter;
        }
    }
}