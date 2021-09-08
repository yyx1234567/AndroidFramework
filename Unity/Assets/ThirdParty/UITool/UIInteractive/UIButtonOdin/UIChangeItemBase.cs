using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ButtonState
{
    Normal,
    HighLight,
    Press,
    Disable,
    PressUp,
}

public static class UIChangeSetting
{
    public const float ColorChangeSpeed = 0.3f;
}


[HideInInspector]
public abstract class UIChangeItemBase:IUIButtonEffect
{
    [HideInInspector]
    public MaskableGraphic Graphic;
    [HideInInspector]
    public Sprite ToSprite;
    [HideInInspector]
    public Color ToColor = Color.white;
    public abstract void Back();

    public abstract void Play();



    public virtual void Init() { }
 }
