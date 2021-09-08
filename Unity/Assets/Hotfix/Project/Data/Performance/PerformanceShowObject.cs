using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace ETHotfix
{
    [Sirenix.OdinInspector.Title("显示物体")]
    [Sirenix.OdinInspector.HideReferenceObjectPicker]
    public class PerformanceShowObject : PerformanceBase
    {
         public string TargetID;
         public string HideOnAwake;
         private GameObject _target;
          public override void Init()
        {
            CoroutineComponent.Instance.StartCoroutineVoid(IEInit());
         }

        private IEnumerator IEInit()
        {
            _target = SceneUnitHelper.Get(TargetID);
            yield return null;
            if (bool.Parse( HideOnAwake))
            {
                _target?.SetActive(false);
            }
         }
        public override void Jump()
        {
            _target = GetTarget();
            _target.SetActive(true);
        }

        public override void Reset()
        {
            _target = GetTarget();
            _target.SetActive(false);
        }

        public override IEnumerator StartExecute()
        {
            _target = GetTarget();
            _target.SetActive(true);
            yield break;
        }

        private GameObject GetTarget()
        {
            return _target==null? SceneUnitHelper.Get(TargetID): _target;
        }

        public override void Stop()
        {
        }
    }
}
