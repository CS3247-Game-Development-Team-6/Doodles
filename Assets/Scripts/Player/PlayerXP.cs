using System.Collections.Generic;
using UnityEngine;

public class PlayerXP : MonoBehaviour {
    [SerializeField] private float xpPerChunk; 
    [SerializeField] private List<float> xpToUnlockSpellLevel;

    private float xp;
    private SpellInventoryUI spellInventory;
    private Transform spellOverlay;

    private void Start() {
        xp = 0;
        spellInventory = FindObjectOfType<SpellInventoryUI>();
        spellOverlay = spellInventory.transform.parent;
    }

    public void IncreaseByXPPerChunk() {
        xp += xpPerChunk;
    }

    public void TryUnlockSpell() {
        if (spellInventory.isMaxLevelUnlocked) return;

        Debug.Log($"xp {xp} [threshold {xpToUnlockSpellLevel[spellInventory.MaxLevelUnlocked]}]");
        float xpThreshold =
            spellInventory.MaxLevelUnlocked < xpToUnlockSpellLevel.Count ?
            xpToUnlockSpellLevel[spellInventory.MaxLevelUnlocked] : xpToUnlockSpellLevel[xpToUnlockSpellLevel.Count - 1];
        
        if (xp >= xpToUnlockSpellLevel[spellInventory.MaxLevelUnlocked]) {
            spellOverlay.gameObject.SetActive(true);
            spellInventory.gameObject.SetActive(true);
            spellInventory.UnlockNextLevel();
            Debug.Log($"Unlocking next Spell [level {spellInventory.MaxLevelUnlocked}]");
        }
    }
}
