using CAFU.Core.Presentation.View;
using Mogura.Presentation.Presenter;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.CompilerServices;

namespace Mogura.Presentation.View.Game
{
    public class Controller : Controller<Controller, GamePresenter, GamePresenter.Factory>
    {
        private Text _timerText;
        public Vector3 MoguraPosMin;
        public Vector3 MoguraPosMax;
        
        protected override void OnStart()
        {
            base.OnStart();
            
            _timerText = GameObject.Find("/UI/Canvas/Number").GetComponent<Text>();
            
            this.GetPresenter().InitTimer();
            StartCoroutine(SpawnMogura());
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
        
        private IEnumerator SpawnMogura()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                this.GetPresenter().SpawnMogura(MoguraPosMin, MoguraPosMax);
            }
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