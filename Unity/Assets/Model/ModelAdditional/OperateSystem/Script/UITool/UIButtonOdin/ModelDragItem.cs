using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
 using DG.Tweening;
using System.Linq;
using UnityEngine.UI;
namespace ETModel { 
public class ModelDragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GameObject go;

    public Action<ModelDragItem, ETModel.MaterialResourcesConfig> AddDragItemEvent;

    public ETModel.MaterialResourcesConfig materialResources;

    public bool HasMaterialResources;
    internal UIDragItem uidragitem;
    private GameObject _lastgameobject;
    private void Awake()
    {
     }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!HasMaterialResources)
            return;
        go.GetComponent<CanvasGroup>().alpha = 1;
        go.transform.DOScale(Vector3.one, 0);
        if (GetComponent<CanvasGroup>())
            GetComponent<CanvasGroup>().alpha = 0.5F;
        go.transform.DOScale(Vector3.one * 1F, 0.3F).SetEase(Ease.OutBack);
        go.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        go.transform.Find("Target").GetComponent<Image>().sprite = uidragitem.ItemData.SpriteTarget;

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!HasMaterialResources)
            return;


        go.transform.localPosition = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0);
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

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
                 dragitem.AddDragItemEvent += uidragitem.AddDragItemEvent;
            }
            if (dragitem.HasMaterialResources)
                return;
            var highlight = hit.collider.GetComponent<Highlighting>() ?? hit.collider.gameObject.AddComponent<Highlighting>();
            highlight.StartFlash();
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
        //go.GetComponentInChildren<FDragPanel>().OnDrag(eventData);
    }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_lastgameobject != null)
                _lastgameobject.GetComponent<Highlighting>().CancelFlash();
            if (!HasMaterialResources)
                return;
            //go.GetComponentInChildren<FDragPanel>().OnEndDrag(eventData);
            go.transform.DOScale(Vector3.zero, 0.2F).SetEase(Ease.OutBack);
            go.GetComponent<CanvasGroup>().DOFade(0, 0.2F);
            if (GetComponent<CanvasGroup>())
                GetComponent<CanvasGroup>().DOFade(1, 0.2F);
            List<RaycastResult> results = new List<RaycastResult>();
            UnityEngine.EventSystems.EventSystem.current.RaycastAll(eventData, results);
            if (results.Count == 0)
                return;
            var area = results.Where(x => x.gameObject.GetComponent<UIPlaceArea>());
            var model = results.Where(x => x.gameObject.GetComponent<Collider>());

            if (area.Count() != 0)
            {
                uidragitem.PlaceName = string.Empty;
                uidragitem.transform.position = go.transform.position;
                uidragitem.transform.localScale = Vector3.one;
                uidragitem.transform.SetAsLastSibling();
                uidragitem.transform.DOScale(Vector3.one * 1f, 0.2f).SetEase(Ease.OutBack).onComplete += () =>
                {
                     HasMaterialResources = false;
                };
                return;
            }

            if (model.Count() != 0)
            {
                if (!model.FirstOrDefault().gameObject.name.StartsWith("PlaceHold"))
                {
                    return;
                }
                var modeldrag = model.FirstOrDefault().gameObject.GetComponent<ModelDragItem>() ??
                    model.FirstOrDefault().gameObject.AddComponent<ModelDragItem>();
                if (modeldrag.HasMaterialResources)
                    return;
                AddDragItemEvent?.Invoke(modeldrag, materialResources);
                HasMaterialResources = false;
                modeldrag.uidragitem = uidragitem;
                modeldrag.HasMaterialResources = true;
                modeldrag.materialResources = materialResources;
                return;
            }
         }
    }
}
