using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;
using System.Linq;
using UnityEditor;
using System;
using System.Text.RegularExpressions;

namespace ETModel
{
    [CreateAssetMenu(menuName = "DataManager/Manager/ComponentManager")]
      public class ComponentManager : SettingItemBase
    {
        [LabelText("框架组件")]
        public List<ComponentInfo> components = new List<ComponentInfo>();

        [LabelText("项目组件")]
        public List<ComponentInfo> ProjectComponents = new List<ComponentInfo>();


        public string ComponentPath = "Assets/ModelAdditional";
        public string ComponentInfoPath = "Assets/Res/ScriptObject/Component/ComponentInfo";
        public string ComponentInfoPath2 = "Assets/Res/ScriptObject/Component/ProjectComponentInfo";

        public string ProjectComponentInfoPath = "Assets/Hotfix/Project";


         
        [Button("扫描组件", ButtonSizes.Medium)]
        public void ScanComponentHandle()
        {
             ScanComponent(ComponentPath, ComponentInfoPath, components);
            ScanComponent(ProjectComponentInfoPath, ComponentInfoPath2, ProjectComponents);

        }

        private void ScanComponent(string directory, string componentInfoPath, List<ComponentInfo> projectComponents)
        {
             var directoryInfo = new DirectoryInfo(directory);
            GetFileName(directoryInfo, componentInfoPath, projectComponents);

            foreach (var Directories in directoryInfo.GetDirectories())
            {
                ScanComponent(Directories.FullName, componentInfoPath, projectComponents);
            }
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }

        private void GetFileName(DirectoryInfo directoryInfo, string directory, List<ComponentInfo> projectComponents)
        {
#if UNITY_EDITOR
            foreach (var item in directoryInfo.GetFiles())
            {
                var fullname = item.FullName;
                var temp = fullname.Split('\\');

                if (fullname.EndsWith("Component.cs"))
                {
                    var name = temp[temp.Length - 1].Split('.')[0];
                    if (projectComponents.Select(x => x).Count(y => y.name == name) == 0)
                    {
                        var data = Regex.Split(fullname, "Assets", RegexOptions.IgnoreCase);
                        var target = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets" + data[1]);
                        var infopath = directory + "/" + name + ".asset";
                        if (target == null)
                        {
                            if (File.Exists(infopath))
                            {
                                File.Delete(infopath);
                            }
                            continue;
                        }
                        if (!File.Exists(infopath))
                        {
                            ComponentInfo scriptableObj = CreateInstance<ComponentInfo>();
                            scriptableObj.name = name;
                            scriptableObj.Target = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets" + data[1]);
                            projectComponents.Add(scriptableObj);
                            AssetDatabase.CreateAsset(scriptableObj, directory + "/" + name + ".asset");
                            EditorUtility.SetDirty(scriptableObj);
                        }
                        else
                        {
                            var scriptableObj = AssetDatabase.LoadAssetAtPath<ComponentInfo>(infopath);
                            projectComponents.Add(scriptableObj);
                            EditorUtility.SetDirty(scriptableObj);
                        }
                    }
                }
            }
#endif
        }
    }
}