using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : MonoBehaviour {
    public string baseTag = "Base";

    // List that holds all the towers & base in range
    private List<GameObject> towerInRange;

    public float range;
    public float bombDamage;
    public float fireCountDown;

    [Header("Effect Prefabs")]
    public GameObject selfDestructEffect;
    public GameObject selfDestructSound;


    public void Start() {
        towerInRange = new List<GameObject>();
        UpdateTarget();
    }

    private void UpdateTarget() {
        Tower[] towers = GameObject.FindObjectsOfType<Tower>();
        GameObject[] bases = GameObject.FindGameObjectsWithTag(baseTag);

        foreach (Tower tempTarget in towers) {
            float distanceToPlayer = Vector3.Distance(transform.position, tempTarget.transform.position);
            if (distanceToPlayer <= range) {
                towerInRange.Add(tempTarget.gameObject);
            }
        }

        foreach (GameObject tempTarget in bases) {
            // range from the model
            float distanceToPlayer = Vector3.Distance(transform.position, tempTarget.transform.position);
            if (distanceToPlayer <= range) {
                towerInRange.Add(tempTarget);
            }
        }
    }

    public void Shoot() {
        foreach (var tower in towerInRange) {
            Tower towerScript = tower.GetComponent<Tower>();
            if (towerScript) {
                float distance = Vector3.Distance(transform.position, tower.transform.position);
                float damage = CalculateDamage(distance);
                towerScript.DecreaseHealth(damage);
            }

            if (tower.CompareTag("Base")) {
                if (tower.GetComponent<Base>() == null) {
                    Debug.LogError($"Missing Base script on {tower}");
                }
                float distance = Vector3.Distance(transform.position, tower.transform.position);
                float damage = CalculateDamage(distance);
                tower.GetComponent<Base>().TakeDmg(Mathf.RoundToInt(damage));
            }
        }
        TriggerEffect();
        Destroy(gameObject);
    }

    // Trigger visual and sound effects
    private void TriggerEffect() {
        GameObject selfDestructEffectParticle = (GameObject)Instantiate(selfDestructEffect, transform.position, transform.rotation);
        GameObject selfDestructEffectAudio = (GameObject)Instantiate(selfDestructSound, transform.position, transform.rotation);
        selfDestructEffectAudio.GetComponent<AudioSource>().Play();
    }

    // Calculate damage based on distance of the enemy from the origin of the landmine orb
    private float CalculateDamage(float distance) {
        float cosDamage = .5f * Mathf.Cos((1 / (0.4f * range) * distance)) + .5f;
        return bombDamage * cosDamage;
    }

    private void Update() {
        if (fireCountDown <= 0) {
            Shoot();
            return;
        }
        fireCountDown -= Time.deltaTime;
    }

    // Debug Tool: Draw a sphere to show the range of the tower
    // private void OnDrawGizmosSelected() {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, 3);
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(firePoint.position, 0.1f);
    // }
}
