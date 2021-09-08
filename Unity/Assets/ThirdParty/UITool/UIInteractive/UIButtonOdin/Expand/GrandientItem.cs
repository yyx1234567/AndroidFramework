using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
[Title("渐变")]
[HideReferenceObjectPicker]
public class GrandientItem : UIChangeItemBase
{
    [LabelText("渐变对象")]
    public List<MaskableGraphic> TargetList = new List<MaskableGraphic>();
    [LabelText("渐变方式")]
    public Coffee.UIExtensions.UIGradient.Direction Direction;
    [LabelText("渐变颜色")]
    public Color Color1, Color2;

    private List<Color> _initColor = new List<Color>();
    public override void Back()
    {
        for (int i = 0; i < TargetList.Count; i++)
        {
            Coffee.UIExtensions.UIGradient uiscript =
                TargetList[i].GetComponent<Coffee.UIExtensions.UIGradient>() ??
                TargetList[i].gameObject.AddComponent<Coffee.UIExtensions.UIGradient>();
            uiscript.direction = Direction;
            DOTween.To(() => uiscript.color1, x => uiscript.color1 = x, _initColor[0], UIChangeSetting.ColorChangeSpeed);
            DOTween.To(() => uiscript.color2, x => uiscript.color2 = x, _initColor[1], UIChangeSetting.ColorChangeSpeed);
        }
    }

    public override void Play()
    {
         for (int i = 0; i < TargetList.Count; i++)
        {
            Coffee.UIExtensions.UIGradient uiscript =
                TargetList[i].GetComponent<Coffee.UIExtensions.UIGradient>() ??
                TargetList[i].gameObject.AddComponent<Coffee.UIExtensions.UIGradient>();
            uiscript.direction = Direction;
            if (_initColor == null)
            {
                _initColor = new List<Color>();
            }
            if (_initColor.Count == 0)
            {
                _initColor.Add(uiscript.color1);
                _initColor.Add(uiscript.color2);
             }
            DOTween.To(() => uiscript.color1, x => uiscript.color1 = x, Color1, UIChangeSetting.ColorChangeSpeed);
            DOTween.To(() => uiscript.color2, x => uiscript.color2 = x, Color2, UIChangeSetting.ColorChangeSpeed);
        }
    }
}
