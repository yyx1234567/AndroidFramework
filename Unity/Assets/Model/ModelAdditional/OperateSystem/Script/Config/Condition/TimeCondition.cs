using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
[Sirenix.OdinInspector.Title("时间条件")]
public class TimeCondition : BaseCondition
{
    [Sirenix.OdinInspector.ValueDropdown("ConditionArray")]
    [Sirenix.OdinInspector.LabelText("条件")]
    public string Condition;

    public string[] ConditionArray()
    {
        return new string[] { "经过","每隔"};
    }

    [Sirenix.OdinInspector.LabelText("数值")]
    public float Value;
}
