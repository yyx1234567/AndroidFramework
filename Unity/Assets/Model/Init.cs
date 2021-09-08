using System;
using System.Threading;
using UnityEngine;
using System.Threading.Tasks;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using LitJson;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

namespace ETModel
{
    public class Init : MonoBehaviour
    {
        public static Init Instance;

        public List<GameObject> DontDestoryList = new List<GameObject>();

        public GameObject UILoadingWindow;

        [Sirenix.OdinInspector.LabelText("直接加载项目")]
        public bool LoadProjectDirect;

        [Sirenix.OdinInspector.ShowIf("LoadProjectDirect")]
        [Sirenix.OdinInspector.LabelText("项目名称")]
        public string LoadProjectName;

        private void Awake()
        {
            Instance = this;
            var configData = JsonMapper.ToObject<ConfigData>(AssetsBundleDownloader.LoadFile("Config.txt"));
            var ip = configData.ConfigDic["AssetbundleUrl"];
            PathHelper.Init(ip);
            foreach (var item in DontDestoryList)
            {
                DontDestroyOnLoad(item);
            }
            if (LoadProjectDirect)
            {
                LoadProject(LoadProjectName);
                return;
            }
            Global.LoadProjectName = "platform";
            PathHelper.SetProjectPath(Global.LoadProjectName);
        }

        public async void QuitProject()
        {
            Game.EventSystem.Run(EventIdType.AidQuitEvent);
            await new AsyncOperationTask(SceneManager.LoadSceneAsync("Empty"));
            Game.Close();
            MemoryHelper.ClearMemory();
            AssetBundle.UnloadAllAssetBundles(true);
            Global.LoadProjectName = "platform";
            PathHelper.SetProjectPath(Global.LoadProjectName);
            ETModel.Game.EventSystem.Add(DLLType.Model, typeof(Init).Assembly);
            await ETModel.Init.Instance.ShowLoadingPanel("正在返回....", 24);
            ETModel.Game.Scene.AddComponent<UIComponent>();
            ETModel.Game.Scene.AddComponent<TimerComponent>();
            ETModel.Game.Scene.AddComponent<ResourcesComponent>();
            ETModel.Game.Hotfix.LoadHotfixAssembly();
            ETModel.Game.Hotfix.GotoHotfix();
            await Task.Delay(1000);
            Game.EventSystem.Run(EventIdType.RestartPlatformEvent);
            ETModel.Init.Instance.CloseLoadingPanel();
        }

        public void AddProjectBtn(Button btn)
        {
            btn.gameObject.SetActive(false);
        }

        public async void LoadPlatForm()
        {
            try
            {
                await Init.Instance.ShowLoadingPanel("Loading....", 24);
                ResetProject();
                Global.LoadProjectName = "platform";
                LoadHotFix(Global.LoadProjectName);
                await Task.Delay(100);
                Game.EventSystem.Run(EventIdType.LoadPlatformEvent);
                await Task.Delay(1000);
                Init.Instance.CloseLoadingPanel();

            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log(ex.StackTrace);
            }
        }

        public static void ResetProject()
        {
            Game.Close();
            Game.EventSystem.Add(DLLType.Model, typeof(Init).Assembly);
            Game.Scene.AddComponent<TimerComponent>();
            Game.Scene.AddComponent<UIComponent>();
            Game.Scene.AddComponent<ResourcesComponent>();
        }



        public void LoadHotFix(string Target)
        {
            Global.LoadProjectName = Target;
            PathHelper.SetProjectPath(Target);
            Game.Scene.GetComponent<ResourcesComponent>().LoadOneBundle($"{Target}".ToLower());
            if (Define.IsAsync)
            {
                ResourcesComponent.AssetBundleManifestObject =
            (AssetBundleManifest)Game.Scene.GetComponent<ResourcesComponent>().GetAsset($"{ Global.LoadProjectName}".ToLower(), "AssetBundleManifest");
            }
            Game.Hotfix.LoadHotfixAssembly();
            Game.Hotfix.GotoHotfix();
        }

        float timeCount = 0;
        public async void LoadProject(string Target)
        {
            Target = Target.ToLower();
            try
            {
                ResetProject();
                ///防止多次点击
                if (!LoadProjectDirect && Time.time - timeCount < 1)
                {
                    timeCount = Time.time;
                    return;
                }
                timeCount = Time.time;
                LoadHotFix(Target);
                await Task.Delay(100);

                Game.EventSystem.Run(EventIdType.OpenProjectEvent);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError(ex.StackTrace);
            }
        }

        public async ETTask ShowLoadingPanel(string Content, int fontsize = 36)
        {
            try
            {
                //Game.Scene.GetComponent<ResourcesComponent>().LoadBundle("initsprite");
                //var sprite = Game.Scene.GetComponent<ResourcesComponent>().GetAsset<Sprite>("initsprite", "initsprite");
                //UILoadingWindow.transform.Find("Bg").GetComponent<Image>().sprite = sprite;
            }
            catch (Exception ex)
            {
                //Debug.LogError(ex.StackTrace);
            }
            UILoadingWindow.transform.Find("BackGround/LoadingInfo").GetComponent<Text>().text = Content;
            UILoadingWindow.transform.Find("BackGround/LoadingInfo").GetComponent<Text>().fontSize = fontsize;
            UILoadingWindow.GetComponent<GraphicRaycaster>().enabled = true;
            await UILoadingWindow.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
        }


        public async void CloseLoadingPanel()
        {
            await UILoadingWindow.GetComponent<CanvasGroup>().DOFade(0, 0.5f);
            UILoadingWindow.GetComponent<GraphicRaycaster>().enabled = false;
        }
        private void Update()
        {
            OneThreadSynchronizationContext.Instance.Update();
            Game.Hotfix.Update?.Invoke();
            Game.EventSystem.Update();
        }

        private void LateUpdate()
        {
            Game.Hotfix.LateUpdate?.Invoke();
            Game.EventSystem.LateUpdate();
        }

        private void OnApplicationQuit()
        {

            Game.EventSystem.Run(EventIdType.AidQuitEvent);
            Game.Hotfix.OnApplicationQuit?.Invoke();
            Game.Close();
        }
    }
}