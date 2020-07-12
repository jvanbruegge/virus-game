using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public enum Facing {
    Down,
    Right,
    Up,
    Left
}

enum CurrentAnimation {
    None,
    MoveDown,
    MoveUp,
    MoveLeft,
    MoveRight,
    TurnLeft
}

public class Player : MonoBehaviour {
    [SerializeField]
    private Facing facing;
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private Inventory inventory;
    private Animator animator;
    private InstructionList ui;
    private Transform child;

    [SerializeField]
    private List<EInstruction> instructions = new List<EInstruction>();

    public float CurrentTimer { get; private set; }
    public int NextInstruction { get; private set; }
    public int DeathCounter { get; private set; }
    public List<EInstruction> Inventory { get; private set; }

    public const float timer = 140f / 60f;
    private Vector3 initialPosition;

    private CurrentAnimation state = CurrentAnimation.None;

    private Vector3 move = new Vector3();
    private int rotation = 0;
    private bool isAlive = true;

    private void Awake() {
        this.initialPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        this.Inventory = new List<EInstruction>();
        this.animator = GetComponent<Animator>();
        this.ui = GameObject.FindGameObjectWithTag("UI").GetComponentInChildren<InstructionList>();
        this.NextInstruction = 0;
        this.child = transform.GetChild(0);

        this.AddInstruction(EInstruction.FWD, 0);
        this.AddInstruction(EInstruction.FWD, 1);
        this.AddInstruction(EInstruction.FWD, 2);
        this.AddInstruction(EInstruction.FWD, 3);
        this.AddInstruction(EInstruction.Kill, 4);
    }

    private void Start() {
        this.DeathCounter = -1;
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
                    case EInstruction.Right: this.ExecuteTurnInstruction(instruction);  break;
                    case EInstruction.Kill: StartCoroutine(CreateExplosion()); this.ResetPlayer(); break;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.name == "HazardTile") {
            this.isAlive = false;
            StartCoroutine(CreateExplosion());
            transform.position = initialPosition;
            NextInstruction = 0;
            DeathCounter++;
            inventory.ResetItems();
        } else if (collision.name == "TurnLeftPickup") {
            Inventory.Add(EInstruction.Left);
            inventory.AddItem(EInstruction.Left);
            Destroy(collision.gameObject);
        } 
    }

    private IEnumerator CreateExplosion() {
        GameObject exp = Instantiate(explosion);
        exp.transform.position = transform.position + new Vector3(0.5f, -0.5f, -1);
        yield return new WaitForSeconds(3f);
        Destroy(exp);
    }

    private void LateUpdate() {
        if (move.sqrMagnitude != 0) {
            transform.position += move;
            move = new Vector3();
        }
        if (rotation != 0) {
            child.eulerAngles += new Vector3(0, 0, rotation);
            rotation = 0;
        }
    }

    private void ResetPlayer() {
        if(isAlive) {
            this.DeathCounter++;
        }
        this.facing = Facing.Down;
        child.eulerAngles = new Vector3(0, 0, -90);
        this.isAlive = true;
        transform.position = initialPosition;
        NextInstruction = 0;
        inventory.ResetItems();
    }

    private void ExecuteMoveInstruction(EInstruction instruction) {
        bool down = (facing == Facing.Down && instruction == EInstruction.FWD) || (facing == Facing.Up && instruction == EInstruction.BWD);
        bool left = (facing == Facing.Left && instruction == EInstruction.FWD) || (facing == Facing.Right && instruction == EInstruction.BWD);
        bool right = (facing == Facing.Right && instruction == EInstruction.FWD) || (facing == Facing.Left && instruction == EInstruction.BWD);

        Vector2 dir = new Vector2(0.5f, -0.5f);

        if (down) {
            this.state = CurrentAnimation.MoveDown;
            dir += new Vector2(0, -1);
        } else if (left) {
            this.state = CurrentAnimation.MoveLeft;
            dir += new Vector2(-1, 0);
        } else if (right) {
            this.state = CurrentAnimation.MoveRight;
            dir += new Vector2(1, 0);
        } else {
            this.state = CurrentAnimation.MoveUp;
            dir += new Vector2(0, 1);
        }

        Collider2D collider = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y) + dir, new Vector2(0.8f, 0.8f), 0, LayerMask.GetMask("Walls"));
        if(collider == null) {
            this.animator.SetTrigger("Move");
        } else {
            this.animator.SetTrigger("TryMove");
            this.state = CurrentAnimation.None;
        }
        
    }

    private void ExecuteTurnInstruction(EInstruction instruction) {
        if(instruction == EInstruction.Left) {
            this.facing = (Facing) (((int)facing + 1) % 4);
            this.animator.SetTrigger("TurnLeft");
            this.state = CurrentAnimation.TurnLeft;
        } else {
            this.facing = (Facing) ((int)facing - 1);
            if(this.facing < 0) {
                this.facing = Facing.Left;
            }
        }
    }

    public void UpdatePosition() {
        if (this.state == CurrentAnimation.MoveDown) {
            move = new Vector3(0, -1, 0);
        } else if (this.state == CurrentAnimation.MoveUp) {
            move = new Vector3(0, 1, 0);
        } else if (this.state == CurrentAnimation.MoveLeft) {
            move = new Vector3(-1, 0, 0);
        } else if (this.state == CurrentAnimation.MoveRight) {
            move = new Vector3(1, 0, 0);
        } else if (this.state == CurrentAnimation.TurnLeft) {
            rotation = 90;            
        }
        this.state = CurrentAnimation.None;
    }

    public void AddInstruction(EInstruction instruction, int position) {
        this.AddInstruction(instruction, position, false);
    }
    public void AddInstruction(EInstruction instruction, int position, bool user) {
        this.instructions.Insert(position, instruction);
        this.ui.AddInstruction(instruction, position, user);
        if(position < NextInstruction) {
            NextInstruction++;
        }
    }

    public void RemoveInstruction(int index) {
        if(NextInstruction > index) {
            NextInstruction--;
        }
        this.instructions.RemoveAt(index);
        this.ui.RemoveInstruction(index);
    }
}
