using ETModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventArgType 
{
    public struct CloseHightlightEvent
    {
        public string TargetName;
    }
}

namespace ETHotfix
{
     [Event(EventIdType.CloseHightlightEvent)]
    public class CloseHightlightEvent : AEvent
    {
        public override void Run()
        {
            HighlightTargetEvent.LastHighlightObject?.ConstantOff();

             //var go = SceneUnitHelper.Get(arg.TargetName);
             //var highlight = go.GetComponent<HighlightingSystem.Highlighter>() ?? go.AddComponent<HighlightingSystem.Highlighter>();
             //highlight.ConstantOff();  
        }
    }
}
