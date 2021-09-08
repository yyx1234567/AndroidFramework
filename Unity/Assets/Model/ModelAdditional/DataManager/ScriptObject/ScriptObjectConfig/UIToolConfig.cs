using Sirenix.OdinInspector;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace ETModel
{
     public class UIToolConfig : ScriptObjectBaseConfig
    {
        [Sirenix.OdinInspector.TableColumnWidth(60)]
        public string Name;

        [PreviewField]
        public GameObject Prefab;

        public HierarchyType Hierarchy;
    }
}