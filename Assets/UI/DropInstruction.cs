using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropInstruction : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField]
    private InstructionList list;
    [SerializeField]
    private RectTransform content;
    private Camera mainCamera;

    private GameObject ghost;
    private int index;
    private bool isDragging = false;

    private void Awake() {
        this.mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    public void OnDrop(PointerEventData eventData) {
        Debug.Log("Drop");
        // TODO: Add instruction for player
        Destroy(ghost);
        isDragging = false;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (eventData.dragging) {

            ghost = Instantiate(eventData.pointerDrag, content);
            index = this.GetIndex(eventData);
            ghost.transform.SetSiblingIndex(index);
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);
            isDragging = true;
        }
    }

    public void OnPointerMove(PointerEventData eventData) {
        if (isDragging) {
            int newIndex = this.GetIndex(eventData);
            if(newIndex != index) {
                ghost.transform.SetSiblingIndex(newIndex);
                index = newIndex;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (eventData.dragging) {
            if (ghost != null) {
                Destroy(ghost);
            }
            isDragging = false;
        }
    }
    private int GetIndex(PointerEventData eventData) {
        Vector2 relative;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, eventData.pointerCurrentRaycast.screenPosition, mainCamera, out relative);
        return -((int)relative.y - InstructionList.height / 2 + 25) / InstructionList.height;
    }
}
