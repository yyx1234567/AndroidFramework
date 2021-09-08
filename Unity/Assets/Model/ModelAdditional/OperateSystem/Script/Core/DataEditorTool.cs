using ETModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ETModel
{
    public static class DataEditorTool
    {
        public static Dictionary<string, OperateItemScriptObject> AllOperateData;
        public static Dictionary<string, ScriptObjectConfig> AllConfigData;

        private static readonly string ScriptObjectOperatePath = "Assets/Bundles/Independent/OperateConfig.prefab";
        private static readonly string ScriptObjectConfigPath = "Assets/Bundles/Independent/DataConfig.prefab";

        public static string[] GetTargetState(OperateItemScriptObject carPartScript)
        {
            var list = carPartScript.OperateInfo;

            List<string> state = new List<string>();
            state.Add(carPartScript.OperateInfo.Name);

            return state.ToArray();
        }

        public static string[] GetTargetState(string carPartScript)
        {
            return GetTargetState(GetAllSciptObject()[carPartScript]);
        }

        public static string[] GetAllPartScript()
        {
            return GetAllSciptObject().Select(x => x.Key).ToArray();
        }

        public static List<OperateItemScriptObject> GetOperateList()
        {
             return GetAllSciptObject().Select(x => x.Value).ToList();
        }

        public static IEnumerable<T> TryGetConfig<T>() where T : ETModel.ScriptObjectBaseConfig
        {
             if (!GetAllSciptObjectConfig().TryGetValue(typeof(T).Name, out ETModel.ScriptObjectConfig category))
            {
                throw new Exception($"AddressableConfigComponent not found key: {typeof(T).FullName}");
            }
            return category.ItemList.OfType<T>();
        }
        public static Dictionary<string, OperateItemScriptObject> GetAllSciptObject()
        {
            if (AllOperateData == null)
            {
                Dictionary<string, OperateItemScriptObject> dic = new Dictionary<string, OperateItemScriptObject>();
#if UNITY_EDITOR
                GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(ScriptObjectOperatePath);
                var data = go.GetComponent<ReferenceCollector>();
                foreach (var coder in data.data)
                {
                     var datas = (coder.gameObject as GameObject).GetComponent<ReferenceCollector>().data;
                     foreach (var item in datas)
                    {
                        dic.Add(item.key, item.gameObject as OperateItemScriptObject);
                     }
                }
                
#endif
                AllOperateData = dic;
                return dic;
            }
            else
            {
                return AllOperateData;
            }
        }

        public static Dictionary<string, ScriptObjectConfig> GetAllSciptObjectConfig()
        {
            if (AllConfigData == null)
            {
                Dictionary<string, ScriptObjectConfig> dic = new Dictionary<string, ScriptObjectConfig>();
#if UNITY_EDITOR
                GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(ScriptObjectConfigPath);
                var data = go.GetComponent<ReferenceCollector>();
                foreach (var item in data.data)
                {
                    dic.Add(item.key, item.gameObject as ScriptObjectConfig);
                }
#endif
                AllConfigData = dic;
                return dic;
            }
            else
            {
                return AllConfigData;
            }
        }
    }
 }