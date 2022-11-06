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
            (!PlayerPrefs.HasKey("latestSceneIndex")
            || !CanContinue(PlayerPrefs.GetInt("latestSceneIndex")))) {
            transform.Find("Continue").gameObject.SetActive(false);
        }
        staticMapInfos = mapInfos;

    }

    private bool CanContinue(int index) {
        if (IsLevelIndex(index)) {
            return GetLatestLevelName(index) != null;
        } else {
            return GetLatestDialogueName(index) != null;
        }
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

        if (!CanContinue(latestSceneIndex)) {
            Debug.LogWarning("cannot continue game");
            return;
        }

        // TODO: add check if scene index exceeds
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
        int mapInfosIndex = (index - 1) / 2;
        if (mapInfosIndex >= mapInfos.Length) {
            Debug.LogWarning("mapInfos' index exceeds");
            return null;
        }
        if (mapInfos[mapInfosIndex].dialogueSceneName == "") {
            Debug.LogWarning("dialogue scene name empty");
            return null;
        }
        return mapInfos[mapInfosIndex].dialogueSceneName;
    }

    private string GetLatestLevelName(int index) {
        int mapInfosIndex = index / 2 - 1;
        if (mapInfosIndex >= mapInfos.Length) {
            Debug.LogWarning("mapInfos' index exceeds");
            return null;
        }
        if (mapInfos[mapInfosIndex].gameSceneName == "") {
            Debug.LogWarning("game scene name empty");
            return null;
        }
        return mapInfos[mapInfosIndex].gameSceneName;
    }

    private MapInfo GetLatestMapInfo(int index) {
        int mapInfosIndex = index / 2 - 1;
        if (mapInfosIndex >= mapInfos.Length) {
            Debug.LogWarning("mapInfos' index exceeds");
            return null;
        }
        return mapInfos[mapInfosIndex];
    }

    public void QuitGame() {
        Application.Quit();
    }
}
