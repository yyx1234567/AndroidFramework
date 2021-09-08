using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System;
using ETModel;
using System.Threading.Tasks;
using System.Threading;

namespace ETModel
{
    [Event(EventIdType.RestartPlatformEvent)]
    public class RestartPlatformEvent : AEvent
    {
        public override void Run()
        {
            try
            {
                Game.EventSystem.Run(EventIdType.LoadPlatformEvent);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.StackTrace);
            }
        }
     }
}