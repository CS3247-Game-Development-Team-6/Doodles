using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class InkMultiplierSpell : Spell {
    [SerializeField] private Image imageEffectTime;
    private float effectTimer = 0.0f;
    [SerializeField] private float inkMultiplierIncrease;

    private void Start()
    {
        imageEffectTime.gameObject.SetActive(false);
        imageEffectTime.fillAmount = 0.0f;
    }

    private void Update()
    {
        if (!Spell.InGame) return;
        if (effectTimer>0.0f)
        {
            effectTimer -= Time.deltaTime;
            imageEffectTime.fillAmount = effectTimer / duration;
        }
        else
        {
            imageEffectTime.gameObject.SetActive(false);
        }
    }
    public override IEnumerator Activate(SpellUI ui) {
        imageEffectTime.gameObject.SetActive(true);
        effectTimer = duration;
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
        imageEffectTime.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();
    }
}
