using System.Collections;
using UnityEngine;

public class SpellHandler : MonoBehaviour {
    public static SpellHandler instance { get; private set; }

    private SpellHandler() { }

    private void Start() {
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;
    }

    public  IEnumerator HandleEffect(Spell effect) {
        effect.Activate();
        yield return new WaitForSeconds(effect.duration);
        effect.Deactivate();
    }
    public void DeactivateEffect(Spell effect)
    {
        effect.Deactivate();
    }
}
