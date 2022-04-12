using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public ShopItemUI[] items;
    public PlayerMovement playerMovement;
    public ParticleSystem invalidActionEffect;
    private BuildManager buildManager;

    private void Start()
    {
        buildManager = BuildManager.instance;
    }

    public void SetTurretToBuild() 
    {
        buildManager.SetTowerToBuild(buildManager.standardTowerPrefab);
    }

    public void SetMissileLauncher()
    {
        buildManager.SetTowerToBuild(buildManager.missileLauncherPrefab);
    }


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
}
