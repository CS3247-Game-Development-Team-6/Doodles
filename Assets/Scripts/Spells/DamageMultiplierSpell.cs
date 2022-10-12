using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
public class DamageMultiplierSpell : Spell {
    [SerializeField] private float defense_reducing;
    private float originalValue;

    Dictionary<Enemy, int> originalValues;
    public override IEnumerator Activate(SpellUI ui)
    {
        originalValues = new Dictionary<Enemy, int>();
        ChargeCost();
        Enemy[] allObjects = FindObjectsOfType<Enemy>();
        foreach (Enemy e in allObjects)
        {
            originalValues.Add(e, e.GetDefense());
            e.ReduceDefense((int)(originalValues[e]* defense_reducing));
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
                e.SetDefense(originalValues[e]);
            }
            
        }
        yield return new WaitForEndOfFrame();
    }

}
