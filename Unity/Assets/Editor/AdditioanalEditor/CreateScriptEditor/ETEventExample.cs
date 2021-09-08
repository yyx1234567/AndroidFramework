using ETModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventArgType 
{
    public struct ScriptName 
    {
        
    }
}

namespace ETHotfix
{
    public partial class EventIdType
    {
        public const string ScriptName = "ScriptName";
    }

    [Event(EventIdType.ScriptName)]
    public class ScriptName : AEvent<EventArgType.ScriptName>
    {
        public override void Run(EventArgType.ScriptName arg)
        {
             
        }
    }
}
