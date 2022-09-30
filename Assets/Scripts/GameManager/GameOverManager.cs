using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour {
    public Text wavesText;
    public GameObject raycastOccluder;
    public string retrySceneName;

    // Enabled when game is running
    void OnEnable() {
        wavesText.text = ChunkSpawner.totalWaveCount.ToString();

        raycastOccluder.SetActive(true);

        // Currently only works for single level (Hardcoded value here)
        FindObjectOfType<AudioManager>().Stop("Level 1 BGM");

        StartCoroutine(PauseGame());

        GameObject[] ddols = GameStateManager.GetDontDestroyOnLoadObjects();
        for (int i = 0; i < ddols.Length; i++) {
            Destroy(ddols[i]);
        }
    }

    IEnumerator PauseGame() {
        // wait for 1.5s to show the full camera shake (if there is)
        yield return new WaitForSeconds(1.5f);

        // pause
        Time.timeScale = 0f;

    }

    public void Retry() {
        // load current active scene
        // deprecated: SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        GameObject[] ddols = GameStateManager.GetDontDestroyOnLoadObjects();
        for (int i = 0; i < ddols.Length; i++) {
            Destroy(ddols[i]);
        }

        SceneManager.LoadScene(retrySceneName);

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
