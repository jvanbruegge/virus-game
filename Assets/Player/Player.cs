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
    private Facing facing;
    private Animator animator;
    private InstructionList ui;

    [SerializeField]
    private int deaths = 0;
    [SerializeField]
    private List<EInstruction> instructions = new List<EInstruction>();

    public float CurrentTimer { get; private set; }
    public int NextInstruction { get; private set; }

    public const float timer = 140f / 60f;
    private Vector3 initialPosition;

    private CurrentAnimation state = CurrentAnimation.None;

    private Vector3 move = new Vector3();
    private bool isAlive = true;

    private void Awake() {
        this.initialPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        this.animator = GetComponent<Animator>();
        this.ui = GameObject.FindGameObjectWithTag("UI").GetComponentInChildren<InstructionList>();
        this.NextInstruction = 0;

        this.AddInstruction(EInstruction.FWD, 0);
        this.AddInstruction(EInstruction.FWD, 1);
        this.AddInstruction(EInstruction.FWD, 2);
        this.AddInstruction(EInstruction.FWD, 3);
        this.AddInstruction(EInstruction.Kill, 4);
    }

    private void Start() {
        this.deaths = -1;
        this.ResetPlayer();
    }

    private void Update() {
        this.CurrentTimer += Time.deltaTime;
        if (CurrentTimer > timer) {

            if (!isAlive) {
                this.ResetPlayer();
            } else {
                EInstruction instruction = this.instructions[NextInstruction++];
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
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.name == "HazardTile") {
            this.isAlive = false;
        }
    }

    private void LateUpdate() {
        if (move.sqrMagnitude != 0) {
            transform.position += move;
            move = new Vector3();
        }

    }

    private void ResetPlayer() {
        this.deaths++;
        this.isAlive = true;
        transform.position = initialPosition;
        NextInstruction = 0;
    }

    private void ExecuteMoveInstruction(EInstruction instruction) {
        bool down = (facing == Facing.Down && instruction == EInstruction.FWD) || (facing == Facing.Up && instruction == EInstruction.BWD);

        if (down) {
            this.animator.SetTrigger("MoveDown");
            this.state = CurrentAnimation.MoveDown;
        } else {
            this.animator.SetTrigger("MoveUp");
        }
    }

    public void UpdatePosition() {
        if (!isAlive) {

        }
        if (this.state == CurrentAnimation.MoveDown) {
            move = new Vector3(0, -1, 0);
        }
        this.state = CurrentAnimation.None;
    }

    private void AddInstruction(EInstruction instruction, int position) {
        this.instructions.Insert(position, instruction);
        this.ui.AddInstruction(instruction, position);
    }
}
