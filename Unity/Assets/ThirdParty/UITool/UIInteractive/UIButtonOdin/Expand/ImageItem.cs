using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
[Title("图片变化")]
[HideReferenceObjectPicker]
public class ImageItem : UIChangeItemBase
{
    [LabelText("对象")]
    public new MaskableGraphic Graphic;
    [LabelText("切换图片")]
    public new Sprite ToSprite;
    [LabelText("变换颜色")]
    public new Color ToColor = Color.white;

    private Sprite  _initSprite;
    private Color? _initColor;

    private Tween _lastTween;
    public override void Back()
    {
        ///非运行模式下
        if (!Application.isPlaying)
        {
            if (Graphic == null)
                return;
            Graphic.color = _initColor.Value;
            if (ToSprite != null)
            {
                (Graphic as Image).sprite = _initSprite;
            }
            return;
        }

        if (Graphic == null)
            return;
        Graphic.DOColor(_initColor.Value, UIChangeSetting.ColorChangeSpeed);
        if (ToSprite != null)
        {
            (Graphic as Image).sprite = _initSprite;
        }
    }

    public override void Play()
    {
        ///非运行模式下
        if (!Application.isPlaying)
        {
            if (!_initColor.HasValue && Graphic != null)
            {
                _initColor = Graphic.color;
            }
            if (Graphic == null)
                return;
            Graphic.color = ToColor;
            if (ToSprite != null)
            {
                _initSprite = _initSprite == null ? (Graphic as Image).sprite : _initSprite;
                (Graphic as Image).sprite = ToSprite;
            }
            return;
         }


        if (!_initColor.HasValue&& Graphic!=null)
        {
            _initColor = Graphic.color;
        }
        if (Graphic == null)
            return;
        _lastTween?.Kill();
        _lastTween= Graphic.DOColor(ToColor,UIChangeSetting.ColorChangeSpeed);
        if (ToSprite != null)
        {
             _initSprite = _initSprite == null ? (Graphic as Image).sprite : _initSprite;
               (Graphic as Image).sprite = ToSprite;
        }
    }
 }
