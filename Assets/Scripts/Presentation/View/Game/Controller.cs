using CAFU.Core.Presentation.View;
using Mogura.Presentation.Presenter;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

namespace Mogura.Presentation.View.Game
{
    public class Controller : Controller<Controller, GamePresenter, GamePresenter.Factory>
    {
        private Text _timerText;
        private Text _scoreText;
        public int Score;
        public Vector3 MoguraPosMin;
        public Vector3 MoguraPosMax;
        public float Seconds;
        private System.Action _update;
        
        protected override void OnStart()
        {
            base.OnStart();
            
            // TODO: 名前以外の間違って変えにくい要素を通して取得したい…Tagとか?
            _timerText = GameObject.Find("/UI/Canvas/TimerText").GetComponent<Text>();
            _scoreText = GameObject.Find("/UI/Canvas/ScoreText").GetComponent<Text>();
            Score = 0;
            
            this.GetPresenter().InitTimer(Seconds);
            StartCoroutine(SpawnMogura());
            
            // TODO: ControllerベタがきになってるのでUseCaseに分離したい
            _update = () =>
            {
                if (_timerText.text == "0")
                {
                    this.GetPresenter().GotoResult();
                    _update = () => {};
                }
                UpdateTimer();
            };
        }
        
        protected void Update()
        {
            _update();
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
                GamePresenter.SpawnMogura(MoguraPosMin, MoguraPosMax);
            }
        }

        public void BeatMogura(BaseEventData data)
        {
            var pointerEventData = (PointerEventData)data;

            Score += GamePresenter.BeatMogura(pointerEventData.pointerPress);
            _scoreText.text = Score.ToString();
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