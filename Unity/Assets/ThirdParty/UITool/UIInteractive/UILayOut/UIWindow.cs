 using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
  using System.Threading.Tasks;
using System.Linq;
using System;

namespace ETModel
{
    public class UIWindow : MonoBehaviour
    {
        public UIToggleOdin ControlToggle;

        public List<UIButtonOdin> OpenBtn;

        public List<UIButtonOdin> CloseBtn;
  
        private List<UIButtonOdin> _activeBtn = new List<UIButtonOdin>();

        public Action OpenEvent;

        public Action CloseEvent;

        public void Init()
        {
            if (GetComponent<CanvasGroup>() == null)
            {
                gameObject.AddComponent<CanvasGroup>();
            }
            _activeBtn = GetComponentsInChildren<UIButtonOdin>().ToList();
            CloseImmediate();
             if (ControlToggle != null)
            {
                  ControlToggle.ToggleEvent += ControlToggleHandle;
            }
            if (OpenBtn != null)
            {
                foreach (var item in OpenBtn)
                {
                    if (item != null)
                        item.ClickEvent += OpenBtnHandle;
                }
            }
            if (CloseBtn != null)
            {
 
                foreach (var item in CloseBtn)
                {
                     if (item != null)
                        item.ClickEvent += CloseBtnHandle;
                }
            }
            gameObject.SetActive(false);
        }

        public void SetOpenBtn(UIButtonOdin btn)
        {
            if (!OpenBtn.Contains(btn))
            {
                OpenBtn.Add(btn);
            }
        }

        public void SetToggleBtn(UIToggleOdin toggle)
        {
            ControlToggle = toggle;
        }

        private void CloseBtnHandle()
        {
            if (ControlToggle != null)
            {
 
                ControlToggle.IsOn = false;
            }
            else
            {
                CloseWindow();
            }
        }

        private void OpenBtnHandle()
        {
            if (ControlToggle != null)
            {
                ControlToggle.IsOn = true;
            }
            else
            {
                ShowWindow();
            }
        }

        private void ControlToggleHandle(bool arg)
        {
            if (arg)
            {
                ShowWindow();
            }
            else
            {
                CloseWindow();
            }
        }

        public void ShowWindow(bool Setscale=false)
        {
            OpenEvent?.Invoke();
            transform.SetAsLastSibling();
            gameObject.SetActive(true);
            if(Setscale)
            transform.DOScale(Vector3.one * 0.8f, 0);
            GetComponent<CanvasGroup>().DOFade(1,0.3f);
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            transform.DOScale(Vector3.one, 0.3f);
             foreach (var item in _activeBtn)
            {
                item.enabled = true;
            }
        }

        public async void CloseWindow(bool setscale=false)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            if(setscale)
            transform.DOScale(Vector3.one * 0.9f, 0.3f);
            GetComponent<CanvasGroup>().DOFade(0, 0.3f);
            foreach (var item in _activeBtn)
            {
                item.enabled = false;
            }
            await Task.Delay(300);
            gameObject.SetActive(false);
            CloseEvent?.Invoke();
        }

        public void CloseImmediate(bool setscale=false)
        {
            if(setscale)
            transform.DOScale(Vector3.one * 0.9f, 0);
            GetComponent<CanvasGroup>().DOFade(0, 0);
         }
    }
}