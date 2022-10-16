using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class DamageMultiplierSpell : Spell {
    [SerializeField] private float defense_reducing;
    private float originalValue;
    [SerializeField] private Image imageEffectTime;
    [SerializeField] private GameObject DefenseReducingPrefab;
    private float effectTimer = 0.0f;
    Dictionary<Enemy, int> originalValues;
    List<GameObject> effect;
    private void Start()
    {
        if (imageEffectTime == null) {
            Debug.LogWarning("TODO: Haven't put in imageEffectTime yet");
            return;
        }
        imageEffectTime.gameObject.SetActive(false);
        imageEffectTime.fillAmount = 0.0f;
    }
    public class Ref<T> where T : struct
    {
        public T Value { get; set; }
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
        effect = new List<GameObject>();
        imageEffectTime.gameObject.SetActive(true);
        effectTimer = duration;
        originalValues = new Dictionary<Enemy, int>();
        ChargeCost();
        Enemy[] allObjects = FindObjectsOfType<Enemy>();
        foreach (Enemy e in allObjects)
        {

            GameObject g = (GameObject)Instantiate(DefenseReducingPrefab, e.transform.position, Quaternion.identity);
            g.transform.parent = e.transform;
            effect.Add(g);
            originalValues.Add(e, e.GetDefense());
            e.ReduceDefense(e.GetDefense());
            
        }

        ui.ResetCooldownTimer();

        // The effect will now be activated for {duration} seconds.
        yield return new WaitForSeconds(duration);
        StartCoroutine(Deactivate(ui));
    }
    public override IEnumerator Deactivate(SpellUI ui)
    {
        imageEffectTime.gameObject.SetActive(false);
        Enemy[] allObjects = FindObjectsOfType<Enemy>();
        foreach (Enemy e in allObjects)
        {
            if (originalValues.ContainsKey(e))
            {
                e.SetDefense(originalValues[e]);
                
                
            }
            
        }
        foreach (GameObject e in effect)
        {
            Destroy(e);

        }


        yield return new WaitForEndOfFrame();
    }

}
