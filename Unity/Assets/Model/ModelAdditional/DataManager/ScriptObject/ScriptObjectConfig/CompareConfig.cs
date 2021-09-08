using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "DataManager/Config/CompareConifg")]
public class CompareConfig : SerializedScriptableObject
{
    public class ScriptObjectInfo
    {
        [FolderPath]
        public string SOPath;
        public GameObject Prefab;
    }
    [TableList]
    public List<ScriptObjectInfo> Data;

#if UNITY_EDITOR
    [Button("更新数据", ButtonSizes.Gigantic)]
    public void UpdateData()
    {
        foreach (var item in Data)
        {
            var collect = item.Prefab.GetComponent<ReferenceCollector>();
            collect.Clear();

            if (Directory.Exists(item.SOPath))
            {
                DirectoryInfo direction = new DirectoryInfo(item.SOPath);
                FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Name.EndsWith(".meta"))
                    {
                        continue;
                    }
                    string path = files[i].FullName;
                    path = path.Replace(@"\", "/");
                    path = "Assets" + path.Replace(Application.dataPath, "");
                    ScriptableObject data = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                    collect.Add(data.name, data);
                }
            }
        }
    }
#endif
}

  
