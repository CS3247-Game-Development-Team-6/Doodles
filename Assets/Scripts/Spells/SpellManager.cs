using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public static SpellManager instance = null;
    public int doodleDamageIncreasing=0;
    private float elementEffectLifeTimeFactor = 1.0f;
    private float elementEffectAugmentationFactor = 1.0f;
    public bool isCasting = false;

    // public List<SpellUI> emptySlots { get; private set; }
    public List<SpellUI> slots { get; private set; }
    public bool isGameplay;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if(instance!=this) {
            Debug.LogError($"More than one SpellManager found in scene, destroying {name}");
            Destroy(gameObject);
        }
    }

    private void Start() {
        slots = new List<SpellUI>();
        // emptySlots = new List<SpellUI>();
        for (int i = 0; i < transform.childCount; i++) {
            Transform child = transform.GetChild(i);
            SpellUI slot = child.GetComponent<SpellUI>();
            if (slot == null) continue;
            slot.enabled = true;
            slot.Level = i;
            if (slot.spell != null) slots.Add(slot);
        }
        SpellInventoryUI spellInventory = FindObjectOfType<SpellInventoryUI>();
        spellInventory.AttachSpellManager(this);
    }

    public void Add(SpellUI slot) {
        if (!isGameplay) {
            Debug.Log($"Not in gameplay, spellManager not adding {slot}");
            return;
        }
        slot.enabled = true;
        slot.transform.SetParent(transform);
        slots.Add(slot);
    }
    public float GetElementEffectLifetimeFactor()
    {
        return elementEffectLifeTimeFactor;
    }

    public float GetElementAugmentationFactor()
    {
        return elementEffectAugmentationFactor;
    }
    public void ActivateElementBurst(float lifeTimeFactor,float augmentationFactor)
    {
        elementEffectLifeTimeFactor = lifeTimeFactor;
        elementEffectAugmentationFactor = augmentationFactor;
    }

    public void DeActivateElementBurst()
    {
        elementEffectLifeTimeFactor = 1.0f;
        elementEffectAugmentationFactor = 1.0f;
    }


}
