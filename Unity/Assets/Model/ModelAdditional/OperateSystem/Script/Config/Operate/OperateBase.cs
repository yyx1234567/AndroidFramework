using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.IO;

namespace ETModel
{
    [HideReferenceObjectPicker]
    public class PartStateInfo
    {
        [LabelText("对象")]
        [ValueDropdown("GetName")]
        public string Name;
        [LabelText("状态")]
        [ValueDropdown("GetState")]
        public string State;
#if UNITY_EDITOR
        public string[] GetName() { return DataEditorTool.GetAllPartScript(); }
        public string[] GetState()
        {
            if (string.IsNullOrEmpty(Name))
                return null;
            return DataEditorTool.GetTargetState(Name);
        }
#endif
    }
    public abstract class OperateBase:ICloneable
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
                self = this.GetScirptFormDirectory("Assets/Model/OperateSystem/Script");
             }
        }
#endif

        [GUIColor(0, 1, 0.8f)]
        [LabelText("任务名称")]
        public string Name;
        [LabelText("任务ID")]
        public string ID;
        public string State
        {
            get
            {
                string[] temp = this.ToString().Split('.');
                string state = temp[temp.Length - 1];
                return state;
            }
        }
        [LabelText("操作记录")]
        [TextArea]
        public string OperateRecord;

        [LabelText("条件设置")]
        public bool ActiveSetting;

        [ShowIf("ActiveSetting")]
        [LabelText("操作状态条件")]
        [HideReferenceObjectPicker]
        public List<BaseCondition> ActiveCondition = new List<BaseCondition>();

        [LabelText("操作表现")]
        public List<PerformanceBase> CarPerformanceDic = new List<PerformanceBase>();


        public virtual void Init() { }

        public virtual async ETTask Operate()
        {

        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
