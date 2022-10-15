using System.Collections.Generic;
using UnityEngine;

public class EnemyInventoryUI : MonoBehaviour {
    public List<EnemySlotUI> inventoryList { get; private set; }
    public HashSet<EnemyInfo> uniqueEnemies { get; private set; }
    public EnemySlotUI currentEnemySelected { get; private set; }
    public GameObject prefabEnemySlot;

    [Header("UI Components")]
    [SerializeField] private EnemyDescriptionUI enemyDescriptionUI;

    public void LoadEnemies(MapInfo mapInfo) {
        if (inventoryList == null) inventoryList = new List<EnemySlotUI>();
        uniqueEnemies = new HashSet<EnemyInfo>();
        foreach (var chunk in mapInfo.chunkInfo) {
            foreach (var wave in chunk.waves) {
                uniqueEnemies.Add(wave.enemy.GetComponent<Enemy>().enemyInfo);
            }
        }
        foreach (var slot in inventoryList) {
            Debug.Log($"slot {slot.enemyInfo.name} ");
            Destroy(slot.gameObject);
        }
        inventoryList.Clear();
        foreach (var enemy in uniqueEnemies) {
            EnemySlotUI slot = Instantiate(prefabEnemySlot, transform).GetComponent<EnemySlotUI>();
            slot.enemyInfo = enemy;
        }
    }

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
