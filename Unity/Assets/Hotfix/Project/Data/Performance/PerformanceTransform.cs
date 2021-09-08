using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using System.Linq;

namespace ETHotfix
{
    [Sirenix.OdinInspector.Title("修改Transform")]
    [Sirenix.OdinInspector.HideReferenceObjectPicker]
    public class PerformanceTransform : PerformanceBase
    {
         public string TargetID;
         public RotateMode Mode;
         public Vector3 RotateValue;
         public Vector3 MoveValue;
         public bool ChangeScale=false;
         public Vector3 ScaleValue;


        public float Time;

        private Vector3 _initRotate;
        private Vector3 _initLocalPosition;
        private Vector3 _initScale;
        private Vector3 NewPosition;

        private GameObject _target;
       private GameObject  target { get {
                return _target == null ? SceneUnitHelper.Get(TargetID) : _target;
            }
        }
        public override void Init()
        {
            if (target != null)
            {
                _initRotate = target.transform.localEulerAngles;
                _initLocalPosition = target.transform.localPosition;
                _initScale = target.transform.localScale;
                NewPosition = MoveValue + target.transform.localPosition;
                RotateValue += target.transform.eulerAngles;
            }
        }
        public override void Jump()
        {
            if (target != null)
            {
                target.transform.DORotate(RotateValue, 0, RotateMode.Fast);
                target.transform.DOLocalMove(NewPosition, 0);
                if (ChangeScale)
                    target.transform.DOScale(ScaleValue, 0);
            }
        }

        public override void Reset()
        {
            if (target != null)
            {
                target.transform.localEulerAngles = _initRotate;
                target.transform.localPosition = _initLocalPosition;
                if (ChangeScale)
                    target.transform.localScale = _initScale;
            }

        }

        public override IEnumerator  StartExecute()
        {
            if (target != null)
            {
                 target.transform.DORotate(RotateValue, Time,RotateMode.Fast);
                target.transform.DOLocalMove(NewPosition, Time);
                if(ChangeScale)
                target.transform.DOScale(ScaleValue, Time);
            }
            yield return new WaitForSeconds(Time);
        }

        public override void Stop()
        {
        }
    }
}
