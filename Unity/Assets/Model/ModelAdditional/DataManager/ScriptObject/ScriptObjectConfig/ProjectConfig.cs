using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;
namespace ETModel
{
    [Title("任务")]
    public class ProjectConfig
    {
        [ValueDropdown("GetRole")]
        [TableColumnWidth(100, false)]
        public string Role;

        private string[] GetRole=> DataEditorTool.TryGetConfig<RoleConfig>().Select(x => x.RoleName).ToArray();

        [Title("详细任务列表")]
        [TableList]
        public List<ProjectInfo> ProjectList = new List<ProjectInfo>();
    }

    [HideReferenceObjectPicker]
    public class ProjectInfo
    {
        [TableColumnWidth(200, false)]
        [LabelText("任务ID")]
        [ValueDropdown("GetTaskID")]
        public string ID;
        [LabelText("任务名称")]
        public string Name;

        private string[] GetTaskID => DataEditorTool.GetOperateList().Select(x=>x.OperateInfo).Select(x=>x.ID).ToArray();
    }
}
