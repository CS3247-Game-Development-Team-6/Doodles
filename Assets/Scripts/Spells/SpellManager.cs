using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour {
    public static SpellManager instance { get; private set; } = null;
    
    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
    }

    public static IEnumerator HandleEffect(GlobalEffect effect) {
        // but the next line not works, don't know why!
        effect.Activate();
        yield return new WaitForSeconds(effect.duration);
        effect.Deactivate();
    }
    public static void DeactivateEffect(GlobalEffect effect)
    {
        effect.Deactivate();
    }
}
