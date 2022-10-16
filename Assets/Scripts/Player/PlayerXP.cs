using System.Collections.Generic;
using UnityEngine;

public class PlayerXP : MonoBehaviour {
    [SerializeField] private float xpPerChunk; 
    [SerializeField] private List<float> xpToUnlockSpellLevel;

    private float xp;
    public SpellInventoryUI spellInventory;
    public Transform spellOverlay;

    private void Start() {
        xp = 0;
        spellInventory = FindObjectOfType<SpellInventoryUI>();
        spellOverlay = spellInventory.transform.parent;
    }

    public void IncreaseByXPPerChunk() {
        xp += xpPerChunk;
    }

    public void TryUnlockSpell() {
        Debug.Log($"xp {xp} [threshold {xpToUnlockSpellLevel[spellInventory.MaxLevelUnlocked]}]");
        if (xp >= xpToUnlockSpellLevel[spellInventory.MaxLevelUnlocked]) {
            spellOverlay.gameObject.SetActive(true);
            spellInventory.gameObject.SetActive(true);
            spellInventory.UnlockNextLevel();
            Debug.Log($"Unlocking next Spell [level {spellInventory.MaxLevelUnlocked}]");
        }
    }
}
