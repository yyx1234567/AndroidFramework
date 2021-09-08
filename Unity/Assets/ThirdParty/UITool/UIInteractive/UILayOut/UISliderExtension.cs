using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISliderExtension : MonoBehaviour, IEndDragHandler, IDragHandler, IBeginDragHandler
{
     [Header("Content组件")]
     public GameObject content;
     public int nodeCount = 0;
     public float nodeWidth = 0;
     float curTouchPos = 0;
     float lastTouchPos = 0;
     bool switchTargetIsLeft = false;

    public Transform UIBtnListContent;
    void Start()
    {
        GetMaxNodeCount();
     }

    private bool _dragStart;
     public void OnBeginDrag(PointerEventData eventData)
    {
        if(content.GetComponent<UnityEngine.UI.HorizontalLayoutGroup>()!=null)
        DestroyImmediate(content.GetComponent<UnityEngine.UI.HorizontalLayoutGroup>());
         lastTouchPos = eventData.position.x;
        curTouchPos = eventData.position.x;
        _dragStart = true;
    }

     public void OnDrag(PointerEventData eventData)
    {
 
        if (eventData.pointerCurrentRaycast.gameObject.name != "Viewport")
        {
            return;
        }

        int index =Math.Abs( (int)content.transform.localPosition.x / (int)nodeWidth);
  
 
        if (content.transform.position.x > 0&& _dragStart)
        {
            _dragStart = false;
            var childcount = content.transform.childCount;
            var taget = content.transform.GetChild(childcount-1);
            taget.SetAsFirstSibling();
            taget.localPosition = new Vector3(-nodeWidth, taget.localPosition.y, taget.localPosition.z);
            var childlist = new List<Transform>();
            for (int i = 0; i < childcount; i++)
            {
                childlist.Add(content.transform.GetChild(0));
                content.transform.GetChild(0).transform.SetParent(null);
            }

            content.transform.localPosition = new Vector3(content.transform.localPosition.x - nodeWidth, content.transform.localPosition.y, content.transform.localPosition.z);

            foreach (Transform item in childlist)
            {
                item.SetParent(content.transform);
            }
        }

        if (Math.Abs(content.transform.position.x) > Math.Abs((nodeCount -1)* (nodeWidth)) && _dragStart)
        {
             _dragStart = false;
            var taget = content.transform.GetChild(0);
            taget.SetAsLastSibling();
            taget.localPosition = new Vector3((index+1)*nodeWidth, taget.localPosition.y, taget.localPosition.z);
            var childlist = new List<Transform>();
            var childcount = content.transform.childCount;
            for (int i = 0; i < childcount; i++)
            {
                childlist.Add(content.transform.GetChild(0));
                content.transform.GetChild(0).transform.SetParent(null);
            }
 
             content.transform.localPosition = new Vector3(content.transform.localPosition.x + nodeWidth, content.transform.localPosition.y, content.transform.localPosition.z);
             
            foreach (Transform item in childlist)
            {
                item.SetParent(content.transform);
            }
         }
        //if (content.transform.position.x > (nodeWidth / 2))
        //{
        //    return;
        //}
        curTouchPos = eventData.position.x;
        
        var delta = lastTouchPos - curTouchPos;
         if (delta < 0)
            switchTargetIsLeft = true;
        else if (delta > 0)
            switchTargetIsLeft = false;
         content.transform.position =new Vector3(content.transform.position.x- delta, content.transform.position.y, content.transform.position.z);
        lastTouchPos = eventData.position.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        int index = (int)content.transform.localPosition.x / (int)nodeWidth;
        SetIndexEffect();
        if (content.transform.localPosition.x > 0)
        {
            content.transform.DOLocalMoveX(0, 0.3f);
        }
        else
        {
            if (!switchTargetIsLeft)
            {
                 index +=-1;
                if (Math.Abs(index) >= nodeCount)
                {
                    index += 1;
                }
            }
            content.transform.DOLocalMoveX(index*nodeWidth, 0.3f);
         }
        Debug.LogError($"index  {index}");
      }
    
     public int GetMaxNodeCount()
    {
         UpdateNodeData();
        return nodeCount;
    }

  
     void UpdateNodeData()
    {
         int x = 0;
        List<string> names = new List<string>();
        for (int i = 0; i < content.transform.childCount; i++)
        {
            if (content.transform.GetChild(i).gameObject.activeSelf)
            {
                names.Add("");
                x++;
            }
        }
        nodeCount = x;

        if (UIBtnListContent != null)
        {
            UIBtnListContent.GetComponent<UIGrid>().Show(names);
            DestroyImmediate(UIBtnListContent.GetChild(0).gameObject);
            UIBtnListContent.GetChild(0).GetComponent<UIToggleOdin>().IsOn = true;
         }

         if(nodeCount>0)
        nodeWidth = content.transform.GetChild(0).GetComponent<RectTransform>().rect.width;
    }

    int currentIndex = 0;
    public void SetIndexEffect()
    {
        if (UIBtnListContent == null)
            return;
   
        if (switchTargetIsLeft)
        {
            currentIndex -= 1;
            if (currentIndex < 0)
            {
                currentIndex = nodeCount - 1;
            }
        }
        else
        {
             currentIndex += 1;

            if (currentIndex >= nodeCount)
            {
                currentIndex = 0;
            }
         }
         UIBtnListContent.GetChild(currentIndex).GetComponent<UIToggleOdin>().IsOn = true;
     }

 
 }
 