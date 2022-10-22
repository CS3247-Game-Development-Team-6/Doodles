using System.Collections.Generic;
using UnityEngine;

public class PlayerXP : MonoBehaviour {
    [SerializeField] private float xpPerChunk; 
    [SerializeField] private List<float> xpToUnlockSpellLevel;

    private float xp;
    private SpellInventoryUI spellInventory;
    private Transform spellOverlay;

    private void Start() {
        xp = -1;
        spellInventory = FindObjectOfType<SpellInventoryUI>();
        spellOverlay = spellInventory.transform.parent;
    }

    public void IncreaseByXPPerChunk() {
        xp += xpPerChunk;
    }

    public void LockSpell() {
        spellOverlay.gameObject.SetActive(false);
        spellInventory.gameObject.SetActive(false);
    }

    public void TryUnlockSpell() {
        if (spellInventory.isMaxLevelUnlocked) return;

        Debug.Log($"xp {xp} [threshold {xpToUnlockSpellLevel[spellInventory.MaxLevelUnlocked+1]}]");
        float xpThreshold =
            spellInventory.MaxLevelUnlocked+1 < xpToUnlockSpellLevel.Count ?
            xpToUnlockSpellLevel[spellInventory.MaxLevelUnlocked+1] : xpToUnlockSpellLevel[xpToUnlockSpellLevel.Count - 1];
        
        if (xp >= xpToUnlockSpellLevel[spellInventory.MaxLevelUnlocked + 1]) {
            spellOverlay.gameObject.SetActive(true);
            spellInventory.gameObject.SetActive(true);
            spellInventory.UnlockNextLevel();
            Debug.Log($"Unlocking next Spell [level {spellInventory.MaxLevelUnlocked}]");
        }
    }
}
