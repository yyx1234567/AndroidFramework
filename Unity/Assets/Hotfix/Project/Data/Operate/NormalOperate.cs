using Sirenix.OdinInspector;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using System.Linq;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

namespace ETHotfix
{
    [Title("普通操作")]
    [OperateName("普通操作")]
    public class NormalOperate : OperateBase
    {
         public bool AutoOperate;
         public string ViewID;
 
        public override void Init()
        {
            base.Init();
        }

        public override void JumpStep()
        {
            JumpPerformance();
        }

        public override void StartOperate()
        {
            base.StartOperate();
            if (AutoOperate)
            {
                CoroutineComponent.Instance.StartCoroutineVoid(Operate());
            }
        }

        public override IEnumerator Operate()
        {
            if (!string.IsNullOrEmpty(ViewID))
            {
                Game.EventSystem.Run(EventIdType.MoveCameraEvent, Camera.main, ViewID);
            }
            UIHelper.OpenUI<UIMaskWindowComponent>();
            yield return CoroutineComponent.Instance.StartCoroutine(PlayPerformance());

            Game.EventSystem.Run(EventIdType.MissionCompeleteEvent, DataInfo);

            UIHelper.CloseUI<UIMaskWindowComponent>();

        }

        public override void ResetStep()
        {
            ResetPerformance();
        }
    }
}