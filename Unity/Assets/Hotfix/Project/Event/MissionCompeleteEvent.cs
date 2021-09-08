using ETModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ETHotfix
{
 
     [Event(EventIdType.MissionCompeleteEvent)]
    public class MissionCompeleteEvent : AEvent<OperateItemScriptObject>
    {
        public override void Run(OperateItemScriptObject a)
        {
            var index = a.OperateInfo.Index + 1;
            Game.EventSystem.Run(EventIdType.StartMissionWithIndexEvent, index);
        }
    }
}
