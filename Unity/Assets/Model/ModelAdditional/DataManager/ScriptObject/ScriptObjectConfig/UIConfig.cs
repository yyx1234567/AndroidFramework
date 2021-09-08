using Sirenix.OdinInspector;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace ETModel
{
    public enum HierarchyType
    {
        Bottom = 0,
        Medium = 10001,
        Top = 20001,
        TopMost = 30001
    }
    public class UIConfig : ScriptObjectBaseConfig
    {
        [Sirenix.OdinInspector.TableColumnWidth(60)]
        public string Name;

        public string Description;

        public HierarchyType Hierarchy;
    }
}