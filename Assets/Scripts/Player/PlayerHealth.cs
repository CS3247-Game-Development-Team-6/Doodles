using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
    /*
    Requires player game object to have a "HealthCanvas/HealthBar" game object containing a health bar Image.
    */
    public float healthAmount;
    public float maxHealth = 100;

    private Image healthBar;
    private GameStateManager gameStateManager;

    void Start() {
        healthBar = GameObject.Find("HealthCanvas/HealthBG/HealthBar").GetComponent<Image>();
        healthAmount = maxHealth;
        gameStateManager = GameObject.Find("GameMaster").GetComponent<GameStateManager>();

        Load();
    }

    public void TakeDamage(float amount) {
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

    public void Save() {
        PlayerData data = new PlayerData {
            health = healthAmount,
            maxHealth = maxHealth,
            playerPos = transform.position,
        };

        string json = JsonUtility.ToJson(data);
        string path = Application.dataPath + "/player.json";
        File.WriteAllText(path, json);
        Debug.Log($"Saving: {json} at {path}");

    }

    public bool Load() {
        string path = Application.dataPath + "/player.json";
        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            maxHealth = data.maxHealth;
            TakeDamage(data.maxHealth - data.health);
            transform.position = data.playerPos;
            Debug.Log($"health {healthAmount}; max {maxHealth}; {transform.position}");
            return true;
        } else {
            Debug.Log("no save found.");
            return false;
        }
    }

    private class PlayerData {
        public float health;
        public float maxHealth;
        public Vector3 playerPos;
    }
}
