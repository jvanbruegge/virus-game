﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragInstruction : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler  {
    private RectTransform rect;
    private Canvas canvas;
    private CanvasGroup group;
    private RectTransform parent;
    [SerializeField]
    private DropInstruction drop;
   
    private Vector3 initialPosition;
    private GameObject clone;

    private void Awake() {
        rect = GetComponent<RectTransform>();
        group = GetComponent<CanvasGroup>();
        parent = transform.parent as RectTransform;

        canvas = GameObject.FindGameObjectWithTag("UI").GetComponent<Canvas>();

        initialPosition = rect.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        group.blocksRaycasts = false;
        group.alpha = 0.6f;
    }

    public void OnEndDrag(PointerEventData eventData) {
        rect.anchoredPosition = initialPosition;
        group.blocksRaycasts = true;
        group.alpha = 1f;
        LayoutRebuilder.ForceRebuildLayoutImmediate(parent);
            
    }

    public void OnPointerDown(PointerEventData eventData) {
    }

    public void OnDrag(PointerEventData eventData) {
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
        drop.OnPointerMove(eventData);
    }
}
