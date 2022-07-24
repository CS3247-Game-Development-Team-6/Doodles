using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
    public PlayerMovement playerMovement;
    public ParticleSystem invalidActionEffect;
    [SerializeField] private ShopTowerUI selectedTower;
    // Deprecating: Use towerManager for new building system.
    private TowerManager towerManager;

    private void Start() {
        towerManager = TowerManager.instance;
        selectedTower = GetComponentInChildren<ShopTowerUI>();
        if (selectedTower) {
            SetTowerToBuild(selectedTower);
        }
    }

    public void SetTowerToBuild(ShopTowerUI item) {
        item.gameObject.GetComponent<Image>().sprite = item.selected;
        if (selectedTower != null && selectedTower != item) { 
            selectedTower.gameObject.GetComponent<Image>().sprite = selectedTower.unselected;
        }

        selectedTower = item;
        TowerInfo towerInfo = item.towerInfo;
        towerManager.SetTowerToBuild(towerInfo);
    }
}
