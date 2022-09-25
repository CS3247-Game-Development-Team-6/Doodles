using System.Collections;
using UnityEngine;

public class GelEffect : Effect {

    // values that can be set
    public float gelLifetime = 2.0f; // lifetime of the effect
    // calls the coroutine Deactivate after yielding for ^ seconds
    public float gelSlowAmount = 0.1f; // value 0 to 1 applied to speed of enemy
    
    private float enemyBaseSpeed;
    public IEnumerator Activate(Enemy enemy) {
        enemyBaseSpeed = enemy.getSpeed();
        float newSpeed = enemyBaseSpeed * gelSlowAmount;
        enemy.setSpeed(newSpeed);
        Debug.Log($"{enemy} speed set to {newSpeed}");
        yield return new WaitForSeconds(gelLifetime);
        Deactivate(enemy);
    }

    public IEnumerator Deactivate(Enemy enemy) {
        Debug.Log($"{enemy} revert to {enemyBaseSpeed}");
        enemy.setSpeed(enemyBaseSpeed);
        yield return new WaitForEndOfFrame();
    }
}
