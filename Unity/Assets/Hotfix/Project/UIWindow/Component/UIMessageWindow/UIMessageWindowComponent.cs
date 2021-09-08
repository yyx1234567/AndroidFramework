using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using ETModel;

namespace ETHotfix
{
    public enum ConfirmPanelType
    {
        EventType,
        QuitPanel,
        CompeletePanel,
        WrongPanel,
        ChangeLifter,
        CompeleteMission,
        JumpStep
    }

    public partial class UIMessageWindowComponent : UIWindowComponent
    {

        private void RegisterEvent()
        {
            Init();
        }
        public override void Init()
        {
            Collector.Get<GameObject>("EventPanel").GetComponent<UIWindow>().Init();
            Collector.Get<GameObject>("CompeletePanel").GetComponent<UIWindow>().Init();
            Collector.Get<GameObject>("ErrorPanel").GetComponent<UIWindow>().Init();
            Collector.Get<GameObject>("QuitPanel").GetComponent<UIWindow>().Init();

            Collector.Get<GameObject>("CompeletePanel").GetComponent<UIWindow>().CloseEvent += CloseHandle;
            _ConfrimBtn = Collector.GetMonoComponent<UIButtonOdin>("ConfirmBtn");
            _CancelBtn = Collector.GetMonoComponent<UIButtonOdin>("CancelBtn");
            _CloseBtn = Collector.GetMonoComponent<UIButtonOdin>("CloseBtn");
            CloseQuitBtn = Collector.GetMonoComponent<UIButtonOdin>("CloseQuitBtn");
            CancelQuitBtn = Collector.GetMonoComponent<UIButtonOdin>("CancelQuitBtn");

             _BackGround = Collector.GetMonoComponent<Image>("BackGround");
            ErrorPanelCloseBtn = Collector.GetMonoComponent<UIButtonOdin>("ErrorPanelCloseBtn");
            ErrorPanelCancelBtn = Collector.GetMonoComponent<UIButtonOdin>("ErrorPanelCancelBtn");
            ErrorPanelReSelectBtn = Collector.GetMonoComponent<UIButtonOdin>("ErrorPanelReSelectBtn");
            ConfirmQuitBtn = Collector.GetMonoComponent<UIButtonOdin>("ConfirmQuitBtn");

            _ConfrimBtn.ClickEvent += CloseHandle;
            _CloseBtn.ClickEvent += CloseHandle;
            CloseQuitBtn.ClickEvent += CloseHandle;
            CancelQuitBtn.ClickEvent += CloseHandle;
            ConfirmQuitBtn.ClickEvent += CloseHandle;
            _CancelBtn.ClickEvent += CloseHandle;
            ErrorPanelCloseBtn.ClickEvent += CloseHandle;
            ErrorPanelCancelBtn.ClickEvent += CloseHandle;
            ErrorPanelReSelectBtn.ClickEvent += CloseHandle;

        }

        UIButtonOdin ErrorPanelCloseBtn, ErrorPanelCancelBtn, ErrorPanelReSelectBtn;
        private void CloseHandle()
        {
            _BackGround.raycastTarget = false;
            _BackGround.DOFade(0, 0.3f);
        }

        private Image _BackGround;
        private Action _lastEvent;
        private Action _lastCancelEvent;
        private Action _lastErrorEvent;
        private Action _lastErrorCancelEvent;

        private UIButtonOdin _ConfrimBtn;
        private UIButtonOdin _CancelBtn;
        private UIButtonOdin _CloseBtn;
        private UIButtonOdin ConfirmQuitBtn;

        private UIButtonOdin CancelQuitBtn;
        private UIButtonOdin CloseQuitBtn;

          public void ShowPanel(Action act, string message, ConfirmPanelType type = ConfirmPanelType.EventType)
        {
            base.Show();
            _BackGround.raycastTarget = true;
            _BackGround.DOFade(0.3f, 0.3f);
            SetTypePanel(type, message);
            _ConfrimBtn.ClickEvent -= _lastEvent;
            _ConfrimBtn.ClickEvent += act;
            ConfirmQuitBtn.ClickEvent -= _lastEvent;
            ConfirmQuitBtn.ClickEvent += act;

            _lastEvent = act;
            GameObject.SetActive(true);
        }




        public void ShowPanel(Action act, Action act2, string message, ConfirmPanelType type = ConfirmPanelType.EventType)
        {
            base.Show();
            _BackGround.raycastTarget = true;
            _BackGround.DOFade(0.3f, 0.3f);
            SetTypePanel(type, message);
            switch (type)
            {
                case ConfirmPanelType.WrongPanel:
                    ErrorPanelReSelectBtn.ClickEvent -= _lastErrorEvent;
                    ErrorPanelReSelectBtn.ClickEvent += act;
                    ErrorPanelCancelBtn.ClickEvent -= _lastErrorCancelEvent;
                    ErrorPanelCancelBtn.ClickEvent += act2;
                    _lastErrorEvent = act;
                    _lastErrorCancelEvent = act2;
                    break;
                default:
                    _ConfrimBtn.ClickEvent -= _lastEvent;
                    _ConfrimBtn.ClickEvent += act;
                    _CancelBtn.ClickEvent -= _lastCancelEvent;
                    _CancelBtn.ClickEvent += act2;
                    _lastEvent = act;
                    _lastCancelEvent = act2;
                    break;
            }
            GameObject.SetActive(true);
        }


        private void SetTypePanel(ConfirmPanelType type, string message)
        {
            switch (type)
            {
                case ConfirmPanelType.EventType:
                    Collector.Get<GameObject>("EventPanel").transform.localPosition = _EndPos;
                    _EndPos = Vector3.zero;
                    Collector.Get<GameObject>("EventPanel").GetComponent<UIWindow>().ShowWindow();
                    Collector.Get<GameObject>("EventPanel").GetComponentInChildren<TMPro.TMP_Text>().text = message;
                    break;
                case ConfirmPanelType.CompeletePanel:
                    Collector.Get<GameObject>("CompeletePanel").transform.localPosition = _EndPos;
                    _EndPos = Vector3.zero;
                    Collector.Get<GameObject>("CompeletePanel").GetComponentInChildren<TMPro.TMP_Text>().text = message;
                    Collector.Get<GameObject>("CompeletePanel").GetComponent<UIWindow>().ShowWindow();
                    break;
                case ConfirmPanelType.WrongPanel:
                    Collector.Get<GameObject>("ErrorPanel").transform.localPosition = _EndPos;
                    _EndPos = Vector3.zero;
                    Collector.Get<GameObject>("ErrorPanel").GetComponent<UIWindow>().ShowWindow();
                    break;
                case ConfirmPanelType.QuitPanel:
                    Collector.Get<GameObject>("QuitPanel").transform.localPosition = _EndPos;
                    _EndPos = Vector3.zero;
                    Collector.Get<GameObject>("QuitPanel").GetComponent<UIWindow>().ShowWindow();
                    break;
            }
        }

        private Vector3 _EndPos;
        public void SetPosition(Vector3 pos)
        {
            _EndPos = pos;
        }
    }
}

