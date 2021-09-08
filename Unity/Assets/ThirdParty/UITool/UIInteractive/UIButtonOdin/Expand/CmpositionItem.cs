using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Title("叠加颜色")]
[HideReferenceObjectPicker]
public class CmpositionItem : UIChangeItemBase
{
    [LabelText("叠加对象")]
    public List<MaskableGraphic> TargetList = new List<MaskableGraphic>();
    [HideInInspector]
    public List<Color> InitColorList = new List<Color>();
    [LabelText("叠加颜色")]
    public Color Color1;

    public override void Back()
    {
        for (int i = 0; i < TargetList.Count; i++)
        {
            TargetList[i].color = InitColorList[i];
        }
    }

    public override void Play()
    {
        if (InitColorList.Count == 0)
        {
            foreach (var item in TargetList)
            {
                InitColorList.Add(item.color);
            }
        }
        for (int i = 0; i < TargetList.Count; i++)
        {
            TargetList[i].color = InitColorList[i] * Color1;
        }
    }
}
