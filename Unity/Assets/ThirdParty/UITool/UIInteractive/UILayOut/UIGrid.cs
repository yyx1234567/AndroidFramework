using DG.Tweening;
using ETModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

     public class UIGrid : MonoBehaviour
    {
        public GameObject Prefab;
        public GameObject Space;
        public UIGrid Child;
        public List<Sprite> SpriteList = new List<Sprite>();
        public int SpriteIndex;
        private List<GameObject> m_GameObjectPool = new List<GameObject>();

        [HideInInspector]
        public List<GameObject> m_ShowList = new List<GameObject>();
        [HideInInspector]
        public List<GameObject> SpaceList = new List<GameObject>();
        public Action<UIToggleOdin> ItemEvent;
        private int m_CurrentCount;

        private void Awake()
        {
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
                    temp.GetComponentInChildren<UIToggleOdin>().ToggleEventSelf += ItemEvent;
                if (Space != null)
                {
                    GameObject space = Instantiate(Space);
                    space.transform.SetParent(Prefab.transform.parent, false);
                    SpaceList.Add(space);
                }
            }
            if (Space != null)
                Space.SetActive(false);
            Prefab.SetActive(false);
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="ItemName"></param>
        public void Show(List<string> ItemName)
        {
             int delta = ItemName.Count - m_CurrentCount;
            if (delta > 0)
            {
                Add(ItemName.Count);
                m_CurrentCount = ItemName.Count;
            }

            ///显示对象
            m_ShowList.Clear();
            for (int i = 0; i < m_GameObjectPool.Count; i++)
            {
                m_GameObjectPool[i].gameObject.SetActive(false);
            }
            if (SpaceList.Count > 0)
            {
                for (int i = 0; i < m_GameObjectPool.Count; i++)
                {
                    SpaceList[i].gameObject.SetActive(false);
                }
            }
            for (int i = 0; i < ItemName.Count; i++)
            {
                if (m_GameObjectPool[i].GetComponentInChildren<Text>() != null)
                    m_GameObjectPool[i].GetComponentInChildren<Text>().text = ItemName[i];
            if (m_GameObjectPool[i].GetComponentInChildren<TMPro.TMP_Text>() != null)
                m_GameObjectPool[i].GetComponentInChildren<TMPro.TMP_Text>().text = ItemName[i];

            m_GameObjectPool[i].transform.localScale = Vector3.zero;
                m_GameObjectPool[i].gameObject.SetActive(true);
                if (SpaceList.Count > 0)
                {
                    SpaceList[i].gameObject.SetActive(true);
                }
                if (SpriteList.Count > 0)
                {
                    m_GameObjectPool[i].GetComponentsInChildren<Image>()[SpriteIndex].sprite = SpriteList[i];
                }
                m_ShowList.Add(m_GameObjectPool[i]);
            }
            if (gameObject.activeSelf)
            {
                ShowAnimation();
            }
        }

        public void ShowAnimation()
        {
            ShowAnimationIE();
        }

        public async void ShowAnimationIE()
        {
             foreach (var item in m_ShowList)
            {
                if (gameObject.activeSelf)
                    item.transform.DOScale(Vector3.one, 0f);
                //await timer.WaitAsync(100);
            }
        }


        /// <summary>
        /// 显示带图片的
        /// </summary>
        /// <param name="ItemName"></param>
        public void Show(List<string> ItemName, List<Sprite> sprites, string imageName)
        {
             int delta = ItemName.Count - m_CurrentCount;
            if (delta > 0)
            {
                Add(ItemName.Count);
                m_CurrentCount = ItemName.Count;
            }


            ///显示对象
            m_ShowList.Clear();
            for (int i = 0; i < m_GameObjectPool.Count; i++)
            {
                m_GameObjectPool[i].gameObject.SetActive(false);
            }
            if (SpaceList.Count > 0)
            {
                for (int i = 0; i < m_GameObjectPool.Count; i++)
                {
                    SpaceList[i].gameObject.SetActive(false);
                }
            }
            for (int i = 0; i < ItemName.Count; i++)
            {
                if (m_GameObjectPool[i].GetComponentInChildren<Text>() != null)
                    m_GameObjectPool[i].GetComponentInChildren<Text>().text = ItemName[i];
                if (m_GameObjectPool[i].GetComponentInChildren<TMPro.TMP_Text>() != null)
                   m_GameObjectPool[i].GetComponentInChildren<TMPro.TMP_Text>().text = ItemName[i];
                m_GameObjectPool[i].transform.Find(imageName).GetComponent<Image>().sprite = sprites[i];
                m_GameObjectPool[i].transform.Find(imageName).GetComponent<Image>().SetNativeSize();
                m_GameObjectPool[i].gameObject.SetActive(true);
                if (SpaceList.Count > 0)
                {
                    SpaceList[i].gameObject.SetActive(true);
                }
                m_ShowList.Add(m_GameObjectPool[i]);
            }
            if (gameObject.activeSelf)
            {
                ShowAnimation();
            }
        }


        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            for (int i = 0; i < m_ShowList.Count; i++)
            {
                m_ShowList[i].SetActive(false);
            }
            for (int i = 0; i < SpaceList.Count; i++)
            {
                SpaceList[i].SetActive(false);
            }
        }

        public void Clean()
        {
            m_CurrentCount = 0;
            for (int i = 0; i < m_ShowList.Count; i++)
            {
                DestroyImmediate(m_ShowList[i]);
            }
            m_ShowList.Clear();

            for (int i = 0; i < SpaceList.Count; i++)
            {
                DestroyImmediate(SpaceList[i]);
            }
            SpaceList.Clear();

            for (int i = 0; i < m_GameObjectPool.Count; i++)
            {
                DestroyImmediate(m_GameObjectPool[i]);
            }
            m_GameObjectPool.Clear();
        }
    }
 