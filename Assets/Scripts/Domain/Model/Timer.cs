using CAFU.Core.Domain.Model;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

namespace Mogura.Domain.Model
{
    public interface ITimer : IModel
    {
    }

    public class Timer : ITimer
    {
        public float Seconds;
        private Text _displayTime;
    }
}

