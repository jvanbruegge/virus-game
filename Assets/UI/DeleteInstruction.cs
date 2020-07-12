using UnityEngine;

public class DeleteInstruction : MonoBehaviour {
    public int index;
    private Player player;

    private void Awake() {
        this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void RemoveInstruction() {
        player.RemoveInstruction(index);
    }
}
