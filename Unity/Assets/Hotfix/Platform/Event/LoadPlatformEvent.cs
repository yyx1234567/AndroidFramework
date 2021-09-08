using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LinqUtils;
using System;
using ETModel;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Collections.Generic;

namespace ETHotfix
{
    [Event(EventIdType.LoadPlatformEvent)]
    public class LoadPlatformEvent : AEvent
    {
        public override async void Run()
        {
            try
            {
                InitApi();
                Game.Scene.AddComponent<UIComponent>();
                await ETModel.Game.Scene.AddComponent<ETModel.ScriptObjectConfigComponent>().LoadAsync();
                ETModel.Game.Scene.GetComponent<ResourcesComponent>().LoadOneBundle($"{Global.LoadProjectName}".ToLower());
                if (Define.IsAsync)
                {
                    ResourcesComponent.AssetBundleManifestObject =
                (AssetBundleManifest)ETModel.Game.Scene.GetComponent<ResourcesComponent>().GetAsset($"{ Global.LoadProjectName}".ToLower(), "AssetBundleManifest");
                }
                await ResourcesHelper.LoadScene("mainscene", "MainScene");
                UnityEngine.Debug.Log("加载平台 成功！！！！！！！！");
                //UIHelper.OpenUI<UIStoreWindowComponent>();
                MemoryHelper.ClearMemory();

            }
            catch (Exception ex)
            {
                Debug.Log(ex.StackTrace);
            }
        }

        public static void InitApi()
        {

        }
    }
}