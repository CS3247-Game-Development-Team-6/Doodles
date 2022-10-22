using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour {

    public ParticleSystem hitEffect; 

    public int bulletDamage = 20;

    private float maxLifeTime = 2f;
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
        if (other.collider.CompareTag("Enemy")) {
            other.collider.GetComponentInParent<Enemy>().TakeDamage(bulletDamage, null);
        }
        Instantiate(hitEffect, transform.position, Quaternion.identity); // Quaternion.identity is the default rotation
        Destroy(this.gameObject);
    }

    private void CheckLifeTime() {
        if (currentLifeTime <= 0) {
            Destroy(this.gameObject);
        }
    }
}
