using ETModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ETHotfix
{
    public class UIWindowComponent : Component
    {
        public string m_Hierarchy;

        private   ReferenceCollector _collector;
        public   ReferenceCollector Collector
        {
            get
            {
                if (_collector == null)
                {
                      _collector = GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
                }
                return _collector;
            }
        }
 
        private Canvas _canvas;
        public Canvas m_Canvas
        {
            get
            {
                if (_canvas == null)
                    _canvas = GetParent<UI>().GameObject.GetComponent<Canvas>();
                return _canvas;
            }
        }


        public Sprite GetSprite(string textureName)
        {
            Texture2D texture = Collector.Get<Texture2D>(textureName);
            Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sp;
        }

 
        protected virtual void Show()
        {
             GetParent<UI>().GameObject.SetActive(true);
             Game.Scene.GetComponent<UIComponent>().UpdateSortingOrder(this);
         }

        protected virtual void Hide()
        {
             GetParent<UI>().GameObject.SetActive(false);
        }

        public void SetActive(bool active)
        {
            if (active)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }
        public virtual void ResetUI(List<string> itemlist) { }
        public virtual void Init() 
        {
          
        }
    }
}