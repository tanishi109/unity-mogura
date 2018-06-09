using System.Linq;
using CAFU.Core.Presentation.View;
using Mogura.Presentation.Presenter;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Mogura.Presentation.View.Result
{
    public class Controller : Controller<Controller, ResultPresenter, ResultPresenter.Factory>
    {
        private Text _scoreNumText;
        
        protected override void OnStart()
        {
            base.OnStart();

            // TODO: viewに移動する
            var gameScene = SceneManager.GetSceneByName("MoguraGame");

            if (!gameScene.IsValid())
            {
                return;
            }
            
            var controller = gameScene
                .GetRootGameObjects()
                .ToList()
                .Find(obj => obj.GetComponent<Mogura.Presentation.View.Game.Controller>())
                .GetComponent<Mogura.Presentation.View.Game.Controller>();

            // TODO: 名前以外の間違って変えにくい要素を通して取得したい
            _scoreNumText = GameObject.Find("/UI/Canvas/ScoreNumText").GetComponent<Text>();
            _scoreNumText.text = controller.Score.ToString();
            
            SaveIfRankIn(controller.Score);
        }

        public void SaveIfRankIn(int score)
        {
            ResultPresenter.SaveIfRankIn(score);
        }

        public void RetryGame()
        {
            ResultPresenter.RetryGame();
        }
        
        public void GoBackTop()
        {
            ResultPresenter.GoBackTop();
        }
    }

    public static class ViewExtension
    {
        public static ResultPresenter GetPresenter(this IView view)
        {
            return Controller.Instance.Presenter;
        }
    }
}