using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public ShopItemUI[] items;
    public PlayerMovement playerMovement;
    public ParticleSystem invalidActionEffect;
    // Deprecating: Use towerManager for new building system.
    private BuildManager buildManager;
    private TowerManager towerManager;

    private void Start()
    {
        buildManager = BuildManager.instance;
    }

    // Deprecating: Use SetTowerToBuild for new building system.
    public void SetTowerAttempt(ShopItemUI item) {
        if (playerMovement != null && playerMovement.GetIsBuilding()) {
            Instantiate(invalidActionEffect, playerMovement.transform.position, Quaternion.identity);
            return;
        }
        if (buildManager != null) {
            buildManager.SetTowerToBuild(item.tower);
            item.gameObject.GetComponent<Image>().sprite = item.selected;
            foreach (ShopItemUI otherItem in items) {
                if (item == otherItem) continue; 
                otherItem.gameObject.GetComponent<Image>().sprite = otherItem.unselected;
            }
        }

    }

    public void SetTowerToBuild(ShopTowerUI item) {
        towerManager.SetTowerToBuild(item.towerInfo);

    }
}
