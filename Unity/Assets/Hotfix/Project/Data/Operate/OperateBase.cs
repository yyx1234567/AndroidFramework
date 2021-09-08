using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ETHotfix
{
    public class OperateBase : ICloneable
    {
        public int Index;
        public OperateItemScriptObject DataInfo;

        public string TipInfo;
        public List<PerformanceBase> CarPerformanceDic = new List<PerformanceBase>();

        public virtual void Init()
        {
            foreach (var item in CarPerformanceDic)
            {
                item.Init();
            }
        }

        public IEnumerator PlayPerformance()
        {
            foreach (var item in CarPerformanceDic)
            {
                yield return item.Execute();
            }
        }
        public void StopPerformance()
        {
            foreach (var item in CarPerformanceDic)
            {
                item.Stop();
            }
        }
        public void JumpPerformance()
        {
            foreach (var item in CarPerformanceDic)
            {
                item.Jump();
            }
        }
        public void ResetPerformance()
        {
            foreach (var item in CarPerformanceDic)
            {
                item.Reset();
            }
        }
        public virtual void StartOperate()
        {
            if (!UIOperateStepWindowComponent.IsStepOperateWindowOpen)
            {
                return;
            }
            if (!string.IsNullOrEmpty(TipInfo))
            {
                MessageBoxHelper.ShowMessage(TipInfo, confirmPanelType: ConfirmPanelType.EventType);
            }
            UIHelper.GetUI<UIOperateStepWindowComponent>().InfoText.text = DataInfo.Description;
            UIHelper.GetUI<UIOperateStepWindowComponent>().Info.GetComponent<UIContentsizeFilter>().UpdateSize();

            ProjectConfigComponent.Instance.CurrentStep = DataInfo.OperateInfo.Index;
            var go = SceneUnitHelper.Get(DataInfo.TargetID);
            if (go == null)
                return;
            var collider = go.transform.Find("Collider");
            if (collider == null)
                return;
            GameObject.DestroyImmediate(collider.GetComponent<OperateHandler>());
            var handle = collider.gameObject.AddComponent<OperateHandler>();
            handle.PointerClickEvent += StartOperateHandle;
        }

        public void CloseOperate()
        {
            var go = SceneUnitHelper.Get(DataInfo.TargetID);
            if (go == null)
                return;
            var collider = go.transform.Find("Collider");
            if (collider == null)
                return;
            GameObject.DestroyImmediate(collider.GetComponent<OperateHandler>());
        }

        private void StartOperateHandle(UnityEngine.EventSystems.PointerEventData pointerEventData)
        {
            if (ProjectConfigComponent.Instance.CurrentStep == DataInfo.OperateInfo.Index)
            {
                Game.EventSystem.Run(EventIdType.CloseHightlightEvent);
                CoroutineComponent.Instance.StartCoroutineVoid(Operate());
            }
        }
        public virtual IEnumerator Operate() { yield break; }
        public virtual void ResetStep() { }
        public virtual void JumpStep() { }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
