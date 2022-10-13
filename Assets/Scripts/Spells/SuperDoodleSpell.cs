using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class SuperDoodleSpell : Spell {
    [SerializeField] private int doodleAttackIncrease;
    [SerializeField] private int healthRecover;
    [SerializeField] private Image imageEffectTime;
    [SerializeField] private GameObject DoodleBuffPrefab;
    private Transform player;
    private float effectTimer = 0.0f;
    private GameObject doodleBuff;
    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        imageEffectTime.gameObject.SetActive(false);
        imageEffectTime.fillAmount = 0.0f;

    }
    private void Update()
    {
        if (effectTimer > 0.0f)
        {
            effectTimer -= Time.deltaTime;
            imageEffectTime.fillAmount = effectTimer / duration;
            doodleBuff.transform.position = player.position;
        }
        else
        {
            Destroy(doodleBuff);
            imageEffectTime.gameObject.SetActive(false);
        }
    }
    public override IEnumerator Activate(SpellUI ui)
    {
        doodleBuff = Instantiate(DoodleBuffPrefab, player.position, Quaternion.identity);
        imageEffectTime.gameObject.SetActive(true);
        effectTimer = duration;
        ChargeCost();
        ui.ResetCooldownTimer();
        SpellManager.instance.doodleDamageIncreasing = doodleAttackIncrease;
        float effectTimer2 =duration;
        PlayerHealth health = player.GetComponent<PlayerHealth>();
        while (effectTimer2>=0.0f)
        {
            health.Heal(healthRecover);
            effectTimer2 -= 1;
            yield return new WaitForSeconds(1);
          
        }
       

       

        // The effect will now be activated for {duration} seconds.
       
        StartCoroutine(Deactivate(ui));
    }
    public override IEnumerator Deactivate(SpellUI ui)
    {
        
        SpellManager.instance.doodleDamageIncreasing = 0;
        imageEffectTime.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();
    }

}
