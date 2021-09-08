using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System;
using ETModel;
using System.Threading.Tasks;
using System.Threading;

namespace ETHotfix
{
    [Event(EventIdType.ControlUpdateIconEvent)]
    public class ControlUpdateIconEvent : AEvent
    {
         public override  void Run()
        {
            try
            {
                Debug.Log("控制有更新时的图标");
                //Game.Scene.GetComponent<UIComponent>().GetUIComponent<StudentTrainWindowComponent>(UIType.StudentTrainWindow).SetDownLoadBtnState(true);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.StackTrace);
            }
        }

       
    }
}