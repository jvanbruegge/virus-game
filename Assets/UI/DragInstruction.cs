using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragInstruction : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler  {
    private RectTransform rect;
    private Canvas canvas;
    public CanvasGroup group;
    private RectTransform parent;
    private DropInstruction drop;
    public EInstruction instruction;
   
    private Vector3 initialPosition;
    private GameObject clone;
    public bool dropped = false;


    private void Awake() {
        rect = GetComponent<RectTransform>();
        group = GetComponent<CanvasGroup>();
        parent = transform.parent as RectTransform;

        drop = GameObject.FindGameObjectWithTag("InstructionList").GetComponent<DropInstruction>();
        canvas = GameObject.FindGameObjectWithTag("UI").GetComponent<Canvas>();

        initialPosition = rect.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (group.interactable == false) {
            eventData.pointerDrag = null;
        } else {
            group.blocksRaycasts = false;
            group.alpha = 0.6f;
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        rect.anchoredPosition = initialPosition;
        LayoutRebuilder.ForceRebuildLayoutImmediate(parent);
        group.blocksRaycasts = true;

        if(dropped) {
            group.alpha = 0.3f;
            group.interactable = false;
        } else {
            group.alpha = 1f;
        }
        dropped = false;
    }

    public void OnPointerDown(PointerEventData eventData) {
    }

    public void OnDrag(PointerEventData eventData) {
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
        drop.OnPointerMove(eventData);
    }
}
