using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/** Used by WinScreen and GameOverScreen
 */
public class GameOverManager : MonoBehaviour {
    public string mainMenuScene;
    public Text wavesText;
    public GameObject raycastOccluder;

    // Enabled when game is running
    void OnEnable() {
        wavesText.text = FindObjectOfType<Map>().WavesCleared.ToString();

        raycastOccluder.SetActive(true);

        // Currently only works for single level (Hardcoded value here)
        FindObjectOfType<AudioManager>().Stop("Level 1 BGM");

        StartCoroutine(PauseGame());

    }

    IEnumerator PauseGame() {
        // wait for 1.5s to show the full camera shake (if there is)
        yield return new WaitForSeconds(1.5f);

        // pause
        Time.timeScale = 0f;
    }

    public void Retry() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
        raycastOccluder.SetActive(false);
    }

    public void GotoMainMenu() {
        SceneManager.LoadScene(mainMenuScene);
        Time.timeScale = 1f;
        raycastOccluder.SetActive(false);
    }
}
