using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    // Remember to add the desired scene to the open scenes list under File -> Build Settings
    public static string MenuSceneName { get; private set; }
    public LoadingUI loadingScreen;
    [SerializeField] private string loadoutSceneName;
    //[SerializeField] private string[] levelSceneNames;
    [SerializeField] private MapInfo[] mapInfos;
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

    /*    public void LoadLevel(int index) {
            if (levelSceneNames == null || levelSceneNames.Length <= index) {
                Debug.LogError($"Level {index} not found!");
                return;
            }

            SceneManager.LoadScene(levelSceneNames[index]);
        }*/

    public void StartNewGame() {
        if (dialogueSceneNames == null || dialogueSceneNames.Length == 0) {
            Debug.LogError("No dialogues found!");
            return;
        }

        loadingScreen.GotoScene(dialogueSceneNames[0]);
        Destroy(this);
    }

    public void ContinueGame() {
        if (!PlayerPrefs.HasKey("latestSceneIndex")) {
            Debug.LogWarning("latestSceneIndex not found in player pref, starting new game");
            StartNewGame();
            return;
        }
        int latestSceneIndex = PlayerPrefs.GetInt("latestSceneIndex");
        if (latestSceneIndex % 2 == 0) {
            // load chapter
            Loadout.mapToLoad = mapInfos[latestSceneIndex / 2 - 1];
            loadingScreen.GotoScene(loadoutSceneName);
            Destroy(this);
        } else {
            // load dialogue
            loadingScreen.GotoScene(dialogueSceneNames[(latestSceneIndex - 1) / 2]);
            Destroy(this);
        }
    }

    public void QuitGame() {
        Application.Quit();
    }
}
