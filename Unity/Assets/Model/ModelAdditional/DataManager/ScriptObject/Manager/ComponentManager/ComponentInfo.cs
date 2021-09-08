using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;
using System;

namespace ETModel
{

    public enum ComponentType
    { 
       Common,
       NetWork,
       Game
    }
    [CreateAssetMenu(menuName = "DataManager/Component/ComponentInfo")]
    public class ComponentInfo : SerializedScriptableObject
    {
         public TextAsset Target;

        [LabelText("组件类型")]
        public ComponentType CType;
  
        [LabelText("组件名称")]
         public string Name;


        [LabelText("功能描述")]
        [TextArea(10,50)]
        public string Desciption;

    }
}