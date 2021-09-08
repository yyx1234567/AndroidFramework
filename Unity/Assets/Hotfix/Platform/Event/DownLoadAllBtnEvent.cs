using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 全部更新事件
    /// </summary>
    [Event(EventIdType.DownLoadAllBtnEvent)]
    public sealed class DownLoadAllBtnEvent : AEvent
    {
        public override void Run()
        {
            long sum = 0;
            foreach (var item in Global.NeedUpdateProjectDic)
            {
                sum += item.Value.TotalSize;
            }
            var sizeinfo= ConventToSize(sum);
            var uicomponent = Game.Scene.GetComponent<UIComponent>();
            //var updateWindow = uicomponent.GetUIComponent<UpdateWindowComponent>(UIType.UpdateWindow);
            //uicomponent.UpdateUILayer(UIType.StudentTrainWindow, false);
            //uicomponent.UpdateUILayer(UIType.UpdateWindow, true);
            //updateWindow.ShowInfo(Global.NeedUpdateProjectDic.Count, sizeinfo.Key,sizeinfo.Value);
        }

        private KeyValuePair<string, string> ConventToSize(long size)
        {
            KeyValuePair<string, string> pair;
            float sizef = size;
            int count = 0;
            while (size > 1024)
            {
                size /= 1024;
                count++;
            }
            string value = size.ToString("f2");
            switch (count)
            {
                case 0:
                    pair = new KeyValuePair<string, string>(value, "B");
                    break;
                case 1:
                    pair = new KeyValuePair<string, string>(value, "KB");
                    break;
                case 2:
                    pair = new KeyValuePair<string, string>(value, "MB");
                    break;
                case 3:
                    pair = new KeyValuePair<string, string>(value, "G");
                    break;
            }
            return pair;
        }
    }
}