using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

public class VictoryMenu : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private GameObject screen;
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

    }

    public void NextLevel() {

    }

    private string MakeStats(LevelInfo level) {
        string res = "This level is solvable with:\n";
        res += level.numDeaths + " Deaths, " + level.numSteps + " Steps\n";
        res += "\n";
        res += player.DeathCounter + " Deaths, " + player.StepCounter + " Steps";
        return res;
    }
}
