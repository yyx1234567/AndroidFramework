using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Coffee.UIExtensions;
using System;
using UnityEngine.EventSystems;
namespace ETModel
{
    public class UICycleBtnLayOut : MonoBehaviour
    {
        private int _CenterIndex;
        public int CenterIndex
        {
            get { return _CenterIndex; }
            set
            {
                if (value >= transform.childCount)
                {
                    _CenterIndex = 0;
                }
                else
                if (value < 0)
                {
                    _CenterIndex = transform.childCount - 1;
                }
                else
                {
                    _CenterIndex = value;
                }
            }
        }

        private List<Vector3> _RecordPosList = new List<Vector3>();

        private List<Transform> _tranformList = new List<Transform>();

        private Tween LastTween;

        public Transform RightPos, LeftPos;

        public List<Tween> Tweens = new List<Tween>();

        private bool IsDrag;
        private void Start()
        {
            foreach (Transform item in transform)
            {
                var btn = item.GetComponent<UIButtonOdin>();
                if (btn != null)
                {
                    btn.ClickEvent += () =>
                    {
                        switch (btn.transform.GetSiblingIndex())
                        {
                            case 1:
                            case 0:
                                float delta = Input.mousePosition.x - _lastPosX;
                                if (Mathf.Abs(delta) > 10)
                                {
                                    break;
                                }
                                 if (IsOnRight(btn.transform))
                                {
                                    MoveLeft();
                                }
                                else
                                {
                                    MoveRight();
                                }
                                break;
                        }
                    };

                }
            }
            CenterIndex = transform.childCount / 2;
            for (int i = 0; i < transform.childCount; i++)
            {
                _RecordPosList.Add(transform.GetChild(i).transform.localPosition);
                _tranformList.Add(transform.GetChild(i));
            }
            MoveRight();
        }


        private bool IsOnRight(Transform target)
        {
            foreach (Transform item in transform)
            {
                if (item != target)
                {
                    if (target.position.x < item.position.x)
                        return false;
                }
            }
            return true;
        }


        public void MoveRight()
        {
            foreach (var item in Tweens)
            {
                item.Kill();
            }
            Tweens.Clear();

            StopAllCoroutines();

            ///移动位置
            CenterIndex--;
            List<Vector3> temp = new List<Vector3>();
            for (int i = 0; i < _tranformList.Count; i++)
            {
                _tranformList[i].SetSiblingIndex(transform.childCount - 2);

                if (i == _tranformList.Count - 1)
                {
                    Tweens.Add(_tranformList[i].DOLocalMove(_RecordPosList[0], 1));
                    temp.Add(_RecordPosList[0]);
                }
                else
                {
                    Tweens.Add(_tranformList[i].DOLocalMove(_RecordPosList[i + 1], 1));
                    temp.Add(_RecordPosList[i + 1]);
                }
            }
            ///设置层级
            int index = CenterIndex - 1;
            if (index < 0)
                index = transform.childCount - 1;
            _tranformList[index].SetAsFirstSibling();
            //StartCoroutine(MoveOutScreen(_tranformList[index], LeftPos.position, RightPos.position, _RecordPosList[0]));
            _RecordPosList = temp;

            ///播放选择的模式的效果
            StartCoroutine(ShowCenterMode());
        }

        private IEnumerator MoveOutScreen(Transform target, Vector3 first, Vector3 sencod, Vector3 end)
        {
            target.DOLocalMove(first, 0.5f);
            yield return new WaitForSeconds(0.5f);
            target.position = sencod;
            target.DOLocalMove(end, 0.5f);
        }

        public void MoveLeft()
        {
            foreach (var item in Tweens)
            {
                item.Kill();
            }
            Tweens.Clear();
             StopAllCoroutines();

            ///移动位置
            List<Vector3> temp = new List<Vector3>();
            CenterIndex++;
            int index = CenterIndex + 1;
            if (index >= transform.childCount)
                index = 0;
            for (int i = 0; i < _tranformList.Count; i++)
            {
                _tranformList[i].SetSiblingIndex(transform.childCount - 2);
                if (i == 0)
                {
                    Tweens.Add(_tranformList[i].DOLocalMove(_RecordPosList[_RecordPosList.Count - 1], 1));
                    temp.Add(_RecordPosList[_RecordPosList.Count - 1]);
                }
                else
                {
                    Tweens.Add(_tranformList[i].DOLocalMove(_RecordPosList[i - 1], 1));
                    temp.Add(_RecordPosList[i - 1]);
                }
            }
            ///设置层级
            _tranformList[index].SetAsFirstSibling();

            //StartCoroutine(MoveOutScreen (_tranformList[index], LeftPos.position, RightPos.position, _RecordPosList[_RecordPosList.Count - 1]));

            _RecordPosList = temp;
            ///播放选择的模式的效果
            StartCoroutine(ShowCenterMode());
        }

        /// <summary>
        /// 显示中间的模式
        /// </summary>
        /// <returns></returns>
        private IEnumerator ShowCenterMode()
        {
            Transform center = _tranformList[CenterIndex];
            for (int i = 0; i < _tranformList.Count; i++)
            {
                UIGradient uIGradient = _tranformList[i].GetComponent<UIGradient>();
                UIGradient iconGradient = _tranformList[i].Find("Icon").GetComponent<UIGradient>();
                TMPro.TMP_Text Name = _tranformList[i].Find("Name").GetComponent<TMPro.TMP_Text>();
                TMPro.TMP_Text Info = _tranformList[i].Find("Info").GetComponent<TMPro.TMP_Text>();
                UIButtonOdin Button = _tranformList[i].Find("ConfirmBtn").GetComponent<UIButtonOdin>();

                if (i != CenterIndex)
                {
                    Tweens.Add(_tranformList[i].DOScale(1, 1f));
                    Tweens.Add(DOTween.To(() => uIGradient.color1, x => uIGradient.color1 = x, new Color(1, 1, 1, 0.5f), 0.3f));
                    Tweens.Add(DOTween.To(() => uIGradient.color2, x => uIGradient.color2 = x, new Color(1, 1, 1, 0.5f), 0.3f));
                    Tweens.Add(DOTween.To(() => iconGradient.color1, x => iconGradient.color1 = x, Color.blue, 1f));
                    Tweens.Add(DOTween.To(() => iconGradient.color2, x => iconGradient.color2 = x, Color.green, 1f));
                    Tweens.Add(Name.DOColor(Color.black, 0.3f));
                    Tweens.Add(Info.DOColor(Color.black, 0.3f));
                    Tweens.Add(Button.transform.DOScale(0f, 0.3f));
                    Tweens.Add(Info.transform.DOLocalMoveY(-150, 0.3f));
                }
                else
                {
                    Tweens.Add(Name.DOColor(Color.white, 0.3f));
                    Tweens.Add(Info.DOColor(Color.white, 0.3f));
                    Tweens.Add(DOTween.To(() => iconGradient.color1, x => iconGradient.color1 = x, Color.white, 0.3f));
                    Tweens.Add(DOTween.To(() => iconGradient.color2, x => iconGradient.color2 = x, Color.white, 0.3f));
                    Tweens.Add(DOTween.To(() => uIGradient.color2, x => uIGradient.color2 = x, new Color(102 / 255f, 216 / 255f, 181 / 255f), 1f));
                    Tweens.Add(DOTween.To(() => uIGradient.color1, x => uIGradient.color1 = x, new Color(87 / 255f, 173 / 255f, 234 / 255f), 1f));
                    _tranformList[i].SetAsLastSibling();
                }
            }
            Tweens.Add(center.DOScale(1.5f, 0.5f));
            yield return new WaitForSeconds(0.5f);
            Tweens.Add(center.DOScale(1.3334f, 0.5f));
            yield return new WaitForSeconds(0.5f);
            Tweens.Add(_tranformList[CenterIndex].Find("Info").transform.DOLocalMoveY(-220, 0.3f));
            yield return new WaitForSeconds(0.3f);
            Tweens.Add(_tranformList[CenterIndex].Find("ConfirmBtn").DOScale(1.2f, 0.3f));
            yield return new WaitForSeconds(0.3f);
            Tweens.Add(_tranformList[CenterIndex].Find("ConfirmBtn").DOScale(1f, 0.2f));
        }

        public string GetCurrentMode()
        {
            return _tranformList[CenterIndex].name;
        }


        public void ResetPanel()
        {
            switch (CenterIndex)
            {
                case 1:
                    MoveRight();
                    break;
                case 2:
                    MoveLeft();
                    break;
            }
        }

           /// <summary>
        /// 操作
        /// </summary>
        float _lastPosX;


        float timeCount = 0;
        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                timeCount = Time.time;
                 _lastPosX = Input.mousePosition.x;
             }
            else if (Input.GetMouseButtonUp(0))
            {
                float delta = Input.mousePosition.x - _lastPosX;
                if (Mathf.Abs(delta) > 10)
                {
                    if (delta > 0)
                    {
                        MoveRight();
                    }
                    else
                    {
                        MoveLeft();
                    }
                }
             }
        }

    
    }
}
