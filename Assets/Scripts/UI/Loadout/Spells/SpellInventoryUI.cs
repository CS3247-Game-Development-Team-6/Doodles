using System.Collections.Generic;
using UnityEngine;

public class SpellInventoryUI : MonoBehaviour {
    public List<SpellSlotUI> inventoryList { get; private set; }
    public SpellSlotUI currentSpellSelected { get; private set; }

    [Header("UI Components")]
    [SerializeField] private SpellDescriptionUI skillDescriptionUI;

    public int Subscribe(SpellSlotUI slot) {
        if (inventoryList == null) inventoryList = new List<SpellSlotUI>();
        int index = inventoryList.Count;
        inventoryList.Add(slot);
        return index;
    }

    public void Hover(SpellInfo spellInfo) {
        skillDescriptionUI.SetInfo(spellInfo);
    }

    public void Unhover() {
        skillDescriptionUI.ResetInfo();
    }

    public void Select(SpellSlotUI slot) {
        if (currentSpellSelected != null) {
            currentSpellSelected.background.sprite = currentSpellSelected.unselectedBg;
        }
        currentSpellSelected = slot;
        skillDescriptionUI.SelectInfo(slot.spellInfo);
        // IF IS IN GAME, add to the SpellManager (Shop) 

    }

}

