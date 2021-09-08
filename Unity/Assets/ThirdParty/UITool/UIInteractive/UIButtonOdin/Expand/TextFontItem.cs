using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[Title("字体变化")]
[HideReferenceObjectPicker]
public class TextFontItem : UIChangeItemBase
{
    [LabelText("对象")]
    public Transform Target;
    [LabelText("字体")]
    public TMPro.TMP_FontAsset Font;
    [LabelText("字体大小")]
    public int FontSize;

    public override void Back()
    {
    }

    public override void Play()
    {
        var tmptext = Target.GetComponent<TMPro.TMP_Text>();
        tmptext.font = Font;
        DOTween.To(() => tmptext.fontSize, (x) => tmptext.fontSize = x, FontSize, UIChangeSetting.ColorChangeSpeed);
    }
}
