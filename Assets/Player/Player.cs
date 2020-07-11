using System.Collections.Generic;
using UnityEngine;

public enum Facing {
    Down,
    Up,
    Left,
    Right
}

enum CurrentAnimation {
    None,
    MoveDown
}

public class Player : MonoBehaviour {
    [SerializeField]
    private InstructionList ui;
    [SerializeField]
    private Facing facing;
    private Animator animator;

    [SerializeField]
    private int deaths = 0;
    [SerializeField]
    private List<EInstruction> instructions = new List<EInstruction>();

    public float CurrentTimer { get; private set; }

    private readonly List<EInstruction> nextInstructions = new List<EInstruction>();
    public const float timer = 140f/60f;
    private Vector3 initialPosition;

    private CurrentAnimation state = CurrentAnimation.None;

    private Vector3 move = new Vector3();

    private void Awake() {
        this.initialPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        this.animator = GetComponent<Animator>();
    }

    private void Start() {
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

    private void LateUpdate() {
        if(move.sqrMagnitude != 0) {
            transform.position += move;
            move = new Vector3();
        }

    }

    private void ResetPlayer() {
        this.deaths++;
        transform.position = initialPosition;

        foreach(EInstruction instruction in this.nextInstructions) {
            this.AddInstruction(instruction);
        }

        this.nextInstructions.Clear();
    }

    private void ExecuteMoveInstruction(EInstruction instruction) {
        bool down = (facing == Facing.Down && instruction == EInstruction.FWD) || (facing == Facing.Up && instruction == EInstruction.BWD);

        if(down) {
            this.animator.SetTrigger("MoveDown");
            this.state = CurrentAnimation.MoveDown;
        } else {
            this.animator.SetTrigger("MoveUp");
        }
    }

    public void UpdatePosition() {
        if(this.state == CurrentAnimation.MoveDown) {
            move = new Vector3(0, -1, 0);
        }
        this.state = CurrentAnimation.None;
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
