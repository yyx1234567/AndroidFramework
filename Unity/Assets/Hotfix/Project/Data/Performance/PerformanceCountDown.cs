using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using System.Linq;

namespace ETHotfix
{
    public enum TimeUnit
    {
        sec,
        min,
        h
    }
    [Sirenix.OdinInspector.Title("倒计时")]
    [Sirenix.OdinInspector.HideReferenceObjectPicker]
    public class PerformanceCountDown : PerformanceBase
    {

        [Sirenix.OdinInspector.LabelText("倒计时运行时间")]
        public float Time;
        [Sirenix.OdinInspector.LabelText("倒计时实际时间")]
        public float RealTime;
        [Sirenix.OdinInspector.LabelText("倒计时单位")]
        public TimeUnit m_TimeUnit;
  
        public override void Jump()
        {
        }

        public override void Reset()
        {
        }

        public override IEnumerator StartExecute()
        {
            if (Time == 0 || RealTime == 0)
                yield break;
            var time = this.Time;
            var tempTime = RealTime;
            switch (m_TimeUnit)
            {
                case TimeUnit.min:
                    tempTime *= 60;
                    break;
                case TimeUnit.h:
                    tempTime *= 3600;
                    break;
            }
            TimeCountHelper.SetSpeed(1, (int)(tempTime / Time));
            var result = RealTime;
            while (result > 0)
            {
                UIHelper.OpenUI<UICountDownWindowComponent>().UpdateValue(((int)result + 1).ToString() + m_TimeUnit.ToString(), time / this.Time);
                yield return null;
                result -= UnityEngine.Time.deltaTime * (RealTime / this.Time);
                time -= UnityEngine.Time.deltaTime;
            }
            TimeCountHelper.SetSpeed(1,1);
            UIHelper.CloseUI<UICountDownWindowComponent>();
         }
         public override void Stop()
        {
        }
    }
}
