using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    // Remember to add the desired scene to the open scenes list under File -> Build Settings
    public static string MenuSceneName { get; private set; }
    public LoadingUI loadingScreen;
    [SerializeField] private string[] levelSceneNames;
    [SerializeField] private string[] dialogueSceneNames;

    public void Awake() {
        if (!PlayerPrefs.HasKey(SettingsMenu.ResolutionHeightPref)) {
            PlayerPrefs.SetInt(SettingsMenu.ResolutionWidthPref, 1920);
            PlayerPrefs.SetInt(SettingsMenu.ResolutionHeightPref, 1080);
            PlayerPrefs.SetInt(SettingsMenu.FullScreenPref, 1);
        }

        Screen.SetResolution(PlayerPrefs.GetInt(SettingsMenu.ResolutionWidthPref),
            PlayerPrefs.GetInt(SettingsMenu.ResolutionHeightPref),
            PlayerPrefs.GetInt(SettingsMenu.FullScreenPref) == 1);

        MenuSceneName = SceneManager.GetActiveScene().name;
    }

    public void LoadLevel(int index) {
        if (levelSceneNames == null || levelSceneNames.Length <= index) {
            Debug.LogError($"Level {index} not found!");
            return;
        }

        SceneManager.LoadScene(levelSceneNames[index]);
    }

    public void StartNewGame() {
        if (dialogueSceneNames == null || dialogueSceneNames.Length == 0) {
            Debug.LogError("No dialogues found!");
            return;
        }

        loadingScreen.GotoScene(dialogueSceneNames[0]);
        Destroy(this);
    }

    public void ContinueGame() {
        if (!PlayerPrefs.HasKey("lastSceneName") || PlayerPrefs.GetString("lastSceneName") == "") {
            Debug.LogWarning("last scene name not found in player pref, starting new game");
            StartNewGame();
            return;
        }
        string lastSceneName = PlayerPrefs.GetString("lastSceneName");
        loadingScreen.GotoScene(lastSceneName); // TODO: if level scene, load loadout
        Destroy(this);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
