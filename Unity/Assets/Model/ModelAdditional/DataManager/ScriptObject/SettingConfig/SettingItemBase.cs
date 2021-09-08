using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ETModel
{
     public class SettingItemBase : Sirenix.OdinInspector.SerializedScriptableObject
    {
#if UNITY_EDITOR
        [ShowInInspector]
        [LabelText("脚本")]
        [OnInspectorInit("GetScript")]
        private TextAsset self;
        private void GetScript()
        {
            if (self == null)
            {
                self = this.GetScirptFormDirectory("Assets/Model/ModelAdditional/DataManager");
            }
        }
#endif
    }
}
