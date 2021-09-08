using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETModel
{
    public class UINumberSelect : MonoBehaviour
    {
        public string TextInfo;
        public delegate void UINumberSelectEvent(int number);
        public event UINumberSelectEvent OnNumberChangedEvent;
        public TMPro.TMP_Text NumberText;
        public UIButtonOdin NextBtn, LastBtn;

        public Action NextBtnEvent;
        public Action LastBtnEvent;
        private int _currentNumber;
        public int CurrentNumber
        {
            get { return _currentNumber; }
            set
            {
                value = Mathf.Clamp(value, MinNumber, MaxNumber);
                if (NumberText != null)
                {
                    NumberText.text = TextInfo + value.ToString();
                }
                _currentNumber = value;
                OnNumberChangedEvent?.Invoke(_currentNumber);
            }
        }
        public int MaxNumber = 10;
        public int MinNumber = 1;
        private void Awake()
        {
            NextBtn.ClickEvent += NextNumber;
            LastBtn.ClickEvent += LastNumber;
            CurrentNumber = MinNumber;
        }

        public void NextNumber()
        {
            CurrentNumber++;
            NextBtnEvent?.Invoke();
        }

        public void LastNumber()
        {
            CurrentNumber--;
            LastBtnEvent?.Invoke();
        }
    }
}