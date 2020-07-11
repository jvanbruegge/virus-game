using System.Collections.Generic;
using UnityEngine;

public class InstructionList : MonoBehaviour
{
    [SerializeField]
    private RectTransform content;
    [SerializeField]
    private RectTransform arrow;

    private List<GameObject> instructions = new List<GameObject>();
    private Player player;

    private const int height = 105;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update() {
        this.arrow.anchoredPosition = new Vector3(40, -35 - player.NextInstruction * height, 0);
    }

    public void AddInstruction(EInstruction instruction, int position) {
        this.instructions.Insert(position, Instruction.Create(instruction, content));
        (this.content.parent as RectTransform).sizeDelta = new Vector2(0, this.instructions.Count * height);
    }
}
