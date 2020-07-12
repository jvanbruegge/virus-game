using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<DragInstruction> items = new List<DragInstruction>();

    public void AddItem(EInstruction item) {
        GameObject obj = Instruction.Create(item, transform);
        obj.AddComponent<CanvasGroup>();
        DragInstruction ins = obj.AddComponent<DragInstruction>();
        ins.instruction = item;
        items.Add(ins);
    }
    
    public void ResetItems() {
        foreach(DragInstruction ins in items) {
            ins.dropped = false;
            ins.group.alpha = 1f;
            ins.group.interactable = true;
        }
    }
}
