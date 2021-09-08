using ETModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

namespace ETHotfix
{
    public partial class EventIdType
    {
         public const string StartMissionWithIndexEvent = "StartMissionWithIndexEvent";
    }
    [Event(EventIdType.StartMissionWithIndexEvent)]
    public class StartMissionWithIndexEvent : AEvent<int>
    {
        private OperateItemScriptObject laststepinfo;
        public override void Run(int index)
        {
            StartMissionEvent.CurrentStepIndex = index;
            var projectconfigcomponent =   Game.Scene.GetComponent<ProjectConfigComponent>();
            var info = projectconfigcomponent.TryGetMission(x => x.StepIndex == index);
            if (info == null)
            {
                if (StartMissionEvent.CurrentStepIndex != 1)
                {
                    MessageBoxHelper.ShowMessage("你已经完成所有任务！", confirmPanelType: ConfirmPanelType.CompeletePanel);
                }
                if (laststepinfo != null)
                {
                    laststepinfo.OperateInfo.CloseOperate();
                    laststepinfo = null;
                }
                return;
            }
             laststepinfo = info;

            info.OperateInfo.StartOperate();
            Game.EventSystem.Run(EventIdType.MoveCameraEvent, Camera.main, info.ViewID);
            Game.EventSystem.Run(EventIdType.HighlightTargetEvent, new HighlightTargetEventArg() { TargetName = info.TargetID, lightType = HighLightType.Mission });

        }
    }
    [Event(EventIdType.StartMissionEvent)]
    public class StartMissionEvent : AEvent<int>
    {
        public static int CurrentStepIndex;
        public override async void Run(int name)
        {
            await Task.Delay(100);
            var projectconfigcomponent =  Game.Scene.GetComponent<ProjectConfigComponent>();
            if (name == 0)
            {
                CurrentStepIndex = 1;
                var info = projectconfigcomponent.TryGetMission(x => x.StepIndex == 1);
                info.OperateInfo.StartOperate();
                Game.EventSystem.Run(EventIdType.MoveCameraEvent, Camera.main, info.ViewID);
                Game.EventSystem.Run(EventIdType.HighlightTargetEvent, new  HighlightTargetEventArg() { TargetName = info.TargetID, lightType = HighLightType.Mission });
                var allmission = projectconfigcomponent.TryGetAllMission();
                for (int i = allmission.Count-1; i >=0 ; i--)
                {
                    allmission[i].OperateInfo.ResetStep();
                }
            }
            else
            {
                 var missionlist = projectconfigcomponent.TryGetAllMission();
                 CoroutineComponent.Instance.StartCoroutineVoid(StartMissionWithIndex(name));
            }
         }

 
        private IEnumerator StartMissionWithIndex(int index)
        {
            OperateItemScriptObject info;
            var projectconfigcomponent = Game.Scene.GetComponent<ProjectConfigComponent>();
            var allmission = projectconfigcomponent.TryGetAllMission();

            var delta = index - CurrentStepIndex;
            if (delta == 0)
                yield break;
            if (delta > 0)
            {
                for (int i = CurrentStepIndex; i < index; i++)
                {
                    allmission[i - 1].OperateInfo.JumpStep();
                }
            }
            else if (delta < 0)
            {
                for (int i = allmission.Count - 1; i >= 0; i--)
                {
                    allmission[i].OperateInfo.ResetStep();
                }
                for (int i = 0; i < index-1; i++)
                {
                    allmission[i].OperateInfo.JumpStep();
                }
             }
            info = projectconfigcomponent.TryGetMission(x => x.StepIndex == index);
            info.OperateInfo.StartOperate();
            CurrentStepIndex = index;
            yield return null;
            Game.EventSystem.Run(EventIdType.MoveCameraEvent, Camera.main, info.ViewID);
            Game.EventSystem.Run(EventIdType.HighlightTargetEvent, new  HighlightTargetEventArg() { TargetName = info.TargetID, lightType = HighLightType.Mission });

        }
    }
}
