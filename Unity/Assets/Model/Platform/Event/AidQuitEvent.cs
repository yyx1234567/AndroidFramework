using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace ETModel
{
    [Event(EventIdType.AidQuitEvent)]
    public class AidQuitEvent : AEvent
    {
        public override void Run()
        {
            if (!Global.HasLogined)
            {
                return;
            }
            try
            {
                //AidLogInfo data = null;
                //string url = $"{ApiCore.ApiUrl(ApiCore.Api_post_quit)}?virtualAidId={Global.aidLogInfo.virtualAidId}";
                //var webrequest = UnityWebRequest.Post(url, "");
                //webrequest.SetRequestHeader("Authorization", "bearer " + Globals.LoginUser.Token);
                //webrequest.SendWebRequest();
                //Global.HasLogined = false;
            }
            catch (Exception ex)
            {
                Global.HasLogined = false;
            }
             //ApiCore.HttpRequestPost($"{ApiCore.ApiUrl(ApiCore.Api_post_quit)}?virtualAidId={Global.aidLogInfo.virtualAidId}", data,true);
            //File.WriteAllText(Application.streamingAssetsPath + "File", $"成功退出{Global.aidLogInfo.virtualAidId}");
        }
    }
}
