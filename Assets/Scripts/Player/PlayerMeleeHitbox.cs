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

    void Start() {
        // Ignore the collisions between layers "Player" and "PlayerBullets"
        Physics.IgnoreLayerCollision(6, 9);
    }
    void OnCollisionEnter(Collision other) {
        // TODO: implement hit effect
        // Instantiate(hitEffect, transform.position, Quaternion.identity); // Quaternion.identity is the default rotation
        // Destroy(effect, 5f); // destroy after 5 ticks
        
        if (other.collider.CompareTag("Enemy")) {
            other.collider.GetComponentInParent<Enemy>().TakeDamage(meleeDamage);
        }
        Destroy(gameObject);
    }
}
