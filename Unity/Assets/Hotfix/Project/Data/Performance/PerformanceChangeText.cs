using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

namespace ETHotfix
{
    [Sirenix.OdinInspector.Title("文字变换")]
    [Sirenix.OdinInspector.HideReferenceObjectPicker]
    public class PerformanceChangeText : PerformanceBase
    {
        public string TargetID;
  
        private Color EndValue;

        private Color _initColor;

        private GameObject _target;
       private GameObject  target { get {
                return _target == null ? SceneUnitHelper.Get(TargetID) : _target;
            }
        }
        public override void Init()
        {
            _initColor = target.GetComponent<TMPro.TMP_Text>().color;
        }
        public override void Jump()
        {
            target.GetComponent<TMPro.TMP_Text>().color = EndValue;
        }

        public override void Reset()
        {
            target.GetComponent<TMPro.TMP_Text>().color = _initColor;
        }

        public override IEnumerator  StartExecute()
        {
            target.GetComponent<TMPro.TMP_Text>().color = EndValue;
            yield break;
        }

        public override void Stop()
        {
            target.GetComponent<TMPro.TMP_Text>().color = _initColor;
        }
    }
}
