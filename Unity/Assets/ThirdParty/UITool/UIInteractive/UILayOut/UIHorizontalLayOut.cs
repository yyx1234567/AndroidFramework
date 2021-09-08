using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

using ETModel;
namespace ETModel
{
    public class UIHorizontalLayOut : MonoBehaviour
    {
        public ScrollRect m_ScrollRect;

        public Transform Content;

        public Transform IndexList;

        public UIButtonOdin ConfirmBtn;

        public Vector2 CellSize;

        public Vector2 CellScaleDSize;

        private Transform _target;

        public float screenWidth;
        public void Init()
        {
            screenWidth = 1920;
            GetComponent<Collider2D>().enabled = false;
            foreach (Transform item in Content)
            {
                if (item.GetComponent<UIButtonOdin>() != null)
                    item.GetComponent<UIButtonOdin>().ClickEvent += () => { ClickBtnHandle(item); };
            }
        }



        public void Show()
        {
            GetComponent<Collider2D>().enabled = false;
            ConfirmBtn.transform.localScale = Vector3.zero;
            Content.localPosition = new Vector3(0, Content.localPosition.y, 0);
            foreach (Transform item in Content)
            {
                if (item.GetComponent<UIButtonOdin>() != null)
                {
                    item.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
                    item.Find("Body").localScale = Vector3.zero;
                }
            }
            StartCoroutine(StartAnimation());
        }

        private IEnumerator StartAnimation()
        {
            foreach (Transform item in Content)
            {
                if (item.GetComponent<UIButtonOdin>() != null)
                {
                    item.GetComponent<RectTransform>().DOSizeDelta(CellSize, 0.5f);
                    item.Find("Body").DOScale(1, 0.5f);
                }
                yield return new WaitForSeconds(0.05f);
            }
            ConfirmBtn.transform.DOScale(Vector3.one * 1.2f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            GetComponent<Collider2D>().enabled = true;
            yield return new WaitForSeconds(0.5f);
            ConfirmBtn.transform.DOScale(Vector3.one, 0.3f);
        }

        private void ClickBtnHandle(Transform target)
        {
            StopCoroutine(ClickBtnAnimation(target));
            StartCoroutine(ClickBtnAnimation(target));
        }

        private IEnumerator ClickBtnAnimation(Transform target)
        {
            _target = target;
            m_ScrollRect.StopMovement();
            float center = screenWidth / 2f;
            float delta = _target.transform.position.x - center;
            Content.transform.DOLocalMoveX((_target.GetSiblingIndex() - 2) * -360 - screenWidth / 2, 0.3f);
            float x = Input.mousePosition.x;
            yield return new WaitForSeconds(0.3f);
            if (Mathf.Abs(x - Input.mousePosition.x) > 1000)
            {
                yield break;
            }
            OnDragEndEvent();
        }


        private Vector2 _IndexNormalSize = new Vector2(8, 8);
        private Vector2 _IndexSelectedSize = new Vector2(24, 8);
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.Find("Body/Index")==null)
            {
                return;
            }
            
            _target = collision.transform;
            collision.transform.GetComponent<RectTransform>().DOSizeDelta(CellScaleDSize, 0.3f);
            Coffee.UIExtensions.UIGradient MissionItemuIGradient = collision.transform.GetComponent<Coffee.UIExtensions.UIGradient>();
            DOTween.To(() => MissionItemuIGradient.color1, x => MissionItemuIGradient.color1 = x, Color.blue, 0.3f);
            DOTween.To(() => MissionItemuIGradient.color2, x => MissionItemuIGradient.color2 = x, Color.green, 0.3f);
            Coffee.UIExtensions.UIGradient indexGradient = collision.transform.Find("Body/Index").GetComponent<Coffee.UIExtensions.UIGradient>();
            DOTween.To(() => indexGradient.color1, x => indexGradient.color1 = x, Color.white, 0.3f);
            DOTween.To(() => indexGradient.color2, x => indexGradient.color2 = x, Color.white, 0.3f);
            collision.transform.Find("Body/Name").GetComponent<Text>().DOColor(Color.white, 0.3f);
            collision.transform.Find("Body/Shadow").GetComponent<Image>().DOFade(1, 0.3f);

            var bg = collision.transform.Find("Body/Bg").GetComponent<Coffee.UIExtensions.UIGradient>();
            DOTween.To(() => bg.color1, x => bg.color1 = x, Color.white, 0.3f);
            DOTween.To(() => bg.color2, x => bg.color2 = x, Color.white, 0.3f);



            Coffee.UIExtensions.UIGradient uILineGradient = collision.transform.Find("Body/Image").GetComponent<Coffee.UIExtensions.UIGradient>();
            DOTween.To(() => uILineGradient.color1, x => uILineGradient.color1 = x, Color.white, 0.3f);
            DOTween.To(() => uILineGradient.color2, x => uILineGradient.color2 = x, Color.white, 0.3f);

            foreach (Transform item in collision.transform)
            {
                item.transform.DOScale(Vector3.one * 1.2f, 0.3f);
            }
            Transform index = IndexList.GetChild(collision.transform.GetSiblingIndex() - 1);
            index.GetComponent<RectTransform>().DOSizeDelta(_IndexSelectedSize, 0.3f);
            Coffee.UIExtensions.UIGradient uIGradient = index.GetComponent<Coffee.UIExtensions.UIGradient>();
            DOTween.To(() => uIGradient.color1, x => uIGradient.color1 = x, Color.blue, 0.3f);
            DOTween.To(() => uIGradient.color2, x => uIGradient.color2 = x, Color.green, 0.3f);

        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.transform.Find("Body/Index") == null)
            {
                return;
            }

            _target = collision.transform;
            collision.transform.GetComponent<RectTransform>().DOSizeDelta(CellSize, 0.3f);
            foreach (Transform item in collision.transform)
            {
                item.transform.DOScale(Vector3.one, 0.3f);
            }
            Coffee.UIExtensions.UIGradient MissionItemuIGradient = collision.transform.GetComponent<Coffee.UIExtensions.UIGradient>();
            DOTween.To(() => MissionItemuIGradient.color1, x => MissionItemuIGradient.color1 = x, Color.white, 0.3f);
            DOTween.To(() => MissionItemuIGradient.color2, x => MissionItemuIGradient.color2 = x, Color.white, 0.3f);

            Coffee.UIExtensions.UIGradient indexGradient = collision.transform.Find("Body/Index").GetComponent<Coffee.UIExtensions.UIGradient>();
            DOTween.To(() => indexGradient.color1, x => indexGradient.color1 = x, Color.blue, 0.3f);
            DOTween.To(() => indexGradient.color2, x => indexGradient.color2 = x, Color.green, 0.3f);
            collision.transform.Find("Body/Name").GetComponent<Text>().DOColor(Color.black, 0.3f);
            collision.transform.Find("Body/Shadow").GetComponent<Image>().DOFade(0, 0.3f);
            var bg = collision.transform.Find("Body/Bg").GetComponent<Coffee.UIExtensions.UIGradient>();
            DOTween.To(() => bg.color1, x => bg.color1 = x, Color.blue, 0.3f);
            DOTween.To(() => bg.color2, x => bg.color2 = x, Color.green, 0.3f);

            Coffee.UIExtensions.UIGradient uILineGradient = collision.transform.Find("Body/Image").GetComponent<Coffee.UIExtensions.UIGradient>();
            DOTween.To(() => uILineGradient.color1, x => uILineGradient.color1 = x, Color.blue, 0.3f);
            DOTween.To(() => uILineGradient.color2, x => uILineGradient.color2 = x, Color.green, 0.3f);
            Transform index = IndexList.GetChild(collision.transform.GetSiblingIndex() - 1);
            index.GetComponent<RectTransform>().DOSizeDelta(_IndexNormalSize, 0.3f);
            Coffee.UIExtensions.UIGradient uIGradient = index.GetComponent<Coffee.UIExtensions.UIGradient>();
            DOTween.To(() => uIGradient.color1, x => uIGradient.color1 = x, Color.grey, 0.3f);
            DOTween.To(() => uIGradient.color2, x => uIGradient.color2 = x, Color.grey, 0.3f);

        }



        private int _count;
        private bool _hasChanged;
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                StopCoroutine("TimeCount");
            }
            if (Input.GetMouseButtonUp(0) && Mathf.Abs(m_ScrollRect.velocity.x) > 100)
            {
                StartCoroutine("TimeCount");
            }
        }

        private IEnumerator TimeCount()
        {
            if (_target == null)
                yield break;
            while (Mathf.Abs(m_ScrollRect.velocity.x) > 100)
            {
                yield return null;
            }
            m_ScrollRect.StopMovement();
            OnDragEndEvent();
        }

        private void OnDragEndEvent()
        {
            float center = screenWidth / 2f;

            float delta = _target.transform.position.x - center;
            Content.transform.DOLocalMoveX((_target.GetSiblingIndex() - 2) * -360 - screenWidth / 2, 0.3f);
        }

        public string GetCurrentSelectedMissionName()
        {
            if (_target == null)
                return "";
            return _target.Find("Body/Name").GetComponent<Text>().text;
        }
    }
}