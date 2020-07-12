using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private Slider volumeSlider;
    [SerializeField]
    private AudioSource music;
    [SerializeField]
    private GameObject showMenu;
    private CanvasGroup group;

    private bool isActive = false;

    private void Awake() {
        this.group = GetComponent<CanvasGroup>();
        ToggleMenu();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            this.ToggleMenu();
        }
    }

    public void ToggleMenu() {
        this.isActive = !isActive || showMenu.activeSelf;
        group.blocksRaycasts = isActive;

        if (isActive) {
            Time.timeScale = 0;
            if(showMenu.activeSelf) {
                showMenu.SetActive(false);
            }
        } else {
            Time.timeScale = 1;
            showMenu.SetActive(false);
        }
        pauseMenu.SetActive(isActive);

    }
    
    public void ToggleHide() {
        this.pauseMenu.SetActive(!this.pauseMenu.activeSelf);
        this.showMenu.SetActive(!this.showMenu.activeSelf);
    }

    public void UpdateVolume() {
        music.volume = volumeSlider.value;
    }
}
