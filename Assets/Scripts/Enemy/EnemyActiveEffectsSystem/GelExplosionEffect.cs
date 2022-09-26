using System.Collections;
using UnityEngine;

public class GelExplosionEffect : IEnemyEffect {

    // values that can be set
    public float gelLifetime = 2.0f; // lifetime of the effect
    // calls the coroutine Deactivate after yielding for ^ seconds
    public float gelSlowAmount = 0.1f; // value 0 to 1 applied to speed of enemy

    public string GetKey() {
        return "Gel";
    }

    public float GetLifetime() {
        return gelLifetime;
    }

    public IEnumerator Activate(Enemy enemy) {
        float debugBaseSpeed = enemy.getFinalSpeed();
        enemy.speedMultiplier *= gelSlowAmount;
        Debug.Log($"{enemy} speed: slow amount {gelSlowAmount}, orig {debugBaseSpeed}, final: {enemy.getFinalSpeed()}");
        yield return new WaitForSeconds(gelLifetime);
    }

    public IEnumerator Deactivate(Enemy enemy) {
        float debugBaseSpeed = enemy.getFinalSpeed();
        enemy.speedMultiplier /= gelSlowAmount;
        Debug.Log($"{enemy} speed set to {enemy.speedMultiplier * debugBaseSpeed}");
        yield return new WaitForEndOfFrame();
    }
}
