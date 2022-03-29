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

        StartCoroutine(PauseGame());
    }

    IEnumerator PauseGame()
    {
        //yield on a new YieldInstruction that waits for seconds.
        //for camera shaking
        yield return new WaitForSeconds(2);

        // pause
        Time.timeScale = 0f;
    }

    public void Retry() {

    }

    public void Menu() {
        
    }
}
