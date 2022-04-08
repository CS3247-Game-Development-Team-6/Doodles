using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Remember to add the desired scene to the open scenes list under File -> Build Settings
    public SettingsScriptableObject settings;
    public void PlayGame ()
    {
        settings.SetCurrGameScene();
    }

    public void QuitGame ()
    {
        Application.Quit();
    }
}
