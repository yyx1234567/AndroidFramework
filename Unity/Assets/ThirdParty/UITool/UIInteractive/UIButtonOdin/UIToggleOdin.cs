using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ToggleState
{
    IsOn,
    UnIsOn
}

public class UIToggleOdin : UISelectable
{
    [LabelText("点击叠加色")]
    public Color PressColor = new Color(0.9f, 0.9f, 0.9f, 1);

     private Dictionary<MaskableGraphic, Color> graphicDic = new Dictionary<MaskableGraphic, Color>();


    [LabelText("ToggleState")]
    public Dictionary<ToggleState, List<UIChangeItemBase>> ToggleStateDic = new Dictionary<ToggleState, List<UIChangeItemBase>>();

    [HideInInspector]
    public Action<bool> ToggleEvent;
    [HideInInspector]
    public Action<UIToggleOdin> ToggleEventSelf;

    public UIToggleGroupOdin Group;
    private bool _inited = false;
    public override void Init()
    {
        if (_inited)
            return;
        _inited = true;
        if (Group != null && !Group.ToggleList.Contains(this))
        {
            Group.ToggleList.Add(this);
        }
        if (m_IsOn)
        {
            IsOn=true;
            if (Group != null)
            {
                if (!Group.ActiveToggle.Contains(this))
                {
                    Group.ActiveToggle.Add(this);
                    Group.SetLastToggle(this);
                }
            }
         }
     }

    public override bool Interactable
    {
        get { return m_Interactable; }
        set
        {
            m_Interactable = value;
            if (m_Interactable)
            {
                ChangeState(ButtonState.Normal);
            }
            else
            {
                if (!IsOn)
                    ChangeState(ButtonState.Disable);
            }
            if (IsOn)
            {
                ChangeState(ToggleState.IsOn,false);
            }
         }
    }

#if UNITY_EDITOR
    [OnValueChanged("SetToggleState")]
#endif
    public bool m_IsOn;
#if UNITY_EDITOR
    private void SetToggleState()
    {
        ChangeState(m_IsOn==true? ToggleState.IsOn: ToggleState.UnIsOn, false);
        //UnityEngine.UI.LayoutUtility.upda
    }
#endif
    public bool IsOn
    {
        get { return m_IsOn; }
        set
        {
            if (m_IsOn != value)
            {
                m_IsOn = value;
                SetState(m_IsOn == true ? ToggleState.IsOn : ToggleState.UnIsOn);
            }
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (IsOn)
            return;
        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (IsOn)
            return;
        base.OnPointerExit(eventData);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!Interactable)
            return;
        OnValueChanged();
     }

    public void OnValueChanged()
    {
         if (Group != null)
        {
            if (!Group.ChangeState(this))
                return;
        }
        m_IsOn = !m_IsOn;
        if (m_IsOn)
        {
            ChangeState(ToggleState.IsOn);
        }
        else
        {
            ChangeState(ToggleState.UnIsOn);
        }
    }

    public void SetState(ToggleState state)
    {
        if (Group != null)
        {
            if (!Group.SetState(this))
            {
                 m_IsOn = !m_IsOn;
                return;
            }
        }
         ChangeState(state);
     }

    public void ChangeState(ToggleState state,bool invokeEvent=true)
    {
        PlayAnimation(state);
        if(invokeEvent)
        InvokeEvent();
    }

    public void PlayAnimation(ToggleState state)
    {
        if (!ToggleStateDic.ContainsKey(state))
            return;
        foreach (var item in ToggleStateDic[state])
        {
            item.Play();
        }
    }

    public void InvokeEvent()
    {
        ToggleEvent?.Invoke(m_IsOn);
        ToggleEventSelf?.Invoke(this);
    }

    public override void ChangeState(ButtonState state)
    {
         base.ChangeState(state);
        //switch (state)
        //{
        //    case ButtonState.Press:
        //        Blend_color(PressColor, gameObject);
        //        break;
        //    case ButtonState.PressUp:
        //        ReSetColor(gameObject);
        //        break;
        //}
    }
    private void ReSetColor(GameObject target)
    {
        var images = target.GetComponentsInChildren<MaskableGraphic>();
        foreach (var item in images)
        {
            if (graphicDic.ContainsKey(item))
                item.DOColor(graphicDic[item], duration: 0.2F);
        }
    }

    private void Blend_color(Color color, GameObject target)
    {
        graphicDic.Clear();
        var images = target.GetComponentsInChildren<MaskableGraphic>();
        foreach (var item in images)
        {
            if (!graphicDic.ContainsKey(item))
            {
                graphicDic.Add(item, item.color);
            }
            item.DOColor(item.color * color, duration: 0.2F);
        }
    }
}
