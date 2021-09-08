using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

namespace ETModel
{
    [Sirenix.OdinInspector.Title("旋转")]
    [Sirenix.OdinInspector.HideReferenceObjectPicker]
    public class PerformanceRotateTarget : PerformanceBase
    {

        public string TargetID;

        public RotateMode Mode;

        public Vector3 RotateValue;

        public float Time;

        private Vector3 _initVector;

        private GameObject _target;
       private GameObject  target { get {
                return _target == null ? GameObject.Find(TargetID) : _target;
            }
        }
        public override void Init()
        {
            if (target != null)
                _initVector = target.transform.localEulerAngles;
        }
        public override void Jump()
        {
            if (target != null)
                target.transform.DOLocalRotate(RotateValue,0,Mode);
        }

        public override void Reset()
        {
            if (target != null)
                target.transform.localEulerAngles = _initVector;
        }

        public override async Task StartExecute()
        {
            if (target != null)
                target.transform.DOLocalRotate(RotateValue, Time, Mode);
        }
    }
}
