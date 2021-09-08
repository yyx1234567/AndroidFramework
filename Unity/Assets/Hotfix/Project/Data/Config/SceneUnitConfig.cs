using Sirenix.OdinInspector;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace ETHotfix
{
    public class SceneUnitConfig : BaseConfig
    {
         public string TargetName;
         public string TargetID;
         public GameObject Prefab;
    }
}