using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    // Remember to add the desired scene to the open scenes list under File -> Build Settings
    public static string MenuSceneName { get; private set; }
    public LoadingUI loadingScreen;
    [SerializeField] private string loadoutSceneName;
    [SerializeField] private MapInfo[] mapInfos; // contain levelSceneNames and dialogueSceneNames

    public static MapInfo[] staticMapInfos;

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

        if (transform.Find("Continue").gameObject &&
            (!PlayerPrefs.HasKey("latestSceneIndex") || PlayerPrefs.GetInt("latestSceneIndex") > (2 * mapInfos.Length))) {
            transform.Find("Continue").gameObject.SetActive(false);
        }
        staticMapInfos = mapInfos;
    }

    public void StartNewGame() {
        if (mapInfos == null || mapInfos.Length == 0 || mapInfos[0].dialogueSceneName == "") {
            Debug.LogError("No dialogues found!");
            return;
        }

        loadingScreen.GotoScene(mapInfos[0].dialogueSceneName);
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
            loadingScreen.GotoScene(mapInfos[(latestSceneIndex - 1) / 2].dialogueSceneName);
            Destroy(this);
        }
    }

    public void QuitGame() {
        Application.Quit();
    }
}
