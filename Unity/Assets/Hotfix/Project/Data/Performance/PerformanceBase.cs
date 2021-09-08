using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace ETHotfix
{
    public class PerformanceBase 
    {
        public virtual void Init() { }
        public virtual IEnumerator  Execute()
        {
             if (CanExcute())
            {
                yield return StartExecute();
            }
        }
 
        /// <summary>
        /// 重置
        /// </summary>
        public virtual void Reset() { }

        /// <summary>
        /// 停止
        /// </summary>
        public virtual void Stop() { }

        /// <summary>
        ///跳步骤
        /// </summary>
        public virtual void Jump() { }

        public virtual IEnumerator StartExecute() {
            Debug.Log("StartExecute");

            yield break; }

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
