using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour {
    public Text wavesText;
    public GameObject raycastOccluder;

    // Enabled when game is running
    void OnEnable() {
        wavesText.text = WaveSpawner.wavesCounter.ToString();

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
        // load current active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // remove occluder
        raycastOccluder.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Menu() {
        // Resume time
        Time.timeScale = 1f;
        if (!PlayerPrefs.HasKey(SettingsScriptableObject.MenuScenePref)) {
            SettingsScriptableObject.Init();
        }

        string menuScene = PlayerPrefs.GetString(SettingsScriptableObject.MenuScenePref);

        // To prevent infinitely opening the same scene which crashes the app
        if (SceneManager.GetActiveScene().name.Equals(menuScene)) return;

        SceneManager.LoadScene(menuScene);
    }
}
