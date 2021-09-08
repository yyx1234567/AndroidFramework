using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

namespace ETModel
{

    [CreateAssetMenu(menuName = "DataManager/Config/DataCreator")]
    public class CharacterDataCreator : DataCreator
    {
        [ListDrawerSettings()]
        public List<CharacterBaseData> CharacterList = new List<CharacterBaseData>();

        private string datasavepath = "Assets/Model/Common/ScriptObject/ScriptObjectData/CharacterData/Data/";

        public override void CreateData()
        {
            CharacterBaseData baseData = new CharacterBaseData();
            baseData.name = (CharacterList.Count + 1).ToString();
#if UNITY_EDITOR
            AssetDatabase.CreateAsset(baseData, datasavepath+ baseData.name+".asset");
#endif
            CharacterList.Add(baseData);
        }

        public override void DeleteData()
        {
           
        }
    }
}
