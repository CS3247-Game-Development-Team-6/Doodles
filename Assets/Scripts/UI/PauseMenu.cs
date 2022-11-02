using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    public static bool GameIsPaused;

    public string menuSceneName;
    public GameObject pauseMenuUI;
    public GameObject gameplayCanvas;
    public GameObject raycastOccluder;
    public LoadingUI loadingScreen;

    private void Start() {
        if (menuSceneName.Length == 0) {
            Debug.LogError($"Empty menuSceneName\nSet in {name}");
        } else if (SceneManager.GetSceneByName(menuSceneName).IsValid()) {
            Debug.LogError($"menuSceneName not found: {menuSceneName}\nSet in {name}");
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (GameIsPaused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void Resume() {
        GameObject[] ddols = GameStateManager.GetDontDestroyOnLoadObjects();
        for (int i = 0; i < ddols.Length; i++) {
            ddols[i].SetActive(true);
        }

        pauseMenuUI.SetActive(false);
        gameplayCanvas.SetActive(true);
        raycastOccluder.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Resume();
    }

    void Pause() {
        if (!GameStateManager.getIsGameEnded()) {
            GameObject[] ddols = GameStateManager.GetDontDestroyOnLoadObjects();
            for (int i = 0; i < ddols.Length; i++) {
                ddols[i].SetActive(false);
            }

            GameIsPaused = true;
            pauseMenuUI.SetActive(true);
            gameplayCanvas.SetActive(false);
            raycastOccluder.SetActive(true);
            Time.timeScale = 0f;
        }

        TooltipSystem.Hide();
    }

    public void Freeze() {
        GameObject[] ddols = GameStateManager.GetDontDestroyOnLoadObjects();
        for (int i = 0; i < ddols.Length; i++) {
            ddols[i].SetActive(false);
        }
        pauseMenuUI.SetActive(true);
        raycastOccluder.SetActive(true);
        Time.timeScale = 0f;
    }

    public void LoadMenu() {
        Resume();
        if (loadingScreen != null) {
            loadingScreen.gameObject.SetActive(true);
            loadingScreen.AddSceneToLoad(menuSceneName);
            loadingScreen.StartLoad();
        } else {
            Debug.LogWarning("No loading screen found. Add one later!");
            SceneManager.LoadScene(menuSceneName);
        }
    }

    public void Retry() {
        Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool IsPaused() {
        return GameIsPaused;
    }

}
