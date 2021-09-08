using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
namespace ETModel.JSFrameWork
{
    [Sirenix.OdinInspector.Title("显示物体")]
    [Sirenix.OdinInspector.HideReferenceObjectPicker]
    public class PreformanceShowObject : PerformanceBase
    {
        [Sirenix.OdinInspector.LabelText("物体对象")]
        [Sirenix.OdinInspector.ValueDropdown("GetTargetData")]
         public string TargetID;
        private string[] GetTargetData;
        //{
        //    get {
        //        List<string> result = new List<string>();
        //        result.AddRange(DataEditorTool.TryGetConfig<UIPanelConfig>().Select(x => x.Name).ToList());
        //        return result.ToArray();
        //    }
        //}
        [Sirenix.OdinInspector.LabelText("初始化时是否隐藏物体")]
        public bool HideOnAwake;

        private GameObject _target;

        public override async void Init()
        {
            _target = GameObject.Find(TargetID);
            await Task.Delay(1000);
            if (HideOnAwake)
                _target?.SetActive(false);
        }

        public override void Jump()
        {
            _target = GetTarget();
            _target.SetActive(true);
        }

        public override void Reset()
        {
            _target = GetTarget();
            _target.SetActive(false);
        }

        public override async Task StartExecute()
        {
            _target = GetTarget();
            _target.SetActive(true);
        }

        private GameObject GetTarget()
        {
            return _target==null? GameObject.Find(TargetID): _target;
        }
    }
}
