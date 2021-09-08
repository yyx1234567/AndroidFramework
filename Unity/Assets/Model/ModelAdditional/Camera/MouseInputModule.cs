using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
/// <summary>
/// 鼠标输入模块
/// </summary>
public class MouseInputModule : StandaloneInputModule
{
    /// <summary>
    /// 鼠标左键信息
    /// </summary>
    public PointerEventData LeftPointerEventData
    {
        get
        {
            if (base.m_PointerData.ContainsKey(kMouseLeftId))
                return base.m_PointerData[kMouseLeftId];
            else
                return null;
        }
    }

    /// <summary>
    /// 鼠标中键信息
    /// </summary>
    public PointerEventData MiddlePointerEventData
    {
        get
        {
            if (base.m_PointerData.ContainsKey(kMouseMiddleId))
                return base.m_PointerData[kMouseMiddleId];
            else
                return null;
        }
    }

    /// <summary>
    /// 鼠标右键信息
    /// </summary>
    public PointerEventData RightPointerEventData
    {
        get
        {
            if (base.m_PointerData.ContainsKey(kMouseRightId))
                return base.m_PointerData[kMouseRightId];
            else
                return null;
        }
    }

    /// <summary>
    /// 所有按键信息
    /// </summary>
    public Dictionary<int, PointerEventData> PointerData
    {
        get { return base.m_PointerData; }
    }
}
