using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace ETModel
{
    [Sirenix.OdinInspector.Title("播放声音")]
    [Sirenix.OdinInspector.HideReferenceObjectPicker]
    public class PerformancePlaySound : PerformanceBase
    {
        public float TimeDelay;
        public string AudioID;
        public bool Loop;

        public override async Task StartExecute()
        {
            PlauSound(TimeDelay);
         }

        public async void PlauSound(float timeAfter)
        {
            await Task.Delay((int) (timeAfter*1000));
          }

        public override void Reset()
        {
         }

        public override void Jump()
        {
         }
    }
}
