using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused;

    public GameObject pauseMenuUI;
    public GameObject gameplayCanvas;
    public GameObject raycastOccluder;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        gameplayCanvas.SetActive(true);
        raycastOccluder.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Resume();
    }

    void Pause()
    {
        if (!GameManager.getIsGameEnded())
        {
            GameIsPaused = true;
            pauseMenuUI.SetActive(true);
            gameplayCanvas.SetActive(false);
            raycastOccluder.SetActive(true);
            Time.timeScale = 0f;
        }
        
        TooltipSystem.Hide();
    }

    public void Freeze() {
        pauseMenuUI.SetActive(true);
        raycastOccluder.SetActive(true);
        Time.timeScale = 0f;
    }

    public void LoadMenu()
    {
        Resume();
        raycastOccluder.SetActive(false);
        if (!PlayerPrefs.HasKey(SettingsScriptableObject.MenuScenePref)) {
            SettingsScriptableObject.Init();
        }

        string menuScene = PlayerPrefs.GetString(SettingsScriptableObject.MenuScenePref);

        // To prevent infinitely opening the same scene which crashes the app
        if (SceneManager.GetActiveScene().name.Equals(menuScene)) return;

        SceneManager.LoadScene(menuScene);
    }

    public void Retry()
    {
        Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public bool IsPaused() {
        return GameIsPaused;
    }

}
