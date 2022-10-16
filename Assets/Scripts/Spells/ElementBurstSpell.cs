using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class ElementBurstSpell : Spell {
    [SerializeField] private float elememtEffectLifeTimeFactor=1.0f;
    [SerializeField] private float elememtEffectAugmentationFactor=1.0f;
    [SerializeField] private Image imageEffectTime;
    private float effectTimer = 0.0f;
    private void Start() {
        if (imageEffectTime == null) {
            Debug.LogWarning("TODO: Haven't put in imageEffectTime yet");
            return;
        }
        imageEffectTime.gameObject.SetActive(false);
        imageEffectTime.fillAmount = 0.0f;
    }

    private void Update()
    {
        if (!Spell.InGame) return;
        if (effectTimer > 0.0f)
        {
            effectTimer -= Time.deltaTime;
            imageEffectTime.fillAmount = effectTimer / duration;
        }
        else
        {
            imageEffectTime.gameObject.SetActive(false);
        }
    }
    public override IEnumerator Activate(SpellUI ui)
    {
        imageEffectTime.gameObject.SetActive(true);
        effectTimer = duration;
        ChargeCost();
        SpellManager.instance.ActivateElementBurst(elememtEffectLifeTimeFactor, elememtEffectAugmentationFactor);
        

        ui.ResetCooldownTimer();

        // The effect will now be activated for {duration} seconds.
        yield return new WaitForSeconds(duration);
        StartCoroutine(Deactivate(ui));
    }
    public override IEnumerator Deactivate(SpellUI ui)
    {
        imageEffectTime.gameObject.SetActive(false);
        SpellManager.instance.DeActivateElementBurst();
        yield return new WaitForEndOfFrame();
    }

}
