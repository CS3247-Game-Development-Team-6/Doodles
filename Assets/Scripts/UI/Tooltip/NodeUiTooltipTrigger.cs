using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NodeUiTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{    
    [SerializeField] private string header;
    
    [Multiline]
    [SerializeField] private string content;

    [Multiline]
    [SerializeField] private string upgradeInfo = "Deals DoT to enemies for 5 seconds";

    private enum ButtonType {
        Upgrade, 
        Destroy, 
        ElementFire,
        ElementIce,
        ElementWater
    };
    [SerializeField] private ButtonType buttonType = ButtonType.Upgrade;  // should appear as a drop down

    private static Node currentNode; // can check upgrade and element status
    private static BuildManager buildManager = BuildManager.instance;
    private static float upgradeCost;
    private static float elementSwapCost;
    private static float currentDamage;
    private static float currentRange;
    private static float currentExplosionRadius;
    private static float upgradeDamage;
    private static float upgradeRange;
    private static float upgradeExplosionRadius;

    public void SetNode(Node node) {
        // Debug.Log("SET NODE!!");
        currentNode = node;
        buildManager = BuildManager.instance;

        // retrieve costs
        upgradeCost = currentNode.towerObj.GetComponent<Turret>().GetUpgradeCost();
        elementSwapCost = currentNode.towerObj.GetComponent<Turret>().GetSwapElementCost();

        // retrieve current stats
        currentDamage = currentNode.towerObj.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetBulletDamage();
        currentExplosionRadius = currentNode.towerObj.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetExplosionRadius();
        currentRange = currentNode.towerObj.GetComponent<Turret>().GetRange();

    }

    public void OnPointerEnter(PointerEventData eventData) {
        // Debug.Log("button type: " + buttonType);
        if (buttonType == ButtonType.Upgrade && currentNode.GetIsUpgraded()) {
            // upgrade button, tower has upgrade
            TooltipSystem.Show("Tower is already upgraded!", header);
            return;
        }
        if ((buttonType == ButtonType.ElementWater
            || buttonType == ButtonType.ElementIce
            || buttonType == ButtonType.ElementFire) 
            && currentNode.GetIsAddedElement()) {
            // element button, tower has element
            TooltipSystem.Show("Tower already has an element!", header);
            return;
        }
        if (buttonType == ButtonType.Destroy) {
            // upgrade button, tower has upgrade
            TooltipSystem.Show(content + "\n" + upgradeInfo, header);
            return;
        }
        
        ////////////////////////////
        // retrieve upgrade stats
        ////////////////////////////
        string towerElement = currentNode.towerObj.GetComponent<Turret>().bulletPrefab.tag;
        if (currentNode.towerObj.tag == "Turret" && currentNode.GetIsUpgraded()) {
            if (buttonType == ButtonType.ElementFire) {
                upgradeDamage = buildManager.upFireTurret.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetBulletDamage();
                upgradeExplosionRadius = buildManager.upFireTurret.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetExplosionRadius();;
                upgradeRange = buildManager.upFireTurret.GetComponent<Turret>().GetRange();
            } else if (buttonType == ButtonType.ElementIce) {
                upgradeDamage = buildManager.upIceTurret.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetBulletDamage();
                upgradeExplosionRadius = buildManager.upIceTurret.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetExplosionRadius();;
                upgradeRange = buildManager.upIceTurret.GetComponent<Turret>().GetRange();
            } else if (buttonType == ButtonType.ElementWater) {
                upgradeDamage = buildManager.upWaterTurret.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetBulletDamage();
                upgradeExplosionRadius = buildManager.upWaterTurret.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetExplosionRadius();;
                upgradeRange = buildManager.upWaterTurret.GetComponent<Turret>().GetRange();
            } else {
                // is upgrade or destroy button 
                upgradeDamage = currentDamage;
                upgradeExplosionRadius = currentExplosionRadius;
                upgradeRange = currentRange;
            }
        } else if (currentNode.towerObj.tag == "Turret" && !currentNode.GetIsUpgraded()) {
            if (buttonType == ButtonType.ElementFire) {
                upgradeDamage = buildManager.fireTurret.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetBulletDamage();
                upgradeExplosionRadius = buildManager.fireTurret.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetExplosionRadius();;
                upgradeRange = buildManager.fireTurret.GetComponent<Turret>().GetRange();
            } else if (buttonType == ButtonType.ElementIce) {
                upgradeDamage = buildManager.iceTurret.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetBulletDamage();
                upgradeExplosionRadius = buildManager.iceTurret.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetExplosionRadius();;
                upgradeRange = buildManager.iceTurret.GetComponent<Turret>().GetRange();
            } else if (buttonType == ButtonType.ElementWater) {
                upgradeDamage = buildManager.waterTurret.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetBulletDamage();
                upgradeExplosionRadius = buildManager.waterTurret.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetExplosionRadius();;
                upgradeRange = buildManager.waterTurret.GetComponent<Turret>().GetRange();
            } else if (buttonType == ButtonType.Upgrade) {
                if (towerElement == "Basic") {
                    upgradeDamage = buildManager.upTowerPrefab.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetBulletDamage();
                    upgradeExplosionRadius = buildManager.upTowerPrefab.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetExplosionRadius();;
                    upgradeRange = buildManager.upTowerPrefab.GetComponent<Turret>().GetRange();
                } else if (towerElement == "Fire") {
                    upgradeDamage = buildManager.upFireTurret.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetBulletDamage();
                    upgradeExplosionRadius = buildManager.upFireTurret.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetExplosionRadius();;
                    upgradeRange = buildManager.upFireTurret.GetComponent<Turret>().GetRange();
                } else if (towerElement == "Ice") {
                    upgradeDamage = buildManager.upIceTurret.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetBulletDamage();
                    upgradeExplosionRadius = buildManager.upIceTurret.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetExplosionRadius();;
                    upgradeRange = buildManager.upIceTurret.GetComponent<Turret>().GetRange();
                } else if (towerElement == "Water") {
                    upgradeDamage = buildManager.upWaterTurret.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetBulletDamage();
                    upgradeExplosionRadius = buildManager.upWaterTurret.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetExplosionRadius();;
                    upgradeRange = buildManager.upWaterTurret.GetComponent<Turret>().GetRange();
                } else {
                    // SHOULD NOT REACH HERE
                    Debug.LogWarning("NodeUiTooltipTrigger error, unaccountable tower element.");
                    upgradeDamage = currentDamage;
                    upgradeExplosionRadius = currentExplosionRadius;
                    upgradeRange = currentRange;
                }
            } else {
                // is destroy button 
                upgradeDamage = currentDamage;
                upgradeExplosionRadius = currentExplosionRadius;
                upgradeRange = currentRange;
            }
        } else if (currentNode.towerObj.tag == "Missile" && currentNode.GetIsUpgraded()) {
            if (buttonType == ButtonType.ElementFire) {
                upgradeDamage = buildManager.upFireMissileLauncher.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetBulletDamage();
                upgradeExplosionRadius = buildManager.upFireMissileLauncher.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetExplosionRadius();;
                upgradeRange = buildManager.upFireMissileLauncher.GetComponent<Turret>().GetRange();
            } else if (buttonType == ButtonType.ElementIce) {
                upgradeDamage = buildManager.upIceMissileLauncher.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetBulletDamage();
                upgradeExplosionRadius = buildManager.upIceMissileLauncher.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetExplosionRadius();;
                upgradeRange = buildManager.upIceMissileLauncher.GetComponent<Turret>().GetRange();
            } else if (buttonType == ButtonType.ElementWater) {
                upgradeDamage = buildManager.upWaterMissileLauncher.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetBulletDamage();
                upgradeExplosionRadius = buildManager.upWaterMissileLauncher.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetExplosionRadius();;
                upgradeRange = buildManager.upWaterMissileLauncher.GetComponent<Turret>().GetRange();
            } else {
                // is upgrade or destroy button 
                upgradeDamage = currentDamage;
                upgradeExplosionRadius = currentExplosionRadius;
                upgradeRange = currentRange;
            }
        } else if (currentNode.towerObj.tag == "Missile" && !currentNode.GetIsUpgraded()) {
            if (buttonType == ButtonType.ElementFire) {
                upgradeDamage = buildManager.fireMissileLauncher.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetBulletDamage();
                upgradeExplosionRadius = buildManager.fireMissileLauncher.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetExplosionRadius();;
                upgradeRange = buildManager.fireMissileLauncher.GetComponent<Turret>().GetRange();
            } else if (buttonType == ButtonType.ElementIce) {
                upgradeDamage = buildManager.iceMissileLauncher.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetBulletDamage();
                upgradeExplosionRadius = buildManager.iceMissileLauncher.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetExplosionRadius();;
                upgradeRange = buildManager.iceMissileLauncher.GetComponent<Turret>().GetRange();
            } else if (buttonType == ButtonType.ElementWater) {
                upgradeDamage = buildManager.waterMissileLauncher.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetBulletDamage();
                upgradeExplosionRadius = buildManager.waterMissileLauncher.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetExplosionRadius();;
                upgradeRange = buildManager.waterMissileLauncher.GetComponent<Turret>().GetRange();
            } else if (buttonType == ButtonType.Upgrade) {
                if (towerElement == "Basic") {
                    upgradeDamage = buildManager.upMissileLauncherPrefab.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetBulletDamage();
                    upgradeExplosionRadius = buildManager.upMissileLauncherPrefab.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetExplosionRadius();;
                    upgradeRange = buildManager.upMissileLauncherPrefab.GetComponent<Turret>().GetRange();
                } else if (towerElement == "Fire") {
                    upgradeDamage = buildManager.upFireMissileLauncher.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetBulletDamage();
                    upgradeExplosionRadius = buildManager.upFireMissileLauncher.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetExplosionRadius();;
                    upgradeRange = buildManager.upFireMissileLauncher.GetComponent<Turret>().GetRange();
                } else if (towerElement == "Ice") {
                    upgradeDamage = buildManager.upIceMissileLauncher.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetBulletDamage();
                    upgradeExplosionRadius = buildManager.upIceMissileLauncher.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetExplosionRadius();;
                    upgradeRange = buildManager.upIceMissileLauncher.GetComponent<Turret>().GetRange();
                } else if (towerElement == "Water") {
                    upgradeDamage = buildManager.upWaterMissileLauncher.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetBulletDamage();
                    upgradeExplosionRadius = buildManager.upWaterMissileLauncher.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetExplosionRadius();;
                    upgradeRange = buildManager.upWaterMissileLauncher.GetComponent<Turret>().GetRange();
                } else {
                    // SHOULD NOT REACH HERE
                    Debug.LogWarning("NodeUiTooltipTrigger error, unaccountable tower element.");
                    upgradeDamage = currentDamage;
                    upgradeExplosionRadius = currentExplosionRadius;
                    upgradeRange = currentRange;
                }
            } else {
                // is destroy button 
                upgradeDamage = currentDamage;
                upgradeExplosionRadius = currentExplosionRadius;
                upgradeRange = currentRange;
            }
        } else if (currentNode.towerObj.tag == "AOE" && currentNode.GetIsUpgraded()) {
            if (buttonType == ButtonType.ElementFire) {
                upgradeDamage = buildManager.upFireAoeTower.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetBulletDamage();
                upgradeExplosionRadius = buildManager.upFireAoeTower.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetExplosionRadius();;
                upgradeRange = buildManager.upFireAoeTower.GetComponent<Turret>().GetRange();
            } else if (buttonType == ButtonType.ElementIce) {
                upgradeDamage = buildManager.upIceAoeTower.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetBulletDamage();
                upgradeExplosionRadius = buildManager.upIceAoeTower.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetExplosionRadius();;
                upgradeRange = buildManager.upIceAoeTower.GetComponent<Turret>().GetRange();
            } else if (buttonType == ButtonType.ElementWater) {
                upgradeDamage = buildManager.upWaterAoeTower.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetBulletDamage();
                upgradeExplosionRadius = buildManager.upWaterAoeTower.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetExplosionRadius();;
                upgradeRange = buildManager.upWaterAoeTower.GetComponent<Turret>().GetRange();
            } else {
                // is upgrade or destroy button 
                upgradeDamage = currentDamage;
                upgradeExplosionRadius = currentExplosionRadius;
                upgradeRange = currentRange;
            }
        } else if (currentNode.towerObj.tag == "AOE" && !currentNode.GetIsUpgraded()) {
            if (buttonType == ButtonType.ElementFire) {
                upgradeDamage = buildManager.fireAoeTower.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetBulletDamage();
                upgradeExplosionRadius = buildManager.fireAoeTower.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetExplosionRadius();;
                upgradeRange = buildManager.fireAoeTower.GetComponent<Turret>().GetRange();
            } else if (buttonType == ButtonType.ElementIce) {
                upgradeDamage = buildManager.iceAoeTower.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetBulletDamage();
                upgradeExplosionRadius = buildManager.iceAoeTower.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetExplosionRadius();;
                upgradeRange = buildManager.iceAoeTower.GetComponent<Turret>().GetRange();
            } else if (buttonType == ButtonType.ElementWater) {
                upgradeDamage = buildManager.waterAoeTower.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetBulletDamage();
                upgradeExplosionRadius = buildManager.waterAoeTower.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetExplosionRadius();;
                upgradeRange = buildManager.waterAoeTower.GetComponent<Turret>().GetRange();
            } else if (buttonType == ButtonType.Upgrade) {
                if (towerElement == "Basic") {
                    upgradeDamage = buildManager.upAoeTower.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetBulletDamage();
                    upgradeExplosionRadius = buildManager.upAoeTower.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetExplosionRadius();;
                    upgradeRange = buildManager.upAoeTower.GetComponent<Turret>().GetRange();
                } else if (towerElement == "Fire") {
                    upgradeDamage = buildManager.upFireAoeTower.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetBulletDamage();
                    upgradeExplosionRadius = buildManager.upFireAoeTower.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetExplosionRadius();;
                    upgradeRange = buildManager.upFireAoeTower.GetComponent<Turret>().GetRange();
                } else if (towerElement == "Ice") {
                    upgradeDamage = buildManager.upIceAoeTower.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetBulletDamage();
                    upgradeExplosionRadius = buildManager.upIceAoeTower.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetExplosionRadius();;
                    upgradeRange = buildManager.upIceAoeTower.GetComponent<Turret>().GetRange();
                } else if (towerElement == "Water") {
                    upgradeDamage = buildManager.upWaterAoeTower.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetBulletDamage();
                    upgradeExplosionRadius = buildManager.upWaterAoeTower.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().GetExplosionRadius();;
                    upgradeRange = buildManager.upWaterAoeTower.GetComponent<Turret>().GetRange();
                } else {
                    // SHOULD NOT REACH HERE
                    Debug.LogWarning("NodeUiTooltipTrigger error, unaccountable tower element.");
                    upgradeDamage = currentDamage;
                    upgradeExplosionRadius = currentExplosionRadius;
                    upgradeRange = currentRange;
                }
            } else {
                // is destroy button 
                upgradeDamage = currentDamage;
                upgradeExplosionRadius = currentExplosionRadius;
                upgradeRange = currentRange;
            }
        } else {
            // SHOULD NOT REACH HERE
            Debug.LogWarning("NodeUiTooltipTrigger SetNode() error!");
            upgradeDamage = currentDamage;
            upgradeExplosionRadius = currentExplosionRadius;
            upgradeRange = currentRange;
        }

        /*
        Debug.Log("tower tag: " + currentNode.tower.tag);
        Debug.Log("tower upgraded: " + currentNode.GetIsUpgraded());
        Debug.Log("current damage: " + currentDamage);
        Debug.Log("upgraded damage: " + upgradeDamage);
        Debug.Log("current range: " + currentRange);
        Debug.Log("upgraded range: " + upgradeRange);
        Debug.Log("current explosion rad: " + currentExplosionRadius);
        Debug.Log("upgraded explosion rad: " + upgradeExplosionRadius);
        */

        /////////////////////////////////////
        // generate full content and headers
        /////////////////////////////////////
        string fullContent = "";
        string fullHeader = "";
        string damageChange = "";
        string rangeChange = "";
        string explosionRadiusChange = "";
        
        if (upgradeDamage - currentDamage >= 0) {
            damageChange = "+" + (upgradeDamage - currentDamage);
        } else {
            damageChange = "" + (upgradeDamage - currentDamage);
        }
        if (upgradeRange - currentRange >= 0) {
            rangeChange = "+" + (upgradeRange - currentRange);
        } else {
            rangeChange = "" + (upgradeRange - currentRange);
        }
        if (upgradeExplosionRadius - currentExplosionRadius >= 0) {
            explosionRadiusChange = "+" + (upgradeExplosionRadius - currentExplosionRadius);
        } else {
            explosionRadiusChange = "" + (upgradeExplosionRadius - currentExplosionRadius);
        }

        fullContent += content + "\n";
        fullContent += "DMG: " + upgradeDamage + " [" + damageChange + "]" + "\n";
        fullContent += "Range: " + upgradeRange + " [" + rangeChange + "]" + "\n";
        fullContent += "Explosion Radius: " + upgradeExplosionRadius + " [" + explosionRadiusChange + "]" + "\n";
        fullContent += upgradeInfo;

        if (buttonType == ButtonType.Upgrade) {
            fullHeader += header + " [$" + upgradeCost + "]";
        } else if (buttonType == ButtonType.ElementIce 
            || buttonType == ButtonType.ElementFire 
            || buttonType == ButtonType.ElementWater) {
            fullHeader += header + " [$" + elementSwapCost + "]";
        } else {
            fullHeader += header + " [$" + 0 + "]";
        }
        

        TooltipSystem.Show(fullContent, fullHeader);
    }

    public void OnPointerExit(PointerEventData eventData) {
        TooltipSystem.Hide();
    }
    
    public void OnPointerDown(PointerEventData eventData) {
        // in-case we click the button, we can hide the tooltip again. For example if we click a button.
        TooltipSystem.Hide();
    }
}

