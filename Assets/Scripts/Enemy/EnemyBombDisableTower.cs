using System.Collections.Generic;
using UnityEngine;

public class EnemyBombDisableTower : MonoBehaviour {

    // List that holds all the towers in range
    private List<GameObject> towerInRange;

    public float range;
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
        foreach (Tower tempTarget in towers) {
            float distanceToPlayer = Vector3.Distance(transform.position, tempTarget.transform.position);
            if (distanceToPlayer <= range) {
                towerInRange.Add(tempTarget.gameObject);
            }
        }
    }

    public void Shoot() {
        foreach (var tower in towerInRange) {
            Tower towerScript = tower.GetComponent<Tower>();
            if (towerScript) {
                towerScript.ApplyEffect(new StopTowerShootingEffect()); // todo: add vfx using constructor
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
