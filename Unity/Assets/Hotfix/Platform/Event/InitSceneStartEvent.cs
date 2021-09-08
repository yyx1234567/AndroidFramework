using ETModel;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [Event(EventIdType.InitSceneStartEvent)]
    public class InitSceneStartEvent : AEvent
    {
        public override async void Run()
        {
            Game.Scene.AddComponent<CoroutineComponent>();
            await ETModel.Game.Scene.AddComponent<ETModel.ScriptObjectConfigComponent>().LoadAsync();
            var scriptobjectconfig=  Game.Scene.AddComponent<ScriptObjectConfigComponent>();
            await ResourcesHelper.LoadScene("mainscene", "MainScene");
            await Task.Delay(100);
            var projectconfigcomponent = Game.Scene.AddComponent<ProjectConfigComponent>();
            var projectgo = GameObject.Instantiate(ProjectConfigComponent.Instance.GetProjectData().Prefab, SceneUnitHelper.Get("GameRoot")?.transform);
            projectgo.transform.localPosition = Vector3.zero;
            projectgo.transform.localEulerAngles = Vector3.zero;
            projectgo.transform.localScale = Vector3.one;
            Game.Scene.AddComponent<InputComponent>();
            projectconfigcomponent.Load();
            await scriptobjectconfig.LoadAsync();
            Game.Scene.AddComponent<CameraComponent>();
            Game.Scene.AddComponent<FreeLookCameraComponent>();
            Game.EventSystem.Run(EventIdType.GameStartEvent, new EventArgType.GameStartEvent());
        }
    }
}
