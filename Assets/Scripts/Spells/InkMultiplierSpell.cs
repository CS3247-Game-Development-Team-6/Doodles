using System;
using System.Collections;
using UnityEngine;

public class InkMultiplierSpell : Spell {

    [SerializeField] private float inkMultiplierIncrease;

    public override IEnumerator Activate(SpellUI ui) {
        ChargeCost();
        
        float mult = InkManager.instance.globalInkGainMultiplier * inkMultiplierIncrease;
        InkManager.instance.SetInkGainMultiplier(mult);
        ui.ResetCooldownTimer();

        // The effect will now be activated for {duration} seconds.
        yield return new WaitForSeconds(duration);
        StartCoroutine(Deactivate(ui));
    }

    public override IEnumerator Deactivate(SpellUI ui) {
        float mult = InkManager.instance.globalInkGainMultiplier / inkMultiplierIncrease;
        InkManager.instance.SetInkGainMultiplier(mult);
        yield return new WaitForEndOfFrame();
    }
}
