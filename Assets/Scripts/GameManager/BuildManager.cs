using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    private GameObject towerToBuild;
    public GameObject playerGO;
    private Node selectedNode;
    public NodeUI nodeUI;
    public GameObject standardTowerPrefab;
    public GameObject missileLauncherPrefab;
    public GameObject aoeTowerPrefab;
    public GameObject fireTurret;
    public GameObject iceTurret;
    public GameObject waterTurret;
    public GameObject fireMissileLauncher;
    public GameObject iceMissileLauncher;
    public GameObject waterMissileLauncher;
    public GameObject fireAoeTower;
    public GameObject iceAoeTower;
    public GameObject waterAoeTower;
    public GameObject upTowerPrefab;
    public GameObject upMissileLauncherPrefab;
    public GameObject upFireTurret;
    public GameObject upIceTurret;
    public GameObject upWaterTurret;
    public GameObject upFireMissileLauncher;
    public GameObject upIceMissileLauncher;
    public GameObject upWaterMissileLauncher;
    public GameObject upAoeTower;
    public GameObject upFireAoeTower;
    public GameObject upIceAoeTower;
    public GameObject upWaterAoeTower;
    public GameObject upgradeButton;
    public GameObject destroyButton;
    public GameObject iceButton;
    public GameObject fireButton;
    public GameObject waterButton;
    public GameObject insufficientInkEffect;
    public GameObject nodeUIGO;


    private void Awake()
    {
        if (instance != null) 
        {
            Debug.Log("This should be a singleton! Only 1 should exist in a scene.");
            return;
        }
        instance = this;
        towerToBuild = standardTowerPrefab;
    }

    public GameObject GetTowerToBuild()
    { 
        return towerToBuild;
    }

    public void SelectNode(Node node) 
    {
        if (selectedNode == node)
        {
            DeselectNode();
            return;
        }
        selectedNode = node;
        nodeUI.SetTarget(node);
        if (node.tower != null) {
            UpdateUiTooltip(node);
        }
    }

    public void DeselectNode()
    { 
        selectedNode = null;
        nodeUI.Hide();
    }

    private void UpdateUiTooltip(Node node) {
        upgradeButton.GetComponent<NodeUiTooltipTrigger>().SetNode(node);
        destroyButton.GetComponent<NodeUiTooltipTrigger>().SetNode(node);
        iceButton.GetComponent<NodeUiTooltipTrigger>().SetNode(node);
        fireButton.GetComponent<NodeUiTooltipTrigger>().SetNode(node);
        waterButton.GetComponent<NodeUiTooltipTrigger>().SetNode(node);
    }

    public void SetTowerToBuild(GameObject tower)
    {
        Debug.Log(tower.name);
        towerToBuild = tower; 
        DeselectNode();
    }

    // Change tower elements
    public void ChangeTowerElement(GameObject newTower, GameObject playerGO, GameObject origTower)
    {
        float cost = newTower.GetComponent<Turret>().GetSwapElementCost();
        Player player = playerGO.GetComponent<Player>();
        // Check if enough ink
        if (!player.hasEnoughInk(cost))
        {
            Instantiate(insufficientInkEffect, nodeUIGO.transform.position, Quaternion.identity);

            // Reset towerToBuild
            this.towerToBuild = origTower;

            return;
        }

        // Get rid of original tower
        nodeUI.target.DestroyTower();

        // Build new tower with correct element
        nodeUI.target.SwapTower();

        player.ChangeInkAmount(-cost);

        // Reset towerToBuild
        this.towerToBuild = origTower;

        // Update UI tooltips
        UpdateUiTooltip(nodeUI.target);

        // Hide nodeUI
        DeselectNode();
    }

    public void buildFireTurret()
    {
        Debug.Log(nodeUI.target.GetIsAddedElement());
        // Check if already added element
        if (nodeUI.target.GetIsAddedElement()) {
            return;
        }

        if (!playerGO.GetComponent<Player>().hasEnoughInk(towerToBuild.GetComponent<Turret>().GetSwapElementCost())) {
            Instantiate(insufficientInkEffect, nodeUIGO.transform.position, Quaternion.identity);
            return;
        }

        GameObject origTower = towerToBuild;

        // Set respective towers
        if (!nodeUI.target.GetIsUpgraded())
        {
            if (nodeUI.target.tower.tag == "Turret")
            {
                towerToBuild = fireTurret;
            }
            else if (nodeUI.target.tower.tag == "Missile")
            {
                towerToBuild = fireMissileLauncher;
            }
            else if (nodeUI.target.tower.tag == "AOE")
            {
                towerToBuild = fireAoeTower;
            }
        }
        else
        {
            if (nodeUI.target.tower.tag == "Turret")
            {
                towerToBuild = upFireTurret;
            }
            else if (nodeUI.target.tower.tag == "Missile")
            {
                towerToBuild = upFireMissileLauncher;
            }
            else if (nodeUI.target.tower.tag == "AOE")
            {
                towerToBuild = upFireAoeTower;
            }
        }

        ChangeTowerElement(this.towerToBuild, this.playerGO, origTower);
        nodeUI.target.SetIsAddedElement(true);
    }

    public void buildIceTurret()
    {
        Debug.Log(nodeUI.target.GetIsAddedElement());
        // Check if already added element
        if (nodeUI.target.GetIsAddedElement()) {
            return;
        }

        if (!playerGO.GetComponent<Player>().hasEnoughInk(towerToBuild.GetComponent<Turret>().GetSwapElementCost())) {
            Instantiate(insufficientInkEffect, nodeUIGO.transform.position, Quaternion.identity);
            return;
        }

        GameObject origTower = towerToBuild;

        // Set respective towers
        if (!nodeUI.target.GetIsUpgraded())
        {
            if (nodeUI.target.tower.tag == "Turret")
            {
                towerToBuild = iceTurret;
            }
            else if (nodeUI.target.tower.tag == "Missile")
            {
                towerToBuild = iceMissileLauncher;
            }
            else if (nodeUI.target.tower.tag == "AOE")
            {
                towerToBuild = iceAoeTower;
            }
        }
        else
        {
            if (nodeUI.target.tower.tag == "Turret")
            {
                towerToBuild = upIceTurret;
            }
            else if (nodeUI.target.tower.tag == "Missile")
            {
                towerToBuild = upIceMissileLauncher;
            }
            else if (nodeUI.target.tower.tag == "AOE")
            {
                towerToBuild = upIceAoeTower;
            }
        }

        ChangeTowerElement(this.towerToBuild, this.playerGO, origTower);
        nodeUI.target.SetIsAddedElement(true);
    }

    public void buildWaterTurret()
    {
        Debug.Log(nodeUI.target.GetIsAddedElement());
        // Check if already added element
        if (nodeUI.target.GetIsAddedElement()) {
            return;
        }

        if (!playerGO.GetComponent<Player>().hasEnoughInk(towerToBuild.GetComponent<Turret>().GetSwapElementCost())) {
            Instantiate(insufficientInkEffect, nodeUIGO.transform.position, Quaternion.identity);
            return;
        }

        GameObject origTower = towerToBuild;

        // Set respective towers
        if (!nodeUI.target.GetIsUpgraded())
        {
            if (nodeUI.target.tower.tag == "Turret")
            {
                towerToBuild = waterTurret;
            }
            else if (nodeUI.target.tower.tag == "Missile")
            {
                towerToBuild = waterMissileLauncher;
            }
            else if (nodeUI.target.tower.tag == "AOE")
            {
                towerToBuild = waterAoeTower;
            }
        }
        else
        { 
            if (nodeUI.target.tower.tag == "Turret")
            {
                towerToBuild = upWaterTurret;
            } 
            else if (nodeUI.target.tower.tag == "Missile")
            {
                towerToBuild = upWaterMissileLauncher;
            }
            else if (nodeUI.target.tower.tag == "AOE")
            {
                towerToBuild = upWaterAoeTower;
            }
        }

        ChangeTowerElement(this.towerToBuild, this.playerGO, origTower);
        nodeUI.target.SetIsAddedElement(true);
    }

    public void UpgradeTower()
    {
        if (nodeUI.target.GetIsUpgraded()) {
            return;
        }

        if (!playerGO.GetComponent<Player>().hasEnoughInk(towerToBuild.GetComponent<Turret>().GetUpgradeCost())) {
            Instantiate(insufficientInkEffect, nodeUIGO.transform.position, Quaternion.identity);
            return;
        }

        GameObject origTower = towerToBuild;

        if (nodeUI.target.tower.tag == "Turret")
        {
            if (nodeUI.target.tower.GetComponent<Turret>().bulletPrefab.tag == "Basic")
            {
                towerToBuild = upTowerPrefab;
            }

            else if (nodeUI.target.tower.GetComponent<Turret>().bulletPrefab.tag == "Fire")
            {
                towerToBuild = upFireTurret;
            }

            else if (nodeUI.target.tower.GetComponent<Turret>().bulletPrefab.tag == "Ice")
            {
                towerToBuild = upIceTurret;
            }

            else if (nodeUI.target.tower.GetComponent<Turret>().bulletPrefab.tag == "Water")
            {
                towerToBuild = upWaterTurret;
            }
        }

        else if (nodeUI.target.tower.tag == "Missile")
        {
            if (nodeUI.target.tower.GetComponent<Turret>().bulletPrefab.tag == "Basic")
            {
                towerToBuild = upMissileLauncherPrefab;
            }

            else if (nodeUI.target.tower.GetComponent<Turret>().bulletPrefab.tag == "Fire")
            {
                towerToBuild = upFireMissileLauncher;
            }

            else if (nodeUI.target.tower.GetComponent<Turret>().bulletPrefab.tag == "Ice")
            {
                towerToBuild = upIceMissileLauncher;
            }

            else if (nodeUI.target.tower.GetComponent<Turret>().bulletPrefab.tag == "Water")
            {
                towerToBuild = upWaterMissileLauncher;
            }
        }

        else if (nodeUI.target.tower.tag == "AOE")
        {
            if (nodeUI.target.tower.GetComponent<Turret>().bulletPrefab.tag == "Basic")
            {
                towerToBuild = upAoeTower;
            }

            else if (nodeUI.target.tower.GetComponent<Turret>().bulletPrefab.tag == "Fire")
            {
                towerToBuild = upFireAoeTower;
            }

            else if (nodeUI.target.tower.GetComponent<Turret>().bulletPrefab.tag == "Ice")
            {
                towerToBuild = upIceAoeTower;
            }

            else if (nodeUI.target.tower.GetComponent<Turret>().bulletPrefab.tag == "Water")
            {
                towerToBuild = upWaterAoeTower;
            }
        }

        ChangeTowerElement(this.towerToBuild, this.playerGO, origTower);
        nodeUI.target.SetIsUpgraded(true);
    }

    

    public void DestroyTower()
    {
        nodeUI.target.DestroyTower();
        nodeUI.target.SetIsTowerBuilt(false);
        nodeUI.target.SetIsAddedElement(false);
        nodeUI.target.SetIsUpgraded(false);
        // Hide nodeUI
        DeselectNode();
    }
}
