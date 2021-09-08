using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using LitJson;
using System.Linq;

namespace ETModel
{

    [CreateAssetMenu(menuName = "DataManager/Config/Data")]
    public class ScriptObjectConfig: Sirenix.OdinInspector.SerializedScriptableObject
    {
#if UNITY_EDITOR
        [ShowInInspector]
        [LabelText("脚本")]
        [OnInspectorInit("GetScript")]
        private TextAsset self;
        private bool Hide = true;
        private void GetScript()
        {
            if (self == null)
            {
                var path = "Assets/Model/ModelAdditional/DataManager";
                self= this.GetScirptFormDirectory(path);
             }
        }
        [LabelText("类型")]
        public ScriptObjectBaseConfig Type;
        [LabelText("类型")]
        //private TextAsset Script => this.GetScirptFormDirectory();
        [Button("添加", ButtonSizes.Large)]
        public void AddData()
        {
            ItemList.Add(Type.Clone() as ScriptObjectBaseConfig);
            OnItemListChanged();
        }
#endif
        [LabelText("配置表名称")]
        public string Name;
        [LabelText("描述")]
        public string Description;
        [LabelText("数据")]
        [Sirenix.OdinInspector.TableList]
        [HideReferenceObjectPicker]
        [OnValueChanged("OnItemListChanged")]
        public List<ScriptObjectBaseConfig> ItemList;
 
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        [OnInspectorInit]
        public virtual void OnItemListChanged()
        {
            if (ItemList == null)
                return;
            for (int i = 0; i < ItemList.Count; i++)
            {
                ItemList[i].Id = i + 1;
            }
        }
    }

}
