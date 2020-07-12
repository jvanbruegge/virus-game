using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
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
    [SerializeField]
    private LevelInfo[] levels;
    [SerializeField]
    private VictoryMenu victory;
    [SerializeField]
    private GameObject clock;
    private Animator animator;
    private InstructionList ui;
    private Transform child;

    [SerializeField]
    private List<EInstruction> instructions = new List<EInstruction>();

    public float CurrentTimer { get; private set; }
    public int NextInstruction { get; private set; }
    public int DeathCounter { get; private set; }
    public int StepCounter { get; private set; }
    public List<EInstruction> Inventory { get; private set; }

    public const float timer = 140f / 60f;
    private Vector3 initialPosition;
    private Facing initialFacing;

    private CurrentAnimation state = CurrentAnimation.None;

    private Vector3 move = new Vector3();
    private int rotation = 0;
    private bool isAlive = false;

    public int level = 0;

    private bool isTransitioning = false;
    private float transitionTimer = 0;
    private int currentPathIndex = 0;
    private Vector3 nextTransitionPoint;
    private Vector3 lastTransitionPoint;

    private void Awake() {
        this.Inventory = new List<EInstruction>();
        this.animator = GetComponent<Animator>();
        this.ui = GameObject.FindGameObjectWithTag("UI").GetComponentInChildren<InstructionList>();
        this.child = transform.GetChild(0);
        this.Spawn(0);
    }

    public void Spawn(int level) {
        this.level = level % levels.Length;
        LevelInfo l = levels[this.level];
        this.initialPosition = l.spawn;
        this.initialFacing = l.spawnFacing;
        this.NextInstruction = 0;
        this.DeathCounter = 0;
        this.StepCounter = 0;
        this.isAlive = false;
        this.CurrentTimer = 0;
        this.nextTransitionPoint = l.nextLevelPath[0].transform.position;
        this.clock.SetActive(true);

        int counter = 0;
        this.instructions.Clear();
        this.ui.Clear();
        this.inventory.Clear();
        foreach (EInstruction instruction in l.instructions) {
            this.AddInstruction(instruction, counter++);
        }

        foreach(GameObject pickup in GameObject.FindGameObjectsWithTag("Pickup")) {
            pickup.transform.GetChild(0).gameObject.SetActive(true);
            pickup.GetComponent<BoxCollider2D>().enabled = true;
        }

        this.isTransitioning = false;
        this.currentPathIndex = 0;
        this.ResetPlayer();
    }

    private void Update() {
        if (!isTransitioning) {
            this.CurrentTimer += Time.deltaTime;
            if (CurrentTimer > timer) {

                if (!isAlive) {
                    this.ResetPlayer();
                } else {
                    EInstruction instruction = this.instructions[NextInstruction++];
                    this.CurrentTimer = 0;
                    this.StepCounter++;

                    switch (instruction) {
                        case EInstruction.FWD:
                        case EInstruction.BWD: this.ExecuteMoveInstruction(instruction); break;
                        case EInstruction.Left:
                        case EInstruction.Right: this.ExecuteTurnInstruction(instruction); break;
                        case EInstruction.Kill: StartCoroutine(CreateExplosion()); this.ResetPlayer(); break;
                    }
                }
            }
        } else {
            transitionTimer += Time.deltaTime * 10/(nextTransitionPoint - lastTransitionPoint).magnitude;
            if(transform.position != nextTransitionPoint) {
                transform.position = Vector3.Lerp(lastTransitionPoint, nextTransitionPoint, transitionTimer);
            } else {
                transitionTimer = 0;
                if(currentPathIndex < levels[level].nextLevelPath.Length - 1) {
                    currentPathIndex++;
                    lastTransitionPoint = nextTransitionPoint;
                    nextTransitionPoint = levels[level].nextLevelPath[currentPathIndex].transform.position;
                } else {
                    this.Spawn(this.level + 1);
                }
            }
        }
    }

    public void Transition() {
        isTransitioning = true;
        this.lastTransitionPoint = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.name == "HazardTile") {
            this.isAlive = false;
            StartCoroutine(CreateExplosion());
            transform.position = initialPosition;
            NextInstruction = 0;
            DeathCounter++;
            inventory.ResetItems();
        } else if (collision.name.EndsWith("Pickup")) {
            EInstruction ins = EInstruction.Kill;
            switch (collision.name) {
                case "TurnLeftPickup": ins = EInstruction.Left; break;
                case "TurnRightPickup": ins = EInstruction.Right; break;
                case "DashPickup": ins = EInstruction.Dash; break;
                case "BackwardsPickup": ins = EInstruction.BWD; break;
            }
            Inventory.Add(ins);
            inventory.AddItem(ins);
            collision.transform.GetChild(0).gameObject.SetActive(false);
            collision.GetComponent<BoxCollider2D>().enabled = false;
        } else if (collision.name == "GoalTile") {
            this.victory.Activate(levels[level]);
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
        if (isAlive) {
            this.DeathCounter++;
        }

        int childReset = 0;
        switch (initialFacing) {
            case Facing.Down: childReset = -90; break;
            case Facing.Right: childReset = 0; break;
            case Facing.Up: childReset = 90; break;
            case Facing.Left: childReset = 180; break;
        }
        child.eulerAngles = new Vector3(0, 0, childReset);
        this.isAlive = true;
        this.facing = initialFacing;
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
        if (collider == null) {
            this.animator.SetTrigger("Move");
        } else {
            this.animator.SetTrigger("TryMove");
            this.state = CurrentAnimation.None;
        }

    }

    private void ExecuteTurnInstruction(EInstruction instruction) {
        if (instruction == EInstruction.Left) {
            this.facing = (Facing)(((int)facing + 1) % 4);
            this.animator.SetTrigger("TurnLeft");
            this.state = CurrentAnimation.TurnLeft;
        } else {
            this.facing = (Facing)((int)facing - 1);
            if (this.facing < 0) {
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
        if (position < NextInstruction) {
            NextInstruction++;
        }
    }

    public void RemoveInstruction(int index) {
        if (NextInstruction > index) {
            NextInstruction--;
        }
        this.instructions.RemoveAt(index);
        this.ui.RemoveInstruction(index);
    }
}
