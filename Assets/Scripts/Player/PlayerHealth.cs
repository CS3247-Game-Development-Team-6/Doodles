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

    // Start is called before the first frame update
    void Start() {
        healthBar = GameObject.Find("HealthCanvas/HealthBar").GetComponent<Image>();
        healthAmount = maxHealth;
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void TakeDamage(int amount) {
        healthAmount -= amount;

        // float number between 0 and 1
        healthBar.fillAmount = healthAmount / maxHealth;

        // TODO: add damage animation?

        if (healthAmount <= 0) {
            Die();
        }
    }

    void Die() {
        Destroy(gameObject);
        // TODO: add death animation?
        // TODO: add game over event
    }
}