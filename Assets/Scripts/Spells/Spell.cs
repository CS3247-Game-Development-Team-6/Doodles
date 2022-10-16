using System.Collections;
using UnityEngine;

public class Spell : MonoBehaviour {

    [SerializeField] public string effectName;

    [SerializeField, Range(1, 1000)] public float cost;
    [SerializeField, Range(1, 100)] public float effectTime;
    [SerializeField, Range(1, 100)] public float cooldownTime;


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
