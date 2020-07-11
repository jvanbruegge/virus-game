using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public enum Facing {
    Down,
    Up,
    Left,
    Right
}

public class Player : MonoBehaviour {
    [SerializeField]
    private Facing facing;
    [SerializeField]
    private InstructionList ui;
    private const int timer = 2;
    [SerializeField]
    private float current_timer = 0f;
    [SerializeField]
    private int deaths = -1;
    [SerializeField]
    private List<EInstruction> instructions = new List<EInstruction>();

    private Vector3 initialPosition;

    private void Start() {
        this.initialPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        this.ResetPlayer();
    }

    private void Update() {
        this.current_timer += Time.deltaTime;
        if (current_timer > timer) {
            EInstruction instruction = this.PopInstruction();
            this.current_timer = 0;
            switch (instruction) {
                case EInstruction.FWD:
                case EInstruction.BWD: this.ExecuteMoveInstruction(instruction); break;
                case EInstruction.Left:
                    break;
                case EInstruction.Right:
                    break;
                case EInstruction.Kill: this.ResetPlayer(); break;
            }
        }
    }

    private void ResetPlayer() {
        this.deaths++;
        transform.position += initialPosition - transform.position;

        this.AddInstruction(EInstruction.FWD);
        this.AddInstruction(EInstruction.FWD);
        this.AddInstruction(EInstruction.FWD);
        this.AddInstruction(EInstruction.FWD);
        this.AddInstruction(EInstruction.Kill);
    }

    private void ExecuteMoveInstruction(EInstruction instruction) {
        Vector3 move = new Vector3(
            this.facing == Facing.Right ? 1 : (this.facing == Facing.Left ? -1 : 0),
            this.facing == Facing.Up ? 1 : (this.facing == Facing.Down ? -1 : 0),
            0
        );
        if (instruction == EInstruction.BWD) {
            move *= -1;
        }
        transform.position += move;
    }

    private EInstruction PopInstruction() {
        EInstruction ins = this.instructions[0];
        this.instructions.RemoveAt(0);
        this.ui.PopInstruction();
        return ins;
    }

    private void AddInstruction(EInstruction instruction) {
        this.instructions.Add(instruction);
        this.ui.AddInstruction(instruction);
    }
}
