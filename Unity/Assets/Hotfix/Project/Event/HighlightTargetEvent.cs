using ETModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
namespace ETHotfix
{
    public struct HighlightTargetEventArg
    {
        public string TargetName;
        public HighLightType lightType;
    }
    public enum HighLightType
    {
        Normal,
        Mission,
    }
 
    [Event(EventIdType.HighlightTargetEvent)]
    public class HighlightTargetEvent : AEvent<HighlightTargetEventArg>
    {
        public static HighlightingSystem.Highlighter LastHighlightObject;
        public override void Run(HighlightTargetEventArg arg)
        {
            var go = SceneUnitHelper.Get(arg.TargetName);
            if (go == null)
            {
                Debug.Log("找不到物体" + arg.TargetName);
                return;
            }
            var highlight = go.GetComponent<HighlightingSystem.Highlighter>() ?? go.AddComponent<HighlightingSystem.Highlighter>();
            switch (arg.lightType)
            {
                case HighLightType.Mission:
                    if (LastHighlightObject != null)
                    {
                        LastHighlightObject.ConstantOff();
                    }
                    LastHighlightObject = highlight;
                    break;
                case HighLightType.Normal:
                    break;
            }
             highlight.ConstantOn();
         }
    }
}
