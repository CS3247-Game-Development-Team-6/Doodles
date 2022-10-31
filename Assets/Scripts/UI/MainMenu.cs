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

        loadingScreen.SaveSceneToPref(mapInfos[0].dialogueSceneName);
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
        if (IsLevelIndex(latestSceneIndex)) {
            Loadout.mapToLoad = GetLatestMapInfo(latestSceneIndex);
            loadingScreen.GotoScene(loadoutSceneName);
            Destroy(this);
        } else {
            loadingScreen.GotoScene(GetLatestDialogueName(latestSceneIndex));
            Destroy(this);
        }
    }

    private bool IsLevelIndex(int index) {
        return index % 2 == 0 && index > 0;
    }

    private string GetLatestDialogueName(int index) {
        return mapInfos[(index - 1) / 2].dialogueSceneName;
    }

    private MapInfo GetLatestMapInfo(int index) {
        return mapInfos[index / 2 - 1];
    }

    public void QuitGame() {
        Application.Quit();
    }
}
