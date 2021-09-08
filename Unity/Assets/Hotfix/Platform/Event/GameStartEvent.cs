using ETModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventArgType 
{
    public struct GameStartEvent 
    {
        
    }
}

namespace ETHotfix
{
    public partial class EventIdType
    {
        public const string GameStartEvent = "GameStartEvent";
    }

    [Event(EventIdType.GameStartEvent)]
    public class GameStartEvent : AEvent<EventArgType.GameStartEvent>
    {
        public override   void Run(EventArgType.GameStartEvent arg)
        {
            UIHelper.OpenUI<UIMainWindowComponent>();  
        }
    }
}
