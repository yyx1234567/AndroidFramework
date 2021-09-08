using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 退出应用事件
    /// </summary>
    [Event(EventIdType.Event_Quit_Platform)]
    public sealed class Event_Quit_Platform : AEvent
    {
        public override async void Run()
        {
            MessageBoxHelper.ShowMessage("是否退出？", () =>
            {
                ETModel.Game.EventSystem.Run(ETModel.EventIdType.Event_Quit_Platform);
            });
           
        }
    }
}