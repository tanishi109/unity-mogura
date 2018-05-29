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
        private Text _scoreText;
        private int _score;
        public Vector3 MoguraPosMin;
        public Vector3 MoguraPosMax;
        
        protected override void OnStart()
        {
            base.OnStart();
            
            // TODO: 名前以外の間違って変えにくい要素を通して取得したい…Tagとか?
            _timerText = GameObject.Find("/UI/Canvas/TimerText").GetComponent<Text>();
            _scoreText = GameObject.Find("/UI/Canvas/ScoreText").GetComponent<Text>();
            _score = 0;
            
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

        public void BeatMogura()
        {
            Debug.Log("beaten!");
            ++_score;
            _scoreText.text = _score.ToString();
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