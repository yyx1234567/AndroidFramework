using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Threading.Tasks;

namespace ETModel
{
    public abstract class PerformanceBase 
    {
        public virtual void Init() { }
        public virtual async Task  Execute()
        {
             if (CanExcute())
            {
                await  StartExecute();
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        public abstract void Reset();

        /// <summary>
        ///跳步骤
        /// </summary>
        public abstract void Jump();

        public abstract Task StartExecute();

        //[LabelText("详细设置")]
        //public bool NeedArg;
        //[ShowIf("NeedArg")]
        //[LabelText("操作状态条件")]
        //public List<PartStateInfo> ActiveCondition = new List<PartStateInfo>();
        //[LabelText("动画时间")]
        //public float AnimationTime;

        public virtual bool CanExcute()
        {
            // if (ActiveCondition != null)
            //{
            //    ConfigDataComponent component = Game.Scene.GetComponent<ConfigDataComponent>();
            //    foreach (var item in ActiveCondition)
            //    {
            //        if (!item.Value.Contains(component.GetCarPartDataByName(item.Key).BaseState))
            //        {
            //            return false;
            //        }
            //    }
            //}
            return true;
        }
    }
}
