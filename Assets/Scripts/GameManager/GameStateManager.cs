using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Text

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject winUI;

    private static bool isGameEnded;


    void Start() {
        isGameEnded = false;
    }

    void Update() {
        if (isGameEnded)
            return;

        if (Base.getHp() <= 0) {
            EndGame();
        }
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
