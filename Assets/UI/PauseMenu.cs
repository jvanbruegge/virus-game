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
    private GameObject inventory;

    private bool isActive = false;

    private void Awake() {
        this.inventory = GameObject.FindGameObjectWithTag("Inventory");
        ToggleMenu();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            this.ToggleMenu();
        }
    }

    public void ToggleMenu() {
        this.isActive = !isActive || showMenu.activeSelf;

        if (isActive) {
            Time.timeScale = 0;
            if(showMenu.activeSelf) {
                showMenu.SetActive(false);
            }
            foreach(CanvasGroup ins in inventory.GetComponentsInChildren<CanvasGroup>()) {
                ins.interactable = false;
            }
        } else {
            Time.timeScale = 1;
            showMenu.SetActive(false);
            foreach(CanvasGroup ins in inventory.GetComponentsInChildren<CanvasGroup>()) {
                ins.interactable = true;
            }
        }
        pauseMenu.SetActive(isActive);

    }

    public void QuitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    
    public void ToggleHide() {
        this.pauseMenu.SetActive(!this.pauseMenu.activeSelf);
        this.showMenu.SetActive(!this.showMenu.activeSelf);
    }

    public void UpdateVolume() {
        music.volume = volumeSlider.value;
    }
}
