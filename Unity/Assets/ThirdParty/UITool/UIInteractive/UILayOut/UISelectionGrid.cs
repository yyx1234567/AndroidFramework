using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public struct SelectionStyle
{
    public List<Sprite> SpriteList;

    public List<string> TextList;

    public string SpriteTarget;

    public ArrangementType m_ArrangementType;
}

public enum ArrangementType
{ 
   Cycle
}

public class UISelectionGrid : MonoBehaviour
{
    public GameObject Prefab;
    [HideInInspector]
    public List<Sprite> SpriteList = new List<Sprite>();
    [HideInInspector]
    public int SpriteIndex;
    private List<GameObject> m_GameObjectPool = new List<GameObject>();

    [HideInInspector]
    public List<GameObject> m_ShowList = new List<GameObject>();
    [HideInInspector]
    public Action<UnityEngine.UI.Button> ItemEvent;
    public Action<UnityEngine.UI.Toggle> ItemToggleEvent;

    public float MaxTextSize;
    private int m_CurrentCount;

    private float _itemWidth;
    private void Awake()
    {
        if (Prefab != null)
            Prefab.SetActive(false);
    }

    /// <summary>
    /// 添加对象
    /// </summary>
    /// <param name="count"></param>
    public void Add(int count)
    {
        Prefab.SetActive(true);
        for (int i = m_CurrentCount; i < count; i++)
        {
            GameObject temp = Instantiate(Prefab);
            temp.transform.SetParent(Prefab.transform.parent, false);
            temp.gameObject.SetActive(false);
            m_GameObjectPool.Add(temp);
            if (ItemEvent != null)
            {
                var btn = temp.GetComponentInChildren<Button>();
                btn.onClick.AddListener(() => { 
                    ItemEvent?.Invoke(btn);
                 });
            }
            if (ItemToggleEvent != null)
            {
                var btn = temp.GetComponentInChildren<Toggle>();
                btn.onValueChanged.AddListener((x) => {
                    ItemToggleEvent?.Invoke(btn);
                });
            }
        }
        Prefab.SetActive(false);
    }

    /// <summary>
    /// 显示
    /// </summary>
    /// <param name="ItemName"></param>
    public void Show(SelectionStyle selection)
    {
        if (!((selection.TextList == null || selection.TextList.Count == 0 )&&( selection.SpriteList == null || selection.SpriteList.Count == 0)))
        {
            int count = selection.TextList == null || selection.TextList.Count == 0 ? selection.SpriteList.Count : selection.TextList.Count;
            int delta = count - m_CurrentCount;

            if (delta > 0)
            {
                Add(count);
                m_CurrentCount = count;
            }
        }
         ///显示对象
        m_ShowList.Clear();
        for (int i = 0; i < m_GameObjectPool.Count; i++)
        {
            m_GameObjectPool[i].gameObject.SetActive(false);
        }

        if (selection.TextList != null)
        {
            MaxTextSize = 0;
            for (int i = 0; i < selection.TextList.Count; i++)
            {
                {
                    var text = m_GameObjectPool[i].GetComponentInChildren<TMPro.TMP_Text>();
                    if (text != null)
                    {
                        var rect = text.transform.parent.GetComponent<RectTransform>();
                        m_GameObjectPool[i].GetComponentInChildren<TMPro.TMP_Text>().text = selection.TextList[i];
                        float witdth = _itemWidth == 0 ? rect.rect.width : _itemWidth;
                        _itemWidth = witdth;
                        if (text.preferredWidth > _itemWidth)
                        {
                            rect.sizeDelta = new Vector2(_itemWidth + 18, text.preferredHeight + 10);
                        }
                        else
                        {
                            rect.sizeDelta = new Vector2(text.preferredWidth + 18, rect.rect.height);
                        }
                    }
                }
                {
                    var text = m_GameObjectPool[i].GetComponentInChildren<Text>();
                    if (text != null)
                    {
                        var rect = text.transform.parent.GetComponent<RectTransform>();
                        m_GameObjectPool[i].GetComponentInChildren<Text>().text = selection.TextList[i];
                        float witdth = _itemWidth == 0 ? rect.rect.width : _itemWidth;
                        _itemWidth = witdth;
                        if (text.preferredWidth > _itemWidth)
                        {
                            rect.sizeDelta = new Vector2(_itemWidth + 18, text.preferredHeight + 10);
                        }
                        else
                        {
                            rect.sizeDelta = new Vector2(text.preferredWidth + 18, rect.rect.height);
                        }
                        if (text.preferredWidth > MaxTextSize)
                        {
                            MaxTextSize = text.preferredWidth;
                        }
                    }
                }
                m_GameObjectPool[i].transform.localScale = Vector3.zero;
                m_GameObjectPool[i].gameObject.SetActive(true);
                m_ShowList.Add(m_GameObjectPool[i]);
            }
        }
         if (selection.SpriteList != null)
        {
             for (int i = 0; i < selection.SpriteList.Count; i++)
            {
                var imageTarget = string.IsNullOrEmpty(selection.SpriteTarget) ? m_GameObjectPool[i].transform : m_GameObjectPool[i].transform.Find(selection.SpriteTarget);
                if (imageTarget != null && imageTarget.GetComponent<Image>() != null)
                {
                    imageTarget.GetComponent<Image>().sprite = selection.SpriteList[i];
                    imageTarget.GetComponent<Image>().SetNativeSize();
                }
                if (SpriteList.Count > 0)
                {
                    m_GameObjectPool[i].GetComponentsInChildren<Image>()[SpriteIndex].sprite = SpriteList[i];
                }
                if (selection.TextList == null)
                {
                    m_GameObjectPool[i].transform.localScale = Vector3.zero;
                    m_GameObjectPool[i].gameObject.SetActive(true);
                    m_ShowList.Add(m_GameObjectPool[i]);
                }
            }
        }
        switch (selection.m_ArrangementType)
        {
            case ArrangementType.Cycle:
                Invoke("CycleArrangement", 0.05f);
                break;
        }
        if (gameObject.activeSelf)
        {
            Invoke("ShowAnimation",0.05f);
        }
    }

    private void CycleArrangement()
    {
        var child = m_ShowList[0];
        bool single = m_ShowList.Count % 2 != 0;
         var vertical = child.transform.parent.GetComponent<VerticalLayoutGroup>();
        if (vertical != null )
        {
            vertical.enabled = true;
            vertical.SetLayoutVertical();
            vertical.enabled = false;
        }
         float radius = 300;

        if (single)
        {
            for (int i = 0; i < m_ShowList.Count; i++)
            {
                var item = m_ShowList[i].transform;
               // int offset =Mathf.Abs( i - middle);
                float offset = Mathf.Sqrt(Mathf.Pow(radius,2)-Mathf.Pow(item.localPosition.y,2));
                item.localPosition = new Vector3(offset-320, item.localPosition.y, item.localPosition.z); ;
             }
        }
        else
        {
            for (int i = 0; i < m_ShowList.Count; i++)
            {
                var item = m_ShowList[i].transform;
                item.localPosition = new Vector3(-i * 50, item.localPosition.y, item.localPosition.z); ;
            }
        }
    }



    public void ShowAnimation()
    {
        StopCoroutine("ShowAnimationIE");
        StartCoroutine("ShowAnimationIE");
    }

    public IEnumerator ShowAnimationIE()
    {
        foreach (var item in m_ShowList)
        {
            if (gameObject.activeSelf)
                item.transform.DOScale(Vector3.one, 0.2f);
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void CloseAnimationIE()
    {
        foreach (var item in m_ShowList)
        {
            if (gameObject.activeSelf)
                item.transform.DOScale(Vector3.zero, 0.1f);
            //yield return null;
        }
     }



    /// <summary>
    /// 关闭
    /// </summary>
    public void Close()
    {
        CloseAnimationIE();
        // StopCoroutine("CloseAnimationIE");
        //StartCoroutine("CloseAnimationIE");
    }

    public void Clean()
    {
        m_CurrentCount = 0;
        for (int i = 0; i < m_ShowList.Count; i++)
        {
            DestroyImmediate(m_ShowList[i]);
        }
        m_ShowList.Clear();



        for (int i = 0; i < m_GameObjectPool.Count; i++)
        {
            DestroyImmediate(m_GameObjectPool[i]);
        }
        m_GameObjectPool.Clear();
    }
}
