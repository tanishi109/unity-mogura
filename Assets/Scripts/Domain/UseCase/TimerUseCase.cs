using CAFU.Core.Domain.UseCase;
using Mogura.Domain.Model;
using UnityEngine;

namespace Mogura.Domain.UseCase
{
    public interface ITimerUseCase : IUseCase
    {
    }

    public class TimerUseCase : ITimerUseCase
    {
        private Model.Timer _timer;
        
        public class Factory : DefaultUseCaseFactory<TimerUseCase>
        {
            protected override void Initialize(TimerUseCase instance)
            {
                base.Initialize(instance);
            }
        }
        
        public void Init(float sec)
        {
            // TODO: Use object initializer
            _timer = new Model.Timer();
            _timer.Seconds = sec;
        }

        public void Update()
        {
            _timer.Seconds -= Time.deltaTime;
        }
        
        public string GetText()
        {
            var seconds = (int) _timer.Seconds;
            return seconds.ToString();
        }
    }
}