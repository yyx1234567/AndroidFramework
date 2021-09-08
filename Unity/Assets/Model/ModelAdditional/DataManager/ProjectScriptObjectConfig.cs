using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETModel {
    [CreateAssetMenu(menuName = "DataManager/Config/ProjectData")]
    public class ProjectScriptObjectConfig : EditorScriptObjectBase
    {
        [Sirenix.OdinInspector.LabelText("案例名称")]
        public string Name;
#if UNITY_EDITOR
        public override void SetPath()
        {
            selfPath = "Assets/Model/ModelAdditional/DataManager";
        }
#endif
        [Sirenix.OdinInspector.LabelText("任务列表")]
        [Sirenix.OdinInspector.TableList]
         public List<ProjectConfig> projectConfigs = new List<ProjectConfig>();
    }
 }