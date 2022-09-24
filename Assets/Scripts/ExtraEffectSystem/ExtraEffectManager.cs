using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This manages extra effects on enemies not tied to elements, and it applied 
 * after element effects
 */
public class ExtraEffectManager : MonoBehaviour {
    private Enemy enemy; // enemy object that this manager belongs to

    // values that can be set
    public float gelLifetime = 2.0f; // lifetime of the effect
    public float gelSlowAmount = 0.9f; // value 0 to 1 applied to speed of enemy
    
    private bool hasGel = false; // separate effect from element effects
    private float currentEffectTime = 0f;
    
    private float enemyCurrentBaseSpeed; // current speed with element effects
    private float enemyCalculatedGelSpeed; // speed of enemy with element effects and gel

    void Start() {
        enemy = GetComponentInParent<Enemy>();
        calculateGelSpeed();
    }

    // Update is called once per frame
    void Update() { 
        HandleGelEffect();
    }

    public void applyGel() {
        this.hasGel = true;
    }

    // should be called when enemy speed is affected
    public void calculateGelSpeed() {
        this.enemyCurrentBaseSpeed = enemy.getSpeed();
        this.enemyCalculatedGelSpeed = enemyCurrentBaseSpeed * gelSlowAmount;
    }

    private void HandleGelEffect() {
        if (!hasGel) {
            // no gel, nothing to handle
            return;
        }

        if (currentEffectTime >= gelLifetime) {
            // gel lifetime is up, remove gel
            this.hasGel = false;
            currentEffectTime = 0.0f;
            enemy.setSpeed(enemyCurrentBaseSpeed); // reset speed
            return;
        }

        currentEffectTime += Time.deltaTime; // increment time counter

        enemy.setSpeed(enemyCalculatedGelSpeed);
        // TODO: setting speed like this might be problematic later
    }
}
