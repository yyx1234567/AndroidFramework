using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
namespace ETModel
{
    [CreateAssetMenu(menuName = "DataManager/Manager/ProjectManager")]
     public class EditorProjectManager : EditorScriptObjectBase
    {
        public TextAsset CurrentProject;

        [Button("测试", ButtonSizes.Large)]
        public void CreateProject()
        {
            //Deserialize();
        }
        public static object Deserialize<T>(  string xml)
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(typeof(T));
                    return xmldes.Deserialize(sr);
                }
            }
            catch (Exception e)
            {

                return null;
            }
        }
        [LabelText("案例列表")]
        [TableList]
        public List<ProjectItem> projects = new List<ProjectItem>();
#if UNITY_EDITOR

        public override void SetPath()
        {
            selfPath = "Assets/Model/ModelAdditional/DataManager/ScriptObject/Manager";
         }
#endif
    }

    public class ProjectItem
    {
        [LabelText("案例ID")]
        public string ProjectID;

        [LabelText("案例名称")]
        public string ProjectName;

        [LabelText("案例数据")]
        public TextAsset ProjectData;
     }
}