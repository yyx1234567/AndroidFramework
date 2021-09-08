using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class OperateHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{

    public Action<PointerEventData>  PointerClickEvent;
    public Action<PointerEventData>  PointerDownEvent;
    public Action<PointerEventData>  PointerEnterEvent;
    public Action<PointerEventData>  PointerExitEvent;
    public Action<PointerEventData>  PointerUpEvent;
    public void OnPointerClick(PointerEventData eventData)
    {
        PointerClickEvent?.Invoke(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PointerDownEvent?.Invoke(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnterEvent?.Invoke(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExitEvent?.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PointerUpEvent?.Invoke(eventData);
    }
}
