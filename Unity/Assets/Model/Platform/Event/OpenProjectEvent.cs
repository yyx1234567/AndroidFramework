using UnityEngine;
using UnityEngine.SceneManagement;

namespace ETModel
{
    /// <summary>
    /// 启动项目
    /// </summary>
    [Event(EventIdType.OpenProjectEvent)]
    public class OpenProjectEvent : AEvent
    {
        public override async void Run()
        {
            await Init.Instance.ShowLoadingPanel("Loading....",24);
            string assetbundleName = $"{ Global.LoadProjectName}_mainscene";
            string scene = $"MainScene";
            await ResourcesHelper.LoadScene(assetbundleName, scene);
            Game.EventSystem.Run(EventIdType.InitSceneStartEvent);
            Global.OpenAidTime = Global.GetCurrentTime();
            MemoryHelper.ClearMemory();
            await TimerComponent.Instance.WaitAsync(2000);
            Init.Instance.CloseLoadingPanel();

        }
    }
}