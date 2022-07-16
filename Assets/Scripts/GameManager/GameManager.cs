using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Text

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject winUI;
    [SerializeField] private Text wavesCounterUI;

    private static bool isGameEnded;
    public static int wavesCounter;


    void Start() {
        isGameEnded = false;
        wavesCounter = 0;
    }

    void Update() {
        if (isGameEnded)
            return;

        if (Base.getHp() <= 0) {
            EndGame();
        }

        wavesCounterUI.text = "Wave " + string.Format("{0}", wavesCounter);
    }

    public void EndGame() {
        isGameEnded = true;
        
        gameOverUI.SetActive(true);
    }

    public void WinGame()
    {
        isGameEnded = true;

        winUI.SetActive(true);
    }

    // for other game scripts to check if the game is ended
    public static bool getIsGameEnded() {
        return isGameEnded;
    }
}
