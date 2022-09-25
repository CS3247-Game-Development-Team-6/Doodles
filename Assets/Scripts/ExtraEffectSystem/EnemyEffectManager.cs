using System;
using System.Collections.Generic;
using UnityEngine;

// Global Effect Manager for enemies, 
public class EnemyEffectManager : MonoBehaviour {
    public static EnemyEffectManager instance {get; private set;}

    private void Start() {
        if (instance != null) {
            Debug.LogWarning("EffectManager already exists. Remove this component.");
        } else { 
            instance = this;
        }
    }
    public void HandleEffect(Effect effect, Enemy enemy) {
        StartCoroutine(effect.Activate(enemy));
    }
    public void DeactivateEffect(Effect effect, Enemy enemy) {
        StartCoroutine(effect.Deactivate(enemy));
    }
}
