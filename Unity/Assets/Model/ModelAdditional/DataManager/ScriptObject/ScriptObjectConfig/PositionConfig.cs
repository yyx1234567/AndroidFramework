using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ETModel
{
    public class PositionConfig : ScriptObjectBaseConfig
    {
        [Sirenix.OdinInspector.TableColumnWidth(200, false)]
        [Sirenix.OdinInspector.LabelText("坐标名称")]
        public string Name;

        [Sirenix.OdinInspector.LabelText("坐标数据")]
        [Sirenix.OdinInspector.TableColumnWidth(300, false)]

        [Sirenix.OdinInspector.OnValueChanged("SetPosition")]
        public Transform Target;
 
         [HideInInspector]
         public Vector3 Position;
 
        public void SetPosition()
        {
            Position = Target.position;
        }
    }
}

