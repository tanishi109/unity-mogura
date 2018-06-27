using CAFU.Core.Domain.UseCase;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mogura.Domain.Model;
using UniRx;
using UniRx.Triggers;
using Mogura.Constants;

namespace Mogura.Domain.UseCase
{
    public interface IMoguraUseCase : IUseCase
    {
    }

    public class MoguraUseCase : IMoguraUseCase
    {
        public class Factory : DefaultUseCaseFactory<MoguraUseCase>
        {
            protected override void Initialize(MoguraUseCase instance)
            {
                base.Initialize(instance);
            }
        }
        
        public void Spawn(Vector3 min, Vector3 max)
        {
            var moguraPrefab = GameObject.Find("Mogura");

            var x = Random.Range(min.x, max.x);
            var y = Random.Range(min.y, max.y);
            var z = Random.Range(min.z, max.z);
            var pos = new Vector3(x, y, z);
            
            var gameObject = Object.Instantiate(moguraPrefab, pos, Quaternion.identity);
            var mogura = new Model.Mogura(gameObject);

            Observable.FromCoroutine(() => Despawn(mogura))
                .Subscribe();
        }
        
        public int Beat(GameObject obj)
        {
            var anim = obj.GetComponent<Animator>();
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Damaged"))
            {
                return 0;
            }
            anim.SetTrigger("Damaged");
            Observable.FromCoroutine(() => DeleteGameObject(obj, 1))
                .Subscribe();
            return 1;
        }

        IEnumerator DeleteGameObject(GameObject obj, float n = 0)
        {
            yield return new WaitForSeconds(n);
            GameObject.Destroy(obj);
        }

        IEnumerator Despawn(Model.Mogura mogura)
        {
            yield return new WaitForSeconds(2);
            
            var anim = mogura.gameObject.GetComponent<Animator>();
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Damaged"))
            {
                GameObject.Destroy(mogura.gameObject); // TODO: access to a static member of a type via a derived type
            }
        }
    }
}