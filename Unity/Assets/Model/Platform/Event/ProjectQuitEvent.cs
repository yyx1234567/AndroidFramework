using ETModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ETModel
{
    [Event(EventIdType.ProjectQuitEvent)]
    public class ProjectQuitEvent : AEvent
    {
        public override void Run()
        {
            //Game.EventSystem.Run(EventIdType.AidQuitEvent);
            //var handle = SceneManager.LoadSceneAsync("Empty");
            //handle.completed += async (x) =>
            //{
            //    try
            //    {
            //        Game.Close();
            //        MemoryHelper.ClearMemory();
            //        AssetBundle.UnloadAllAssetBundles(true);
            //        Global.LoadProjectName = "platform";
            //        PathHelper.SetProjectPath(Global.LoadProjectName);
            //        ETModel.Game.EventSystem.Add(DLLType.Model, typeof(Init).Assembly);
            //        ETModel.Init.Instance.ShowLoadingPanel();
            //        ETModel.Init.Instance.UpdateLoadingPanel("正在返回....", 30);
            //        ETModel.Game.Scene.AddComponent<TimerComponent>();
            //        ETModel.Game.Scene.AddComponent<ResourcesComponent>();
            //        ETModel.Game.Hotfix.LoadHotfixAssembly();
            //         ETModel.Game.Hotfix.GotoHotfix();
            //        await Task.Delay(1000);
            //       // Game.EventSystem.Run(EventIdType.RestartPlatformEvent);
            //        ETModel.Init.Instance.CloseLoadingPanel();
            //    }
            //    catch (Exception ex)
            //    {
            //        UnityEngine.Debug.Log(ex.StackTrace);
            //    }
            //};
        }
    }
}