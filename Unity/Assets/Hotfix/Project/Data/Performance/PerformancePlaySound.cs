using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace ETHotfix
{
    [Sirenix.OdinInspector.Title("播放声音")]
    [Sirenix.OdinInspector.HideReferenceObjectPicker]
    public class PerformancePlaySound : PerformanceBase
    {
        public float TimeDelay;
        public string AudioID;
        public bool Loop;

        public override IEnumerator StartExecute()
        {
             yield break;
        }

     

        public override void Reset()
        {
        }

        public override void Jump()
        {
        }

        public override void Stop()
        {
        }
    }
}
