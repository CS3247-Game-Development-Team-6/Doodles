using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Active Effect Manager for an enemy
// Currently applied after element effects
public class EnemyActiveEffects : MonoBehaviour {
    // enemy this effect manager is attached to
    private Enemy enemy;

    private HashSet<string> activeEffects;

    private void Start() {
        enemy = GetComponent<Enemy>();
        activeEffects = new HashSet<string>();
    }

    public IEnumerator HandleEffect(IEnemyEffect effect) {
        var effectKey = effect.GetKey();
        if (activeEffects.Contains(effectKey)) {
            //Debug.LogWarning($"Effect {effectKey} is already applied and cannot be reapplied.");
        } else {
            //Debug.Log($"Effect {effectKey} is being applied and added!");
            StartCoroutine(effect.Activate(this.enemy)); // apply effect
            activeEffects.Add(effectKey);
            yield return new WaitForSeconds(effect.GetLifetime());
            DeactivateEffect(effect);

        }
    }

    public void DeactivateEffect(IEnemyEffect effect) {
        var effectKey = effect.GetKey();
        if (!activeEffects.Contains(effectKey)) {
            //Debug.LogWarning($"Effect {effectKey} is not active on {enemy}.");
        } else {
            //Debug.Log($"Effect {effectKey} is being deactivated and removed!");
            StartCoroutine(effect.Deactivate(this.enemy)); // remove effect
            activeEffects.Remove(effectKey);
        }
    }
}
