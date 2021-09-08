using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETModel {
    [Event(EventIdType.DisplayTargetEvent)]
    public class DisplayTargetEvent : AEvent<int>
    {
        public override void Run(int ID)
        {
            var config = Game.Scene.GetComponent<ScriptObjectConfigComponent>().TryGet<ViewConfig>(x=>x.Id==ID);
            if (config != null)
            {
                var go = GameObject.Find(config.Target);
                if (go == null)
                    return;
                Game.Scene.GetComponent<DisplayComponent>().DisplayTarget(go);
            }
         }
    }
}
