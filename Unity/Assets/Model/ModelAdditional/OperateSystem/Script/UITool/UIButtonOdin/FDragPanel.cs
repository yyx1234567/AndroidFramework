
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
namespace ETModel
{
    public class DragPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
    {

        private Vector2 originalLocalPointerPosition;
        private Vector3 originalPanelLocalPosition;
        private RectTransform panelRectTransform;
        private RectTransform parentRectTransform;
        void Awake()
        {
            panelRectTransform = transform.parent as RectTransform;
            parentRectTransform = panelRectTransform.parent as RectTransform;
        }

        public void OnPointerDown(PointerEventData data)
        {
            //        GestureManager.IsOverUI = true;
            originalPanelLocalPosition = panelRectTransform.localPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, data.position, data.pressEventCamera, out originalLocalPointerPosition);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            //   IsDrag = false;
            //      GestureManager.IsOverUI = false;
        }
        public void OnDrag(PointerEventData data)
        {
            if (panelRectTransform == null || parentRectTransform == null)
                return;
            Vector2 localPointerPosition;
 
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, data.position, data.pressEventCamera, out localPointerPosition))
            {
                Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;
                panelRectTransform.localPosition = originalPanelLocalPosition + offsetToOriginal;
             }

 
             ClampToWindow ();
        }

        // Clamp panel to area of parent
        void ClampToWindow()
        {

            Vector3 pos = panelRectTransform.localPosition;

            Vector3 minPosition = parentRectTransform.rect.min - panelRectTransform.rect.min;
            Vector3 maxPosition = parentRectTransform.rect.max - panelRectTransform.rect.max;

            pos.x = Mathf.Clamp(panelRectTransform.localPosition.x, minPosition.x, maxPosition.x);
            pos.y = Mathf.Clamp(panelRectTransform.localPosition.y, minPosition.y, maxPosition.y);

            panelRectTransform.localPosition = pos;
        }

 

        public void OnEndDrag(PointerEventData eventData)
        {
            Selectable btn = GetComponentInChildren<Selectable>();
            if (btn != null)
            {
                btn.GetComponent<Image>().raycastTarget = true;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            CanvasGroup cg = GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 1;
            }
            Selectable btn = GetComponentInChildren<Selectable>();
            if (btn != null)
            {
                btn.GetComponent<Image>().raycastTarget = false;
            }
        }
    }
}
