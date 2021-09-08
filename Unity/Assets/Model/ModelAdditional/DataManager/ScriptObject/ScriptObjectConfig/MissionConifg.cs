using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace ETModel
{
    public class MissionConifg : ScriptObjectBaseConfig
    {
        [Sirenix.OdinInspector.LabelText("任务ID")]
        public string ID;

        [Sirenix.OdinInspector.LabelText("所属阶段")]
        [Sirenix.OdinInspector.ValueDropdown("GetAllState")]
        public string State;

        public string[] GetAllState => new string[] { "阶段1","阶段2","阶段3"};


        [Sirenix.OdinInspector.LabelText("任务名称")]
        public string Name;

        [Sirenix.OdinInspector.LabelText("任务对象")]
        [Sirenix.OdinInspector.ValueDropdown("GetAllUnit")]
        public string MissionTarget;

        public string[] GetAllUnit; /*DataEditorTool.TryGetConfig<UnitConfig>().Select(x => x.Name).ToArray();*/

        [Sirenix.OdinInspector.LabelText("任务逻辑")]
        [Sirenix.OdinInspector.ValueDropdown("GetAllMissionScript")]
        public string ScriptName;

        private string[] GetAllMissionScript => ReflectionHelper.CreateAllIOperateName().ToArray();



    }
}

