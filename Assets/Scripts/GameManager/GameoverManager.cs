using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameoverManager : MonoBehaviour
{
    public Text wavesText;
    public GameObject raycastOccluder;

    // everytime this is enabled
    void OnEnable()
    {
        wavesText.text = GameManager.rounds.ToString();
        
        raycastOccluder.SetActive(true);
        //Time.timeScale = 0f;
        StartCoroutine(PauseGame());
    }

    IEnumerator PauseGame()
    {
        //yield on a new YieldInstruction that waits for seconds.
        //for camera shaking
        yield return new WaitForSeconds(1.5f);

        // pause
        Time.timeScale = 0f;
    }

    public void Retry() {

    }

    public void Menu() {
        
    }
}
