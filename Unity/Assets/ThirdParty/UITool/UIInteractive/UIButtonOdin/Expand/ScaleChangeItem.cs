using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Title("缩放变化")]
[HideReferenceObjectPicker]
public class ScaleChangeItem : UIChangeItemBase
{
    [LabelText("对象")]
    public Transform Target;
    [LabelText("缩放")]
    public Vector3 Scale;
    [LabelText("动画时间")]
    public float Time;

    public override void Back()
    {

     }

    public override void Play()
    {
         Target.DOScale(Scale,Time);
    }
}
