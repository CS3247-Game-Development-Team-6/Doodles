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

    public Vector2Int GetResolution() {
        return resolutions[currIndex].widthHeight;
    }

    public void SetCurrMenuScene() {
        inMenu = true;
        SceneManager.LoadScene(resolutions[currIndex].menuScene);
    } 

    public void SetCurrGameScene() {
        inMenu = false;
        SceneManager.LoadScene(resolutions[currIndex].gameScene);
    } 

    public int SetCurrIndex() {
        for (int i = 0; i < resolutions.Length; i++) {
            if (resolutions[i].widthHeight.x == Screen.width && 
                resolutions[i].widthHeight.y == Screen.height) {
                currIndex = i;
                return i;
            }
        }
        return currIndex;
    }

    public void SetResolutionIndex(int index) {
        currIndex = index;
        ResolutionInfo info = resolutions[index];
        Screen.SetResolution(info.widthHeight.x, info.widthHeight.y, Screen.fullScreen);
        inMenu = SceneManager.GetActiveScene().name.StartsWith("Menu");
        SceneManager.LoadScene(inMenu ? info.menuScene : info.gameScene);
    }

    public void SetIsMenu(Scene scene) {
        inMenu = scene.name.StartsWith("Menu");
    }

    public void SetFullScreen(bool isFullScreen) {
        this.isFullScreen = isFullScreen;
        Screen.fullScreen = isFullScreen;
    }
}
