using System.Collections.Generic;
using UnityEngine;

public class SkillInventoryUI : MonoBehaviour {
    public List<SkillSlotUI> inventoryList { get; private set; }

    [Header("UI Components")]
    [SerializeField] private SkillDescriptionUI skillDescriptionUI;

    public int Subscribe(SkillSlotUI slot) {
        if (inventoryList == null) inventoryList = new List<SkillSlotUI>();
        int index = inventoryList.Count;
        inventoryList.Add(slot);
        return index;
    }

    public void Hover() {
        
    }

    public void Unhover() {
        
    }

    public void Select() {

    }

}

