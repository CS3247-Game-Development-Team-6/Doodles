using UnityEngine;
public class InkMultiplierSpell : Spell {

    [SerializeField] private float inkMultiplierIncrease;

    public override void Activate() {
        Debug.Log($"Ink spell activate");
        float mult = InkManager.instance.globalInkGainMultiplier * inkMultiplierIncrease;
        InkManager.instance.SetInkGainMultiplier(mult);
    }


    public override void Deactivate() {
        Debug.Log($"Ink spell deactivate");
        float mult = InkManager.instance.globalInkGainMultiplier / inkMultiplierIncrease;
        InkManager.instance.SetInkGainMultiplier(mult);
    }
}