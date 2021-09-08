using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
 using DG.Tweening;
using UnityEngine.UI;
namespace ETModel
{
    public class UIDragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public GameObject go;

        public ETModel.MaterialResourcesConfig ItemData;

        public Action<UIDragItem, ETModel.MaterialResourcesConfig> DragEndEvent;
        public Action<ModelDragItem, ETModel.MaterialResourcesConfig> AddDragItemEvent;

        private GameObject _lastgameobject;

        [System.NonSerialized]
        public Transform PlaceArea;
        public string PlaceName;
        public void OnBeginDrag(PointerEventData eventData)
        {
            go.GetComponent<CanvasGroup>().alpha = 1;
            go.transform.DOScale(Vector3.one, 0);
            if (GetComponent<CanvasGroup>())
                GetComponent<CanvasGroup>().alpha = 0.5F;
            go.transform.DOScale(Vector3.one * 1F, 0.3F).SetEase(Ease.OutBack);

            go.transform.Find("Target").GetComponent<Image>().sprite = ItemData.SpriteTarget;
            go.GetComponentInChildren<DragPanel>().OnBeginDrag(eventData);

        }

        public void OnDrag(PointerEventData eventData)
        {
            go.GetComponentInChildren<DragPanel>().OnDrag(eventData);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var raycastlist = Physics.RaycastAll(Input.mousePosition, Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (!hit.collider.gameObject.name.StartsWith("PlaceHold"))
                {
                    return;
                }
                var dragitem = hit.collider.GetComponent<ModelDragItem>();
                if (dragitem == null)
                {
                    dragitem = hit.collider.gameObject.AddComponent<ModelDragItem>();
                    dragitem.AddDragItemEvent += AddDragItemEvent;
                }
                if (dragitem.HasMaterialResources)
                    return;
                var highlight = hit.collider.GetComponent<Highlighting>() ?? hit.collider.gameObject.AddComponent<Highlighting>();
                highlight.CancelFlash();
                if (_lastgameobject != null && _lastgameobject != highlight.gameObject)
                {
                    _lastgameobject.GetComponent<Highlighting>().CancelFlash();
                }
                _lastgameobject = highlight.gameObject;
            }
            else
            {
                if (_lastgameobject != null)
                    _lastgameobject.GetComponent<Highlighting>().CancelFlash();
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_lastgameobject != null)
                _lastgameobject.GetComponent<Highlighting>().CancelFlash();

            go.GetComponentInChildren<DragPanel>().OnEndDrag(eventData);
            go.transform.DOScale(Vector3.zero, 0.2F).SetEase(Ease.OutBack);
            go.GetComponent<CanvasGroup>().DOFade(0, 0.2F);
            if (GetComponent<CanvasGroup>())
                GetComponent<CanvasGroup>().DOFade(1, 0.2F);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var raycastlist = Physics.RaycastAll(Input.mousePosition, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (!hit.collider.gameObject.name.StartsWith("PlaceHold"))
                {
                    return;
                }
                var dragitem = hit.collider.GetComponent<ModelDragItem>() ?? hit.collider.gameObject.AddComponent<ModelDragItem>();

                if (dragitem.HasMaterialResources)
                    return;
                dragitem.materialResources = ItemData;
                dragitem.uidragitem = this;

                var highlight = hit.collider.GetComponent<Highlighting>() ?? hit.collider.gameObject.AddComponent<Highlighting>();
                highlight.CancelFlash();

                dragitem.HasMaterialResources = true;
                gameObject.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutBack).onComplete += () =>
                  {
                  };
                PlaceArea = hit.collider.transform;
                PlaceName = hit.collider.transform.parent.parent.Find("Tip").GetComponentInChildren<Text>().text;
                DragEndEvent?.Invoke(this, ItemData);
            }

        }
    }
}