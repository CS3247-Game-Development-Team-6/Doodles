using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameoverManager : MonoBehaviour {
    public Text wavesText;
    public GameObject raycastOccluder;

    // everytime this is enabled
    void OnEnable() {
        wavesText.text = WaveSpawner.wavesCounter.ToString();

        raycastOccluder.SetActive(true);
        //Time.timeScale = 0f;
        StartCoroutine(PauseGame());
    }

    IEnumerator PauseGame() {
        //yield on a new YieldInstruction that waits for seconds.
        //for camera shaking
        yield return new WaitForSeconds(1.5f);

        // Currently only works for single level (Hardcoded value here)
        FindObjectOfType<AudioManager>().Stop("Level 1 BGM");

        // pause
        Time.timeScale = 0f;

    }

    public void Retry() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
