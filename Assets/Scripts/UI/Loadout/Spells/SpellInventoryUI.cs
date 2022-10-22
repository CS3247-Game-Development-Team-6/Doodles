using System.Collections.Generic;
using UnityEngine;

public class SpellInventoryUI : MonoBehaviour {
    public List<SpellSlotUI> inventoryList { get; private set; }
    public SpellSlotUI currentSpellSelected { get; private set; }

    [Header("UI Components")]
    [SerializeField] private SpellDescriptionUI spellDescriptionUI;
    private SpellManager spellManager;

    public int MaxLevelUnlocked { get; private set; } = -1;

    public static readonly int MAX_LEVEL = 3;

    public bool isMaxLevelUnlocked => MaxLevelUnlocked >= MAX_LEVEL;

    private void OnEnable() {
        SpellManager.IsChoosingSpell = true;
    }

    private void OnDisable() {
        SpellManager.IsChoosingSpell = false;
    }

    public int Subscribe(SpellSlotUI slot) {
        if (inventoryList == null) inventoryList = new List<SpellSlotUI>();
        int index = inventoryList.Count;
        inventoryList.Add(slot);
        return index;
    }

    public void AddSelectedToSpellManager() {
        // IF IS IN GAME, add to the SpellManager (Shop) 
        if (spellManager != null) {
            GameObject newSpell = Instantiate(currentSpellSelected.spellUi.gameObject, transform);
            spellManager.Add(newSpell.GetComponent<SpellUI>());
            currentSpellSelected.DisableSelect();
        }
    }

    public void AttachSpellManager(SpellManager spellManager) {
        this.spellManager = spellManager;
        // foreach (Transform child in transform) {
        for (int i = MaxLevelUnlocked + 1; i < transform.childCount; i++) {
            Transform child = transform.GetChild(i);
            CanvasGroup canvas = child.GetComponent<CanvasGroup>();
            if (canvas != null) { 
                LockLevel(canvas);
            }
        }
    }

    public void Hover(SpellInfo spellInfo) {
        spellDescriptionUI.SetInfo(spellInfo);
    }

    public void Unhover() {
        spellDescriptionUI.ResetInfo();
    }

    public void Select(SpellSlotUI slot) {
        if (currentSpellSelected != null) {
            currentSpellSelected.background.sprite = currentSpellSelected.unselectedBg;
        }
        currentSpellSelected = slot;
        currentSpellSelected.background.sprite = currentSpellSelected.selectedBg;
        spellDescriptionUI.SelectInfo(slot.spellInfo);
    }

    private void LockLevel(CanvasGroup canvas) {
        canvas.alpha = 0.5f;
        canvas.blocksRaycasts = false;
    }

    private void UnlockLevel(CanvasGroup canvas) {
        canvas.alpha = 1f;
        canvas.blocksRaycasts = true;
    }

    public void UnlockNextLevel() {
        if (MaxLevelUnlocked > -1) {
            Transform child = transform.GetChild(MaxLevelUnlocked);
            CanvasGroup canvas = child.GetComponent<CanvasGroup>();
            LockLevel(canvas);
        }

        MaxLevelUnlocked++;

        if (MaxLevelUnlocked <= MAX_LEVEL) {
            Transform child = transform.GetChild(MaxLevelUnlocked);
            CanvasGroup canvas = child.GetComponent<CanvasGroup>();
            UnlockLevel(canvas);
        }

    }


}

