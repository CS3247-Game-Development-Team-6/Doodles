using System.Collections;
using UnityEngine;

public class StopTowerShootingEffect : ITowerEffect {

    // values that can be set
    public float lifeTime = 5.0f; // lifetime of the effect
    // calls the coroutine Deactivate after yielding for ^ seconds

    public string GetKey() {
        return "StopTowerShooting";
    }

    public float GetLifetime() {
        return lifeTime;
    }

    public IEnumerator Activate(Tower tower) {
        tower.enabled = false;
        yield return new WaitForSeconds(lifeTime);
    }

    public IEnumerator Deactivate(Tower tower) {
        tower.enabled = true;
        yield return new WaitForEndOfFrame();
    }
}
