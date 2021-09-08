using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

public class UIButtonOdin : UISelectable
{
    [LabelText("点击叠加色")]
    public Color PressColor = new Color(0.9f, 0.9f, 0.9f, 1);

 
    private Dictionary<MaskableGraphic, Color> graphicDic = new Dictionary<MaskableGraphic, Color>();
    private Dictionary<MaskableGraphic, Color> graphicDicEndColor = new Dictionary<MaskableGraphic, Color>();

    public override void ChangeState(ButtonState state)
    {
        base.ChangeState(state);
        switch (state)
        {
            case ButtonState.Press:
                Blend_color(PressColor,gameObject);
                break;
            case ButtonState.PressUp:
                ReSetColor(gameObject);
                break;
        }
    }
    private void ReSetColor(GameObject target)
    {
        var images = target.GetComponentsInChildren<MaskableGraphic>();
        foreach (var item in images)
        {
            if (graphicDic.ContainsKey(item))
            {
                 item.DOColor(graphicDic[item], duration: 0.2F);
            }
        }
    }

    private void Blend_color(Color color, GameObject target)
    {
        var images = target.GetComponentsInChildren<MaskableGraphic>();
        var endcolor = Color.white;
        foreach (var item in images)
        {
            endcolor = item.color * color;
            if (!graphicDic.ContainsKey(item))
            {
                graphicDic.Add(item, item.color);
            }
            if (!graphicDicEndColor.ContainsKey(item))
            {
                graphicDicEndColor.Add(item, endcolor);
                item.DOColor(endcolor, duration: 0.2F);
            }
            else
            {
                item.DOColor(graphicDicEndColor[item], duration: 0.2F);
             }
        }
    }

}
