using System.Collections.Generic;
using UnityEngine;

public class InstructionList : MonoBehaviour {
    [SerializeField]
    private RectTransform content;
    [SerializeField]
    private RectTransform arrow;
    [SerializeField]
    private GameObject buttonX;

    private List<GameObject> instructions = new List<GameObject>();
    private Player player;

    public int ghostIndex = -1;

    public const int height = 98;
    private Vector2 arrowPos;

    private void Awake() {
        arrowPos = this.arrow.anchoredPosition;
    }

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update() {
        int ins = player.NextInstruction;
        this.arrow.anchoredPosition = new Vector3(arrowPos.x, arrowPos.y - (player.NextInstruction + (ins <= ghostIndex || ghostIndex == -1 ? 0 : 1)) * height, 0);
    }

    public void AddInstruction(EInstruction instruction, int position, bool user) {
        GameObject obj = Instruction.Create(instruction, content);
        this.instructions.Insert(position, obj);
        obj.transform.SetSiblingIndex(position);
        (this.content.parent as RectTransform).sizeDelta = new Vector2(0, this.instructions.Count * height);
        if (user) {
            GameObject x = Instantiate(buttonX, obj.transform);
            x.GetComponent<DeleteInstruction>().index = position;
        }
    }

    public void RemoveInstruction(int index) {
        GameObject obj = this.instructions[index];
        this.instructions.RemoveAt(index);
        Destroy(obj);
    }

    public void Clear() {
        foreach(GameObject obj in instructions) {
            Destroy(obj);
        }
        instructions.Clear();
    }
}
