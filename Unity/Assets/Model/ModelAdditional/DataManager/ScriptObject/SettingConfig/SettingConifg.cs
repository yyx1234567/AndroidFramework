using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace ETModel
{
    [HideReferenceObjectPicker]
    [Title("主设置")]
    [CreateAssetMenu(menuName = "DataManager/Config/Setting")]
    public class SettingConifg: SettingItemBase
    {
        public SettingItemBase Item;

        [ValueDropdown("InitSetting")]
        public string Name;
        public string[] InitSetting()
        {
            List<string> result = new List<string>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(SettingItemBase)))
                    {
                        result.Add(type.Name);
                    }
                }
            }
            return result.ToArray();
        }
#if UNITY_EDITOR
        [Button("添加设置",ButtonSizes.Large)]
        public void AddConfig()
        {
            if (string.IsNullOrEmpty(Name))
            {
                EditorUtility.DisplayDialog(title:"提示",message:"请选择类型！","是");
                return;
            }
            var so= ScriptableObject.CreateInstance(Name);
            AssetDatabase.CreateAsset(so, $"Assets/Res/ScriptObject/SettingConfig/{Name}.asset");
            AssetDatabase.SaveAssets();
        }
#endif
    }
}
