using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;

    private static bool isGameEnded;
    public static int rounds;

    void Start() {
        isGameEnded = false;
        rounds = 0;
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

    // for other game scripts to check if the game is ended
    public static bool getIsGameEnded() {
        return isGameEnded;
    }
}
