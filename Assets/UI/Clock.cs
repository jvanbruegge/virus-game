using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    [SerializeField]
    private Image fill;
    [SerializeField]
    private Player player;

    void Update()
    {
        this.fill.fillAmount = player.CurrentTimer / (float)Player.timer;
    }
}
