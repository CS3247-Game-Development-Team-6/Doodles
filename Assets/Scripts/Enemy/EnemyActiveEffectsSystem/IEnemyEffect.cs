using System.Collections;

/**
 * Interface that all EnemyEffects must implement.
 * Activate and Deactivate should NOT call each other unless intended,
 * the deactivation after lifetime of effect is handled by EnemyActiveEffects.
 */
public interface IEnemyEffect {
    public string GetKey();
    public float GetLifetime();
    public IEnumerator Activate(Enemy enemy);
    public IEnumerator Deactivate(Enemy enemy);
}

