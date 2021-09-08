using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace ETModel
{
    public class OperateConfigComponent:Component
    {
        public Dictionary<string, OperateItemScriptObject> AllOperateDic = new Dictionary<string, OperateItemScriptObject>();
        public async void LoadAsync()
        {
            var go = await ResourcesHelper.LoadAsync<GameObject>("config","Config");
            go = go.GetComponent<ReferenceCollector>().Get<GameObject>("OperateConfig");

            foreach (var data in go.GetComponent<ReferenceCollector>().data)
            {
                foreach (var item in (data.gameObject as GameObject).GetComponent<ReferenceCollector>().data)
                {
                     var temp = item.gameObject as OperateItemScriptObject;
                     OperateItemScriptObject so = GameObject.Instantiate(temp);
                     AllOperateDic.Add(item.key, so);
                     so.OperateInfo = (OperateBase)temp.OperateInfo.Clone();
                     so.OperateInfo.Init();
                }
            }
          }
        public void TryStartMission(System.Predicate<OperateBase> predicate)
        {
            var operate = AllOperateDic.Select(x => x.Value.OperateInfo).Where(x=>predicate.Invoke(x)).FirstOrDefault();
            if (operate == null)
            {
                Debug.LogError($"找不到任务");
                return;
            }
            operate.Operate();
        }
      
    }
}