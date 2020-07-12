using Cinemachine;
using TMPro;
using UnityEngine;

public class VictoryMenu : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private GameObject screen;
    [SerializeField]
    private CinemachineVirtualCamera vcam;
    [SerializeField]
    private GameObject clock;
    private Player player;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void Activate(LevelInfo level) {
        text.text = this.MakeStats(level);
        screen.SetActive(true);
        Time.timeScale = 0;
    }

    public void RepeatLevel() {
        player.Spawn(player.level);
        screen.SetActive(false);
        Time.timeScale = 1;
    }

    public void NextLevel() {
        screen.SetActive(false);
        clock.SetActive(false);
        Time.timeScale = 1;
        vcam.Follow = player.transform.GetChild(0);
        player.isTransitioning = true;
    }

    private string MakeStats(LevelInfo level) {
        string res = "This level is solvable with:\n";
        res += level.numDeaths + " Deaths, " + level.numSteps + " Steps\n";
        res += "\n";
        res += player.DeathCounter + " Deaths, " + player.StepCounter + " Steps";
        return res;
    }
}
