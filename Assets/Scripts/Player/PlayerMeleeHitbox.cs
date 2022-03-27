using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeHitbox : MonoBehaviour
{
    /*
    --IMPORTANT--
    Player melee hitbox can be counted under PlayerBullets layer as they serve the same function.
    */
    public GameObject hitEffect; // TODO: get a hit effect for melee actions
    private int meleeDamage = 20;
    private float maxLifeTime = 0.15f;
    private float currentLifeTime;
    private List<Collider> colliderList;
    private bool hasAppliedDamage = false;

    void Start() {
        // Ignore the collisions between layers "Player" and "PlayerBullets"
        Physics.IgnoreLayerCollision(6, 9);
        currentLifeTime = maxLifeTime;
        colliderList = new List<Collider>();
    }

    void Update() {
        CheckLifeTime();
    }

    void FixedUpdate() {
        if (currentLifeTime > 0) {
            currentLifeTime -= Time.deltaTime;
        }
    }

    void OnCollisionStay(Collision other) {
        if (!colliderList.Contains(other.collider)) { 
            colliderList.Add(other.collider);
        }
    }

    private void ApplyDamage() { // apply damage only once
        if (hasAppliedDamage) {
            return;
        }

        foreach (Collider collider in colliderList) {
            if (!collider) continue;    // should not continue if the game object has already been destroyed.
            if (collider.CompareTag("Enemy")) {
                // TODO: implement hit effect
                // Instantiate(hitEffect, transform.position, Quaternion.identity); // Quaternion.identity is the default rotation
                // Destroy(effect, 5f); // destroy after 5 ticks

                collider.GetComponentInParent<Enemy>().TakeDamage(meleeDamage);
            }
        }

        hasAppliedDamage = true;
    }

    private void CheckLifeTime() {
        if (currentLifeTime <= 0) {
            ApplyDamage();
            Destroy(gameObject);
        }
    }
}
