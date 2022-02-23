using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour {

    public GameObject hitEffect; // TODO: get a hit effect for the bullets

    void Start() {
        //Ignore the collisions between layers "Player" and "PlayerBullets"
        Physics.IgnoreLayerCollision(6, 9);
    }
    void OnCollisionEnter(Collision collision) {
        // TODO: implement hit effect
        // Instantiate(hitEffect, transform.position, Quaternion.identity); // Quaternion.identity is the default rotation
        // Destroy(effect, 5f); // destroy after 5 ticks
        Destroy(gameObject);

        // TODO: add enemy damage
    }
}
