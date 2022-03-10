using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static bool gameEnded = false;

    void Update() {
        if (gameEnded)
            return;

        if (Base.getHp() <= 0) {
            EndGame();
        }
    }

    void EndGame() {
        gameEnded = true;
        Debug.Log("Game Over!");
    }

    // for other game scripts to check if the game is ended
    public static bool isGameEnded() {
        return gameEnded;
    }
}
