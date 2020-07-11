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
    private InstructionList ui;
    [SerializeField]
    private Facing facing;

    [SerializeField]
    private int deaths = 0;
    [SerializeField]
    private List<EInstruction> instructions = new List<EInstruction>();

    public float CurrentTimer { get; private set; }

    private readonly List<EInstruction> nextInstructions = new List<EInstruction>();
    public const float timer = 140f/60f;
    private Vector3 initialPosition;

    private void Start() {
        this.initialPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        this.nextInstructions.Add(EInstruction.FWD);
        this.nextInstructions.Add(EInstruction.FWD);
        this.nextInstructions.Add(EInstruction.FWD);
        this.nextInstructions.Add(EInstruction.FWD);
        this.nextInstructions.Add(EInstruction.Kill);

        this.deaths = -1;
        this.ResetPlayer();
    }

    private void Update() {
        this.CurrentTimer += Time.deltaTime;
        if (CurrentTimer > timer) {
            EInstruction instruction = this.PopInstruction();
            this.nextInstructions.Add(instruction);
            this.CurrentTimer = 0;
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

        foreach(EInstruction instruction in this.nextInstructions) {
            this.AddInstruction(instruction);
        }

        this.nextInstructions.Clear();
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
