using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace ETModel
{
    [Sirenix.OdinInspector.Title("隐藏物体")]
    [Sirenix.OdinInspector.HideReferenceObjectPicker]
    public class PerformanceHideObject : PerformanceBase
    {
        public string TargetID;

        public bool SetScale;

        public bool HideOnAwake;

        private GameObject _target;

        private Vector3 InitScale;
        public override async void Init()
        {
            _target = GameObject.Find(TargetID);
            await Task.Delay(1000);
            if (HideOnAwake)
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
            if (SetScale)
            {
                _target.transform.localScale = Vector3.zero;
             }
            else
            {
                _target.SetActive(false);
            }
        }
         public override void Reset()
        {
            if (_target == null)
            {
                _target = GameObject.Find(TargetID);
            }
            if (_target == null)
                return;
            if (SetScale)
            {
                _target.transform.localScale = InitScale;
            }
            _target.SetActive(true);
        }

        public override async Task  StartExecute()
        {
            if (_target == null)
            {
                _target = GameObject.Find(TargetID);
            }
            if (_target == null)
                return;
            if (SetScale)
            {
                await _target.transform.DOScale(0, 0.3F);
            }
            else
            {
                _target.SetActive(false);
            }
        }
    }
}