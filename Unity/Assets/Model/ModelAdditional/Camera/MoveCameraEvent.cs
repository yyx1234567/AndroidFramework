using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETModel
{
    [Event(EventIdType.MoveCameraEvent)]
    public class MoveCameraEvent : AEvent<Camera,int>
    {
        public override void Run(Camera camera,int id)
        {
            var config = Game.Scene.GetComponent<ScriptObjectConfigComponent>().TryGet<ViewConfig>(x=>x.Id==id);

            Debug.LogError(config.Target);

            if (config != null)
            {
                Game.Scene.GetComponent<FreeLookCameraComponent>().MoveToTarget(camera, config);
            }
        }
    }
    
}
