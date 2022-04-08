using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

public class SettingsMenu : MonoBehaviour
{

    public ResolutionInfo[] resolutions;
    public int currIndex = -1;
    public Vector2Int defaultResolution;
    public string defaultMenuScene;
    public string defaultGameScene;
    public Dropdown resolutionDropdown;
    public bool inMenu;
    public Slider volumeSlider;

    public AudioMixer audioMixer;

    private void Start() {

        // check if this is a menu or a game scene
        inMenu = SceneManager.GetActiveScene().name.StartsWith("Menu");

        // reset resolution options
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++) {
            ResolutionInfo info = resolutions[i];
            options.Add(info.widthHeight.x + " x " + info.widthHeight.y);
            if (info.widthHeight.Equals(Screen.currentResolution)) {
                currIndex = i;
                Screen.SetResolution(info.widthHeight.x, info.widthHeight.y, Screen.fullScreen);
                if (inMenu)
                    SceneManager.LoadScene(info.menuScene);
                else
                    SceneManager.LoadScene(info.gameScene);
                Debug.Log("resolution set to " + info.widthHeight);
                return;
            }

        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currIndex;
        resolutionDropdown.RefreshShownValue();

        SetVolume(volumeSlider.value);
    }

    public void SetResolutionIndex(int index) {
        currIndex = index;
        ResolutionInfo info = resolutions[index];
        Screen.SetResolution(info.widthHeight.x, info.widthHeight.y, Screen.fullScreen);

        // check if this is a menu or a game scene
        inMenu = SceneManager.GetActiveScene().name.StartsWith("Menu");

        if (inMenu)
            SceneManager.LoadScene(info.menuScene);
        else
            SceneManager.LoadScene(info.gameScene);
        resolutionDropdown.value = currIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetVolume (float volume) {
        audioMixer.SetFloat("volume", Mathf.Log10(volume)*20);

    }

    public void SetFullScreen(bool isFullScreen) {
        Screen.fullScreen = isFullScreen;
    }

    public void LoadMenuScene() {
        SceneManager.LoadScene(currIndex < 0 ? defaultMenuScene : resolutions[currIndex].menuScene);
    }

    public void LoadGameScene() {
        SceneManager.LoadScene(currIndex < 0 ? defaultGameScene : resolutions[currIndex].gameScene);
    }
    public void ReloadScene() {
        Start();
    }
}
