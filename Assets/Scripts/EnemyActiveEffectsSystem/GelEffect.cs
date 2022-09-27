using System.Collections;
using UnityEngine;

public class GelEffect : Effect {

    // values that can be set
    public float gelLifetime = 2.0f; // lifetime of the effect
    // calls the coroutine Deactivate after yielding for ^ seconds
    public float gelSlowAmount = 0.7f; // value 0 to 1 applied to speed of enemy
    
    private Enemy enemy;
    private float enemyBaseSpeed;
    public bool isActivated { get; private set; } = false;

    public IEnumerator Activate(Enemy enemy) {
        this.enemy = enemy;
        this.enemyBaseSpeed = enemy.getSpeed();
        float newSpeed = enemyBaseSpeed * gelSlowAmount;
        enemy.setSpeed(newSpeed);
        this.isActivated = true;
        
        Debug.Log($"{enemy} speed set to {newSpeed}");
        
        yield return new WaitForSeconds(gelLifetime);
        Deactivate(enemy);
    }

    public void RecalculateSpeed() {
        if (!isActivated) {
            // not activated, do nothing as variables not instantiated
            Debug.LogError($"Attempted to recalculate gel speed for {enemy} without activating effect!");
            return;
        }

        this.enemyBaseSpeed = enemy.getSpeed();
        float newSpeed = enemyBaseSpeed * gelSlowAmount;
        enemy.setSpeed(newSpeed);
        Debug.Log($"{enemy} speed recalculated and set to {newSpeed}");
    }

    public IEnumerator Deactivate(Enemy enemy) {
        if (!isActivated) {
            // not activated, do nothing as variables not instantiated
            Debug.LogError($"Attempted to deactivate gel for {enemy} without activating effect!");
        } else {
            Debug.Log($"{enemy} reverted to {enemyBaseSpeed}");
            
            isActivated = false;
            enemy.setSpeed(enemyBaseSpeed);
            yield return new WaitForEndOfFrame();
        }
    }
}
