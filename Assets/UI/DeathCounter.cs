using TMPro;
using UnityEngine;

public class DeathCounter : MonoBehaviour
{
    private Player player;
    private TextMeshProUGUI text;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update() {
        text.text = "Deaths: " + player.DeathCounter;
    }
}
