using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 退出应用事件
    /// </summary>
    [Event(EventIdType.Event_UpdateProject)]
    public sealed class Event_UpdateProject : AEvent<string>
    {
        public override  void Run(string project)
        {
           
        }
    }
}