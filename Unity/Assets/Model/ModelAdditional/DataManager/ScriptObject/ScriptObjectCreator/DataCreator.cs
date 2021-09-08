using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace ETModel
{
    public abstract class DataCreator : SerializedScriptableObject
    {
        public string Name;
        [Button(Name = "创建", ButtonHeight = 50)]
        public abstract void CreateData();
        [Button(Name = "删除", ButtonHeight = 50)]
        public abstract void DeleteData();
    }
}