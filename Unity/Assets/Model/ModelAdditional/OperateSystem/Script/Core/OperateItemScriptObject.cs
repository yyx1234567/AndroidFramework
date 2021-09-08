using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

namespace ETModel
{
    [CreateAssetMenu(fileName ="Operae", menuName = "操作",order = 1)]
    public class OperateItemScriptObject : SerializedScriptableObject
    {
#if UNITY_EDITOR
        [ShowInInspector]
        [LabelText("脚本")]
        [OnInspectorInit("GetScript")]
        private TextAsset self;
        private void GetScript()
        {
            if (self == null)
            {
                var path = "Assets/Model/OperateSystem/Script";
                if (Directory.Exists(path))
                {
                    foreach (var item in Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories))
                    {
                        var target = item.Split('\\').LastOrDefault();

                        if (target.Replace(".cs", "") == this.GetType().Name)
                        {
                            self = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(item);
                        }
                    }
                }
            }
        }
#endif

        [LabelText("对象")]
        [ValueDropdown("GetAllUnit")]
        public string TargetID;

        private string[] GetAllUnit=> DataEditorTool.TryGetConfig<SceneUnitConfig>().Select(x => x.TargetName).ToArray();

        [HideInInspector]
        public GameObject Target;

        [EnumToggleButtons]
        [LabelText("初始状态")]
        [ValueDropdown("GetState")]
        public string BaseState = DefaultState;

        public const string DefaultState = "初始状态";
#if UNITY_EDITOR
        public string[] GetState()
        {
            List<string> result = new List<string>();
            result.Add(DefaultState);
            var states = DataEditorTool.GetTargetState(this);
            foreach (var item in states)
            {
                result.Add(item);
            }
            return result.ToArray();
        }
#endif

        [LabelText("激活条件")]
        public List<PartStateInfo> ActiveCondition = new List<PartStateInfo>();

        [LabelText("任务触发位置")]
        [ValueDropdown("GetPositionPoint")]
        public string ViewID;
        private string[] GetPositionPoint => DataEditorTool.TryGetConfig<PositionConfig>().Select(x => x.Name).ToArray();

        [LabelText("任务菜单")]
        public OperateBase OperateInfo;
 
    }
}