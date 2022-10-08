using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
public class ElementBurstSpell : Spell {
    [SerializeField] private float elememtEffectLifeTimeFactor=1.0f;
    [SerializeField] private float elememtEffectAugmentationFactor=1.0f;

    
    public override IEnumerator Activate(SpellUI ui)
    {
       
        ChargeCost();
        SpellManager.instance.ActivateElementBurst(elememtEffectLifeTimeFactor, elememtEffectAugmentationFactor);
        

        ui.ResetCooldownTimer();

        // The effect will now be activated for {duration} seconds.
        yield return new WaitForSeconds(duration);
        StartCoroutine(Deactivate(ui));
    }
    public override IEnumerator Deactivate(SpellUI ui)
    {

        SpellManager.instance.DeActivateElementBurst();
        yield return new WaitForEndOfFrame();
    }

}
