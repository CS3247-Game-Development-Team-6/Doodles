using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Remember to add the desired scene to the open scenes list under File -> Build Settings

    public void Awake() {
        if (!PlayerPrefs.HasKey(SettingsScriptableObject.MenuScenePref)) {
            SettingsScriptableObject.Init();
        }

        string menuScene = PlayerPrefs.GetString(SettingsScriptableObject.MenuScenePref);

        // To prevent infinitely opening the same scene which crashes the app
        if (SceneManager.GetActiveScene().name.Equals(menuScene)) return;

        Screen.fullScreen = 
            PlayerPrefs.GetInt(SettingsScriptableObject.FullScreenPref) == SettingsScriptableObject.FULLSCREEN;

        SceneManager.LoadScene(menuScene);
    }

    public void PlayGame ()
    {
        if (!PlayerPrefs.HasKey(SettingsScriptableObject.GameScenePref)) {
            SettingsScriptableObject.Init();
        }

        string gameScene = PlayerPrefs.GetString(SettingsScriptableObject.GameScenePref);
        SceneManager.LoadScene(gameScene);
    }

    public void QuitGame () {
        Application.Quit();
    }
}
