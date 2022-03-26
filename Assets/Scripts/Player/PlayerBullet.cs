using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour {

    public GameObject hitEffect; // TODO: get a hit effect for the bullets
    private int bulletDamage = 30;
    private float maxLifeTime = 10f;
    private float currentLifeTime;

    void Start() {
        // Ignore the collisions between layers "Player" and "PlayerBullets"
        Physics.IgnoreLayerCollision(6, 9);
        currentLifeTime = maxLifeTime;
    }

    void Update() {
        CheckLifeTime();
    }

    void FixedUpdate() {
        if (currentLifeTime > 0) {
            currentLifeTime -= Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision other) {
        // TODO: implement hit effect
        // Instantiate(hitEffect, transform.position, Quaternion.identity); // Quaternion.identity is the default rotation
        // Destroy(effect, 5f); // destroy after 5 ticks
        
        if (other.collider.CompareTag("Enemy")) {
            other.collider.GetComponentInParent<Enemy>().TakeDamage(bulletDamage);
        }
        Destroy(gameObject);
    }

    private void CheckLifeTime() {
        if (currentLifeTime <= 0) {
            Destroy(gameObject);
        }
    }
}
