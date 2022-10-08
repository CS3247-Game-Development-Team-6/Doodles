using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
public class SuperDoodleSpell : Spell {
    [SerializeField] private int doodleAttackIncrease;
    [SerializeField] private int healthRecover;
    private Transform player;
    private PlayerMeleeHitbox melee;
    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;

      
    }
    public override IEnumerator Activate(SpellUI ui)
    {
       
        ChargeCost();
        ui.ResetCooldownTimer();
        SpellManager.instance.doodleDamageIncreasing = doodleAttackIncrease;
        float effectTimer = 0.0f;
        PlayerHealth health = player.GetComponent<PlayerHealth>();
        while (effectTimer<=duration)
        {
            health.Heal(healthRecover);
            yield return new WaitForSeconds(1);
            effectTimer += 1;
        }
       

       

        // The effect will now be activated for {duration} seconds.
       
        StartCoroutine(Deactivate(ui));
    }
    public override IEnumerator Deactivate(SpellUI ui)
    {
        SpellManager.instance.doodleDamageIncreasing = 0;
        yield return new WaitForEndOfFrame();
    }

}
