using System.Collections;
using UnityEngine;

public class Spell : MonoBehaviour {

    [SerializeField] public string effectName;

    /*
    public float cost { get; protected set; }
    public float effectTime { get; protected set; }
    public float cooldownTime { get; protected set; }
    */
    public SpellInfo spellInfo;
    public float cost;
    public float effectTime;
    public float cooldownTime;

    public virtual void Init(SpellInfo spellInfo) {
        if (spellInfo == null) {
            Debug.LogError($"SpellInfo not provided for {name}");
            return;
        }
        this.spellInfo = spellInfo;

        cost = spellInfo.cost;
        effectTime = spellInfo.effectTime;
        cooldownTime = spellInfo.cooldownTime;
    }

    protected void ChargeCost() {
        InkManager.instance.ChangeInkAmount(-cost);
    }

    public virtual IEnumerator Activate(SpellUI ui) {
        Debug.Log($"Effect {effectName} run!");
        yield return new WaitForEndOfFrame();
    }
    public virtual IEnumerator Deactivate(SpellUI ui) { 
        Debug.Log($"Effect {effectName} deactivated!");
        yield return new WaitForEndOfFrame();
    }

}
