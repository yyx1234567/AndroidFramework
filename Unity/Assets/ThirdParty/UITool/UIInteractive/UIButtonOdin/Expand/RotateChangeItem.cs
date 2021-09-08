using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Title("旋转变换")]
[HideReferenceObjectPicker]
public class RotateChangeItem : UIChangeItemBase
{
    [LabelText("对象")]
    public Transform Target;
    [LabelText("旋转")]
    public Vector3 EndValue;
    [LabelText("动画时间")]
    public float Time;

    public override void Back()
    {

     }

    public override void Play()
    {
         Target.DOLocalRotate(EndValue, Time);
    }
}
