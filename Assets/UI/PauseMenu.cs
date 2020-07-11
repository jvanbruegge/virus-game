using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private Slider volumeSlider;
    [SerializeField]
    private AudioSource music;

    private bool isActive = false;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            this.ToggleMenu();
        }
    }

    public void ToggleMenu() {
        this.isActive = !isActive;

        if (isActive) {
            Time.timeScale = 0;
        } else {
            Time.timeScale = 1;
        }
        pauseMenu.SetActive(isActive);

    }

    public void UpdateVolume() {
        music.volume = volumeSlider.value;
    }
}
