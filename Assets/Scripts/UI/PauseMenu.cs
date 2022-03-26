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

    void Start()
    {
        Resume();   // Unfreeze game
    }

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

    void Pause()
    {
        if (!GameManager.getIsGameEnded())
        {
            pauseMenuUI.SetActive(true);
            gameplayCanvas.SetActive(false);
            raycastOccluder.SetActive(true);
            Time.timeScale = 0f;
            GameIsPaused = true;
        }
    }

    public void LoadMenu()
    {
        Resume();
        raycastOccluder.SetActive(false);
        SceneManager.LoadScene(0);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
