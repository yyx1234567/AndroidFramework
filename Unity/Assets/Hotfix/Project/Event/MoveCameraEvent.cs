using ETModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETHotfix
{
    [Event(EventIdType.MoveCameraEvent)]
    public class MoveCameraEvent : AEvent<Camera,string>
    {
        public override void Run(Camera camera,string id)
        {
            var config =Game.Scene.GetComponent<ScriptObjectConfigComponent>().TryGet<ViewConfig>(x=>x.ViewID==id);
            
            if (config != null)
            {
                Game.Scene.GetComponent<FreeLookCameraComponent>().MoveToTarget(camera, config);
            }
        }
    }


    
}
