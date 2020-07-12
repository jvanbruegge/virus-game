using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<GameObject> items = new List<GameObject>();

    public void AddItem(EInstruction item) {
        GameObject obj = Instruction.Create(item, transform);
        items.Add(obj);
        obj.AddComponent<DragInstruction>().instruction = item;
        obj.AddComponent<CanvasGroup>();
    }

}
