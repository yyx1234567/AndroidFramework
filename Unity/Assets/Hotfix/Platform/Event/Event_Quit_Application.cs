using System.Linq;
using UnityEngine;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 退出应用事件
    /// </summary>
    [Event(EventIdType.Quit_Application)]
    public sealed class Event_Quit_Application : AEvent
    {
        public override  void Run()
        {
           MessageBoxHelper.ShowMessage("是否退出？",()=> 
            {
                ETModel.Game.EventSystem.Run(ETModel.EventIdType.ProjectQuitEvent);
            });
         }
    }
}