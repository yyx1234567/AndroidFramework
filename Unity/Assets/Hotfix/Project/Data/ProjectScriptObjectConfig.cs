using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETHotfix
{
    public class ProjectScriptObjectConfig
    {
        public string Name;
        public GameObject Prefab;
        public Dictionary<string, IData> AllDataDic = new Dictionary<string, IData>();

        public T GetData<T>() where T : IData
        {
            if (AllDataDic.TryGetValue(typeof(T).FullName, out IData data))
            {
                return (T)data;
            }
            return default(T);
        }

        public void InitData()
        {
            AllDataDic.Clear();
            AllDataDic.Add(typeof(ExperimentPurposeData).FullName, m_PartDetailData);
            AllDataDic.Add(typeof(ExperimentalMethodData).FullName, m_NoteData);
            AllDataDic.Add(typeof(InstrumentUnitData).FullName, m_OperateData);
            foreach (var item in AllDataDic)
            {
                item.Value.Init();
            }
        }
        public ExperimentPurposeData m_PartDetailData = new ExperimentPurposeData();

        public InstrumentUnitData m_OperateData = new InstrumentUnitData();

        public ExperimentalMethodData m_NoteData = new ExperimentalMethodData();
     }
}