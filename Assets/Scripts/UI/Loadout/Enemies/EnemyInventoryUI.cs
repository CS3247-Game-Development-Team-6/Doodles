using System.Collections.Generic;
using UnityEngine;

public class EnemyInventoryUI : MonoBehaviour {
    public List<EnemySlotUI> inventoryList { get; private set; }
    public EnemySlotUI currentEnemySelected { get; private set; }

    [Header("UI Components")]
    [SerializeField] private EnemyDescriptionUI enemyDescriptionUI;

    public int Subscribe(EnemySlotUI slot) {
        if (inventoryList == null) inventoryList = new List<EnemySlotUI>();
        int index = inventoryList.Count;
        inventoryList.Add(slot);
        return index;
    }

    public void Hover(EnemyInfo enemyInfo) {
        enemyDescriptionUI.SetInfo(enemyInfo);
    }

    public void Unhover() {
        enemyDescriptionUI.ResetInfo();
    }

    public void Select(EnemySlotUI slot) {
        if (currentEnemySelected != null) 
            currentEnemySelected.background.sprite = currentEnemySelected.unselectedBg;
        currentEnemySelected = slot;
        enemyDescriptionUI.SelectInfo(slot.enemyInfo);
    }
}
