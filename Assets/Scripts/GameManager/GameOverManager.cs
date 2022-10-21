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
}
