using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Remember to add the desired scene to the open scenes list under File -> Build Settings
    public string scene;
    public void PlayGame ()
    {
        SceneManager.LoadScene(scene);
    }

    public void QuitGame ()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
