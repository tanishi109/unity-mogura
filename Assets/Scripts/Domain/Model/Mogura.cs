using CAFU.Core.Domain.Model;
using UnityEngine;

namespace Mogura.Domain.Model
{
    public interface IMogura : IModel
    {
    }

    public class Mogura : IMogura
    {
        public GameObject gameObject;

        public Mogura(GameObject obj)
        {
            gameObject = obj;
        }
    }
}