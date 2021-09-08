using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using ETModel;
using System.Linq;
using DG.Tweening;

namespace ETHotfix
{
    public partial class UIPartDetailWindowComponent : UIWindowComponent
    {
        private void RegisterEvent()
        {

        }

        protected override void Show()
        {
            InfoText.text = string.Empty;
            Info.UpdateSize();

            Game.EventSystem.Run(EventIdType.CloseHightlightEvent);
            var data = ProjectConfigComponent.Instance.GetProjectData().GetData<InstrumentUnitData>();
            foreach (var item in data.DisplayInfos)
            {
                GameObject.DestroyImmediate(SceneUnitHelper.Get(item.Target).GetComponent<OperateHandler>());
                var operatehandle= SceneUnitHelper.Get(item.Target).AddComponent<OperateHandler>();
                operatehandle.PointerClickEvent += (x) => 
                {
                    InfoText.text = item.Content;
                    Info.UpdateSize();
                    UnityEngine.Debug.Log("Display");
                };
            }
            base.Show();
        }
    }
}

