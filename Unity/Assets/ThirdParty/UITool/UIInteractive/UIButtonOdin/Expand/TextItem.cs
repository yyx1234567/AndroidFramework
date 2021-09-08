using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Title("文本变化")]
[HideReferenceObjectPicker]
public class TextItem : UIChangeItemBase
{
    [LabelText("自身")]
    public TMPro.TMP_Text Self;
    [LabelText("对象")]
    public TMPro.TMP_Text Target;
    [LabelText("文本")]
    public string Context;

    public override void Back()
    {
    }

    public override void Play()
    {
        if (Target == null)
            return;
        if (Self != null)
        {
           Target.text = Self.text;
        }
        else
        {
           Target.text = Context;
        }
    }
}
