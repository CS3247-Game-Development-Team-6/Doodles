using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/** Used by WinScreen and GameOverScreen
 */
public class GameOverManager : MonoBehaviour {
    public string mainMenuScene;
    public string nextDialogueScene;
    public LoadingUI loadingScreen;
    public Text wavesText;
    public GameObject raycastOccluder;

    // Enabled when game is running
    void OnEnable() {
        if (this.gameObject.name == "WinScreen") {
            loadingScreen.SaveSceneToPref(nextDialogueScene);
        }

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // TODO: add loadingUI
        Time.timeScale = 1f;
        raycastOccluder.SetActive(false);
    }

    public void GotoMainMenu() {
        if (mainMenuScene == "") {
            Debug.LogWarning("empty name for main menu scene ");
            return;
        }
        loadingScreen.GotoScene(mainMenuScene);
        Time.timeScale = 1f;
        raycastOccluder.SetActive(false);
        Destroy(this);
    }

    public void GotoNextDialogue() {
        if (nextDialogueScene == "") {
            Debug.LogWarning("empty name for next dialogue scene ");
            return;
        }

        if (nextDialogueScene == "story-end") {
            Debug.LogWarning("story and game end here");
            return;
        }
        loadingScreen.GotoScene(nextDialogueScene);
        Time.timeScale = 1f;
        raycastOccluder.SetActive(false);
        Destroy(this);
    }
}
