using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Title("显隐变化")]
[HideReferenceObjectPicker]
public class SetActiveChange : UIChangeItemBase
{
    [LabelText("对象")]
    public List<Transform> Target = new List<Transform>();
    [LabelText("激活")]
    public bool Active;

    public override void Back()
    {
     }

    public override void Play()
    {
         for (int i = 0; i <Target.Count; i++)
        {
            Target[i].gameObject.SetActive(Active);
        }
    }
}

