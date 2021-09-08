using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    public interface IPanelSelectHandle<T>
    {
        System.Action<T> ConfirmAction { get; set; }
    }
    public class UINumberSet : MonoBehaviour, IPanelSelectHandle<int>
    {
        private int _maxNumber = 999;

        public Button AddBtn, MinusBtn, ConfirmBtn, CacnelBtn;

        public InputField Input;

        public Text TargetText, MaxNumberText;

        public Image TargetImage;


        private int _currentNumber;
        public int CurrentNumber
        {
            get
            {
                return _currentNumber;
            }
            set
            {
                _currentNumber = value;
                if (_currentNumber < 0)
                    _currentNumber = 0;
                if (_currentNumber > _maxNumber)
                    _currentNumber = _maxNumber;
                Input.text = CurrentNumber.ToString();
            }
        }

        public Action<int> ConfirmAction
        {
            get;
            set;
        }

        private void Awake()
        {
            AddBtn.onClick.AddListener(() => { CurrentNumber++; });
            MinusBtn.onClick.AddListener(() => { CurrentNumber--; });
            Input.onValueChanged.AddListener((x) =>
            {
                CurrentNumber = int.Parse(x);
                Input.text = CurrentNumber.ToString();
            });
            ConfirmBtn.onClick.AddListener(
               () =>
               {
                   GetComponent<UIWindow>().CloseWindow();
                   ConfirmAction.Invoke(CurrentNumber);
               });
            CacnelBtn.onClick.AddListener(() =>
            {
                CurrentNumber = 0;
            });
        }

        public void Show(string text, Sprite sprite, int MaxNumber = 999)
        {
            CurrentNumber = 0;
            _maxNumber = MaxNumber;
            TargetText.text = text;
            TargetImage.sprite = sprite;
            if (MaxNumber < 0)
                MaxNumber = 0;
            if (MaxNumber != 999)
            {
                MaxNumberText.text = $"库存 {MaxNumber} 件";
            }
        }

        public void OnPanelValueSelect()
        {

        }
    }
}