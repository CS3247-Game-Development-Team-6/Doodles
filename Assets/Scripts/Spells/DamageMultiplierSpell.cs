using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
public class DamageMultiplierSpell : Spell {
    [SerializeField] private float damageMultiplierIncrease;
    private float originalValue;

    Dictionary<Enemy, float> originalValues;
    public override IEnumerator Activate(SpellUI ui)
    {
        originalValues = new Dictionary<Enemy, float>();
        ChargeCost();
        Enemy[] allObjects = FindObjectsOfType<Enemy>();
        foreach (Enemy e in allObjects)
        {
            originalValues.Add(e, e.GetDamamgeMultiplier());
            e.SetDamamgeMultiplier(damageMultiplierIncrease);
        }

        ui.ResetCooldownTimer();

        // The effect will now be activated for {duration} seconds.
        yield return new WaitForSeconds(duration);
        StartCoroutine(Deactivate(ui));
    }
    public override IEnumerator Deactivate(SpellUI ui)
    {
        Enemy[] allObjects = FindObjectsOfType<Enemy>();
        foreach (Enemy e in allObjects)
        {
            if (originalValues.ContainsKey(e))
            {
                e.SetDamamgeMultiplier(originalValues[e]);
            }
            
        }
        yield return new WaitForEndOfFrame();
    }

}
