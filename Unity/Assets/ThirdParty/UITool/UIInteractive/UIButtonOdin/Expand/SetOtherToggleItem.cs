using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Title("旋转变换")]
[HideReferenceObjectPicker]
public class SetOtherToggleItem : UIChangeItemBase
{
    [LabelText("对象")]
    public List<UIToggleOdin> Target;
    [LabelText("激活状态")]
    public bool IsOn;
     public override void Back()
    {

     }

    public override void Play()
    {
        foreach (var item in Target)
        {
            item.IsOn = IsOn;
        }
    }
}
