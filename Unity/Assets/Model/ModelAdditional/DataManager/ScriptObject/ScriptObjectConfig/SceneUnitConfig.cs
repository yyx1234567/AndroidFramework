using Sirenix.OdinInspector;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace ETModel
{
    public class SceneUnitConfig : ScriptObjectBaseConfig
    {
        [Sirenix.OdinInspector.LabelText("物体名称")]
        public string TargetName;
        [Sirenix.OdinInspector.LabelText("物体对象ID")]
        public string TargetID;
        [Sirenix.OdinInspector.LabelText("物体预制件")]
        public GameObject Prefab;
    }
}