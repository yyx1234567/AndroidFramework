using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Title("动画变化")]
[HideReferenceObjectPicker]
public class AnimationItem : UIChangeItemBase
{
    [LabelText("动画名称")]
    public string AnimationName;
    public Animator ani;

    public override void Back()
    {
        ani.Play("Back");
     }

    public override void Play()
    {
        ani.Play(AnimationName);
     }
}
