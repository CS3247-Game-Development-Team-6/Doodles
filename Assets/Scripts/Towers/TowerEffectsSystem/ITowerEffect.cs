using System.Collections;

/**
 * Interface that all TowerEffects must implement.
 * Activate and Deactivate should NOT call each other unless intended,
 * the deactivation after lifetime of effect is handled by TowerEffects.
 */
public interface ITowerEffect {
    public string GetKey();
    public float GetLifetime();
    public IEnumerator Activate(Tower tower);
    public IEnumerator Deactivate(Tower tower);
}

