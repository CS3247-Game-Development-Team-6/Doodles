using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
    /*
    Requires player game object to have a "HealthCanvas/HealthBar" game object containing a health bar Image.
    */
    private float healthAmount;
    private float maxHealth = 100;

    private Image healthBar;
    private GameStateManager gameStateManager;

    void Start() {
        healthBar = GameObject.Find("HealthCanvas/HealthBG/HealthBar").GetComponent<Image>();
        healthAmount = maxHealth;
        gameStateManager = GameObject.Find("GameMaster").GetComponent<GameStateManager>();
    }

    public void TakeDamage(int amount) {
        healthAmount -= amount;

        // float number between 0 and 1
        healthBar.fillAmount = healthAmount / maxHealth;

        if (healthAmount <= 0) {
            Die();
        }
    }

    void Die() {
        gameObject.SetActive(false);
        gameStateManager.EndGame();
    }
}
