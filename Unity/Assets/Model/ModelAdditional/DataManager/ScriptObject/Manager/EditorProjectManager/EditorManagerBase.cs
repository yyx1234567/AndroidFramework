using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ETModel
{
    public class EditorScriptObjectBase:Sirenix.OdinInspector.SerializedScriptableObject
    {
 #if UNITY_EDITOR
        [ShowInInspector]
        [LabelText("脚本")]
        [OnInspectorInit("GetScript")]
        private TextAsset self;

        protected string selfPath;
        private void GetScript()
        {
            SetPath();
            if (self == null)
            {
                self = this.GetScirptFormDirectory(selfPath);
            }
        }

        public virtual void SetPath() { }
#endif
    }
}

