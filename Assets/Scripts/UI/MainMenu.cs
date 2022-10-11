using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    // Remember to add the desired scene to the open scenes list under File -> Build Settings

    public void Awake() {
        if (!PlayerPrefs.HasKey(SettingsMenu.ResolutionHeightPref)) {
            PlayerPrefs.SetInt(SettingsMenu.ResolutionWidthPref, 1920);
            PlayerPrefs.SetInt(SettingsMenu.ResolutionHeightPref, 1080);
            PlayerPrefs.SetInt(SettingsMenu.FullScreenPref, 1);
        }

        Screen.SetResolution(PlayerPrefs.GetInt(SettingsMenu.ResolutionWidthPref), 
            PlayerPrefs.GetInt(SettingsMenu.ResolutionHeightPref), 
            PlayerPrefs.GetInt(SettingsMenu.FullScreenPref) == 1);
    }

    public void LoadScene() {

    }

    public void PlayGame() {
        if (!PlayerPrefs.HasKey(SettingsScriptableObject.GameScenePref)) {
            SettingsScriptableObject.Init();
        }

        //string gameScene = PlayerPrefs.GetString(SettingsScriptableObject.GameScenePref);
        string gameScene = "tutorial_scene";
        SceneManager.LoadScene(gameScene);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
