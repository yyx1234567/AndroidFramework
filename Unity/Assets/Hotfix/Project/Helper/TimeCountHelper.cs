using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public enum TimeCountType
    {
        Minus,
        Plus,
    }
    public class TimeCountHelper
    {
        private static int _deltaTime;
        /// <summary>
        /// 倒计时
        /// </summary>
        private static IEnumerator TimeMinus(int _count)
        {
            var waitForSeconds = new WaitForSeconds(1);

            while (true)
            {
                _count--;
                string hour = (_count / 3600).ToString();
                string min = (_count / 60).ToString();
                string sec = (_count % 60).ToString();
                if (_count / 60 < 10)
                {
                    min = "0" + min;
                }
                if (_count % 60 < 10)
                {
                    sec = "0" + sec;
                }
                if (_count / 3600 < 10)
                {
                    hour = "0" + hour;
                }
                if (min == "00" && sec == "00")
                {
                    break;
                }
                Game.EventSystem.Run(EventIdType.TimeCountEvent, hour + ":" + min + ":" + sec);
                yield return waitForSeconds;
            }
        }
        /// <summary>
        /// 正记时
        /// </summary>
        private static IEnumerator TimePlus(int count)
        {
            int _usdTime = 0;
            while (true)
            {
                _usdTime += _deltaTime;
                string hour = (_usdTime / 3600).ToString();
                 string sec = (_usdTime % 60).ToString();
                string min =( _usdTime / 60 - (_usdTime / 3600) * 60).ToString();
                if (int.Parse(min) < 10)
                {
                    min = "0" + min;
                }
                if (_usdTime % 60 < 10)
                {
                    sec = "0" + sec;
                }
                if (_usdTime / 3600 < 10)
                {
                    hour = "0" + hour;
                } 
               Game.EventSystem.Run(EventIdType.TimeCountEvent, hour + ":" + min + ":" + sec);
 
                yield return waitForSeconds1;
            }
        }
        private static WaitForSeconds waitForSeconds1;

        public static void SetSpeed(float time, int delta)
        {
            waitForSeconds1 = new WaitForSeconds(time);
            _deltaTime = delta;
        }
 
        public static void CloseTime()
        {
            CoroutineComponent.Instance.StopCorountineWithID("TimePlus");
            UIHelper.CloseUI<UITimeCountWindowComponent>();
        }


        public static void TimeMission(TimeCountType type, int time)
        {
            switch (type)
            {
                case TimeCountType.Minus:
                    CoroutineComponent.Instance.StartCoroutineVoid(TimeMinus(time));
                    break;
                case TimeCountType.Plus:
                    SetSpeed(1,1);
                    CoroutineComponent.Instance.StopCorountineWithID("TimePlus");
                    CoroutineComponent.Instance.StartCoroutineWithID(TimePlus(time),"TimePlus");
                    break;
            }
        }
    }
       
}