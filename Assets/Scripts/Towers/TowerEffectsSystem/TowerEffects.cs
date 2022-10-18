using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Active Effect Manager for a tower
public class TowerEffects : MonoBehaviour {
    // tower this effect manager is attached to
    private Tower tower;

    private HashSet<string> activeEffects;

    private void Start() {
        tower = GetComponent<Tower>();
        activeEffects = new HashSet<string>();
    }

    public IEnumerator HandleEffect(ITowerEffect effect) {
        var effectKey = effect.GetKey();
        if (activeEffects.Contains(effectKey)) {
            //Debug.LogWarning($"Effect {effectKey} is already applied and cannot be reapplied.");
        } else {
            //Debug.Log($"Effect {effectKey} is being applied and added!");
            StartCoroutine(effect.Activate(this.tower)); // apply effect
            activeEffects.Add(effectKey);
            yield return new WaitForSeconds(effect.GetLifetime());
            DeactivateEffect(effect);
        }
    }

    public void DeactivateEffect(ITowerEffect effect) {
        var effectKey = effect.GetKey();
        if (!activeEffects.Contains(effectKey)) {
            //Debug.LogWarning($"Effect {effectKey} is not active on {enemy}.");
        } else {
            //Debug.Log($"Effect {effectKey} is being deactivated and removed!");
            StartCoroutine(effect.Deactivate(this.tower)); // remove effect
            activeEffects.Remove(effectKey);
        }
    }
}
