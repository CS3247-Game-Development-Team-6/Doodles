using UnityEngine;
using UnityEngine.SceneManagement;


[System.Serializable]
public class ResolutionInfo {
    public Vector2Int widthHeight;
    public string menuScene;
    public string gameScene;

    public ResolutionInfo(Vector2Int widthHeight, string menuScene, string gameScene) {
        this.widthHeight = widthHeight;
        this.menuScene = menuScene;
        this.gameScene = gameScene;
    }
}

[CreateAssetMenu(fileName = "DefaultSettingsValues", menuName = "Settings")]
public class SettingsScriptableObject : ScriptableObject
{
    public ResolutionInfo[] resolutions;
    public int currIndex = 0;
    public float volumeSliderValue;
    public bool inMenu;
    public bool isFullScreen;
    public static readonly string ResolutionPrefx = "ResolutionPrefx";
    public static readonly string ResolutionPrefy = "ResolutionPrefy";
    public static readonly string GameScenePref = "GameScenePref";
    public static readonly string MenuScenePref = "MenuScenePref";
    public static readonly string ResIndexPref = "ResIndexPref";
    public static readonly string FullScreenPref = "FullScreenPref";
    public static readonly int FULLSCREEN = 1;

    public static void Init() {
        PlayerPrefs.SetFloat(ResolutionPrefx, 1920);
        PlayerPrefs.SetFloat(ResolutionPrefy, 1080);
        PlayerPrefs.SetString(GameScenePref, "ShaderScene");
        PlayerPrefs.SetString(MenuScenePref, "Menu");
        PlayerPrefs.SetInt(ResIndexPref, 0);
        PlayerPrefs.SetInt(FullScreenPref, 0);
    }

    private void SetPlayerPrefs(int index) {
        PlayerPrefs.SetFloat(ResolutionPrefx, resolutions[index].widthHeight.x);
        PlayerPrefs.SetFloat(ResolutionPrefy, resolutions[index].widthHeight.y);
        PlayerPrefs.SetString(GameScenePref, resolutions[index].gameScene);
        PlayerPrefs.SetString(MenuScenePref, resolutions[index].menuScene);
        PlayerPrefs.SetInt(ResIndexPref, index);
    }

    public int findSetCurrIndex() {
        for (int i = 0; i < resolutions.Length; i++) {
            if (resolutions[i].widthHeight.x == Screen.width && 
                resolutions[i].widthHeight.y == Screen.height &&
                (SceneManager.GetActiveScene().Equals(resolutions[i].gameScene) ||
                SceneManager.GetActiveScene().Equals(resolutions[i].menuScene))) {
                currIndex = i;
                SetPlayerPrefs(i);
                return i;
            }
        }
        return currIndex;
    }

    public void SetResolutionIndex(int index) {
        currIndex = index;
        SetPlayerPrefs(index);
        ResolutionInfo info = resolutions[index];
        Screen.SetResolution(info.widthHeight.x, info.widthHeight.y, Screen.fullScreen);
        inMenu = SceneManager.GetActiveScene().name.StartsWith("Menu");
        SceneManager.LoadScene(inMenu ? info.menuScene : info.gameScene);
    }

    public void SetFullScreen(bool isFullScreen) {
        this.isFullScreen = isFullScreen;
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt(FullScreenPref, isFullScreen ? 1 : 0);
    }
}
