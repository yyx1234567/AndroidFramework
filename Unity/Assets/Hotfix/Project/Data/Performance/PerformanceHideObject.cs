using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace ETHotfix
{
    [Sirenix.OdinInspector.Title("隐藏物体")]
    [Sirenix.OdinInspector.HideReferenceObjectPicker]
    public class PerformanceHideObject : PerformanceBase
    {
        public string TargetID;

          public string HideOnAwake;

        private GameObject _target;

        private Vector3 InitScale;
        public override void Init()
        {
            CoroutineComponent.Instance.StartCoroutineVoid(IEInit());
          
        }

        private IEnumerator IEInit()
        {
            _target = SceneUnitHelper.Get(TargetID);
            yield return null;
            if (bool.Parse(HideOnAwake))
            {
                _target?.SetActive(false);
            }
            else
            {
                InitScale = _target.transform.localScale;
            }
        }
        public override void Jump()
        {
            _target.SetActive(false);
        }
        public override void Reset()
        {
            if (_target == null)
            {
                _target = SceneUnitHelper.Get(TargetID);
            }
            if (_target == null)
                return;
            if (!bool.Parse(HideOnAwake))
                _target.SetActive(true);
        }

        public override IEnumerator StartExecute()
        {
            if (_target == null)
            {
                _target = SceneUnitHelper.Get(TargetID);
            }
            if (_target == null)
                yield break;
            _target.SetActive(false);
        }

        public override void Stop()
        {
         }
    }
}