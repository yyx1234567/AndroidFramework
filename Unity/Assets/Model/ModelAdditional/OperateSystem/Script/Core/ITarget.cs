using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ETModel {
    public interface ITarget
    {
        bool IsTargetItem { get; set; }
        Dictionary<TargetInfo, float> TargetDic { get; set; }
    }

    public enum TargetInfo
    {
        Target01,
        Target02,
        Target03
    }

    public class TargetItem : ITarget
    {
        [Sirenix.OdinInspector.LabelText("影响指标")]
        [Sirenix.OdinInspector.OnValueChanged("GenerateInitData")]
        [Sirenix.OdinInspector.ShowInInspector]
        public bool IsTargetItem { get; set; }

        [Sirenix.OdinInspector.ShowIf("IsTargetItem")]
        [Sirenix.OdinInspector.ShowInInspector]
         public Dictionary<TargetInfo, float> TargetDic
        {
            get;
            set;
        }
        public void GenerateInitData()
        {
            if (IsTargetItem)
            {
                if (TargetDic == null)
                {
                    TargetDic = new Dictionary<TargetInfo, float>();
                }
                if (TargetDic.Count == 0)
                {
                    foreach (TargetInfo item in System.Enum.GetValues(typeof(TargetInfo)))
                    {
                        TargetDic.Add(item, 0);
                    }
                }
            }
            else
            {
                TargetDic = null;
            }
        }
    }
}