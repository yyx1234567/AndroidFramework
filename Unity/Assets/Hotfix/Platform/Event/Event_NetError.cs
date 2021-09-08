using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using ETModel;

namespace ETHotfix
{
    
    [Event(EventIdType.Event_NetError)]
    public sealed class Event_NetError : AEvent
    {
        public override void Run()
        {
            Global.ServerState = ServerConnect.Error;
         }
    }
}