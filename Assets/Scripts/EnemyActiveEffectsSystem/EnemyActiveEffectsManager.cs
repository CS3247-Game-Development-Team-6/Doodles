using System;
using System.Collections.Generic;
using UnityEngine;

// Active Effect Manager for an enemy
// Currently applied after element effects
public class EnemyActiveEffectsManager : MonoBehaviour {
    // enemy this effect manager is attached to
    private Enemy enemy;
    
    // gel effect variables
    private bool hasGel = false;
    private GelEffect currentGelEffect;
    
    private void Start() {
        this.enemy = GetComponentInParent<Enemy>();
    }

    public void RecalculateSpeed() {
        if (currentGelEffect != null && currentGelEffect.isActivated) { // there is a current gel effect in effect
            currentGelEffect.RecalculateSpeed();
        }
    }

    public void HandleEffect(Effect effect) {
        if (effect is GelEffect) {
            if (hasGel == true) {
                // remove previous gel effect, prevent stacking of effect
                StartCoroutine(effect.Deactivate(this.enemy));
            }

            // track gel effect
            currentGelEffect = (GelEffect) effect;
            hasGel = true;
        }

        StartCoroutine(effect.Activate(this.enemy)); // apply effect
    }

    public void DeactivateEffect(Effect effect) {
        if (effect is GelEffect) {
            hasGel = false;

            StartCoroutine(currentGelEffect.Deactivate(this.enemy)); // remove effect
        }
    }
}
