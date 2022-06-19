using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragPanel : MonoBehaviour,IDragHandler,IPointerDownHandler
{
    RectTransform rectTransform;
    Canvas canvas;

    void Awake() {
        rectTransform = GetComponent<RectTransform>();
        canvas= InventoryManagement.Instance.GetComponent<Canvas>();
    }
    public void OnDrag(PointerEventData eventData)
    {
       rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rectTransform.SetSiblingIndex(2);
    }
}

  
