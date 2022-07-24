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
        if (node.towerObj != null) {
            UpdateUiTooltip(node);
        }
    }

    public void DeselectNode()
    { 
        selectedNode = null;
        nodeUI.Hide();
    }

    private void UpdateUiTooltip(Node node) {
        /*
        upgradeButton.GetComponent<NodeUiTooltipTrigger>().SetNode(node);
        destroyButton.GetComponent<NodeUiTooltipTrigger>().SetNode(node);
        iceButton.GetComponent<NodeUiTooltipTrigger>().SetNode(node);
        fireButton.GetComponent<NodeUiTooltipTrigger>().SetNode(node);
        waterButton.GetComponent<NodeUiTooltipTrigger>().SetNode(node);
        */
    }

    public void SetTowerToBuild(GameObject tower)
    {
        Debug.Log(tower.name);
        towerToBuild = tower; 
        DeselectNode();
    }

    // Change tower elements. Assumes that there is enough ink.
    public void ChangeTowerElement(GameObject newTower, GameObject playerGO, GameObject origTower)
    {
        float cost = newTower.GetComponent<Turret>().GetSwapElementCost();
        InkManager player = playerGO.GetComponent<InkManager>();

        // Get rid of original tower
        nodeUI.selectedNode.DestroyTower();

        // Build new tower with correct element
        nodeUI.selectedNode.SwapTower();

        player.ChangeInkAmount(-cost);

        // Reset towerToBuild
        this.towerToBuild = origTower;

        // Update UI tooltips
        UpdateUiTooltip(nodeUI.selectedNode);

        // Hide nodeUI
        DeselectNode();
    }

    public void buildFireTurret()
    {
        // Check if already added element
        if (nodeUI.selectedNode.GetIsAddedElement()) {
            return;
        }

        if (!playerGO.GetComponent<InkManager>().hasEnoughInk(towerToBuild.GetComponent<Turret>().GetSwapElementCost())) {
            Instantiate(insufficientInkEffect, nodeUIGO.transform.position, Quaternion.identity);
            return;
        }

        GameObject origTower = towerToBuild;

        // Set respective towers
        if (!nodeUI.selectedNode.GetIsUpgraded())
        {
            if (nodeUI.selectedNode.towerObj.tag == "Turret")
            {
                towerToBuild = fireTurret;
            }
            else if (nodeUI.selectedNode.towerObj.tag == "Missile")
            {
                towerToBuild = fireMissileLauncher;
            }
            else if (nodeUI.selectedNode.towerObj.tag == "AOE")
            {
                towerToBuild = fireAoeTower;
            }
        }
        else
        {
            if (nodeUI.selectedNode.towerObj.tag == "Turret")
            {
                towerToBuild = upFireTurret;
            }
            else if (nodeUI.selectedNode.towerObj.tag == "Missile")
            {
                towerToBuild = upFireMissileLauncher;
            }
            else if (nodeUI.selectedNode.towerObj.tag == "AOE")
            {
                towerToBuild = upFireAoeTower;
            }
        }

        ChangeTowerElement(this.towerToBuild, this.playerGO, origTower);
        nodeUI.selectedNode.SetIsAddedElement(true);
    }

    public void buildIceTurret()
    {
        // Check if already added element
        if (nodeUI.selectedNode.GetIsAddedElement()) {
            return;
        }

        if (!playerGO.GetComponent<InkManager>().hasEnoughInk(towerToBuild.GetComponent<Turret>().GetSwapElementCost())) {
            Instantiate(insufficientInkEffect, nodeUIGO.transform.position, Quaternion.identity);
            return;
        }

        GameObject origTower = towerToBuild;

        // Set respective towers
        if (!nodeUI.selectedNode.GetIsUpgraded())
        {
            if (nodeUI.selectedNode.towerObj.tag == "Turret")
            {
                towerToBuild = iceTurret;
            }
            else if (nodeUI.selectedNode.towerObj.tag == "Missile")
            {
                towerToBuild = iceMissileLauncher;
            }
            else if (nodeUI.selectedNode.towerObj.tag == "AOE")
            {
                towerToBuild = iceAoeTower;
            }
        }
        else
        {
            if (nodeUI.selectedNode.towerObj.tag == "Turret")
            {
                towerToBuild = upIceTurret;
            }
            else if (nodeUI.selectedNode.towerObj.tag == "Missile")
            {
                towerToBuild = upIceMissileLauncher;
            }
            else if (nodeUI.selectedNode.towerObj.tag == "AOE")
            {
                towerToBuild = upIceAoeTower;
            }
        }

        ChangeTowerElement(this.towerToBuild, this.playerGO, origTower);
        nodeUI.selectedNode.SetIsAddedElement(true);
    }

    public void buildWaterTurret()
    {
        // Check if already added element
        if (nodeUI.selectedNode.GetIsAddedElement()) {
            return;
        }

        if (!playerGO.GetComponent<InkManager>().hasEnoughInk(towerToBuild.GetComponent<Turret>().GetSwapElementCost())) {
            Instantiate(insufficientInkEffect, nodeUIGO.transform.position, Quaternion.identity);
            return;
        }

        GameObject origTower = towerToBuild;

        // Set respective towers
        if (!nodeUI.selectedNode.GetIsUpgraded())
        {
            if (nodeUI.selectedNode.towerObj.tag == "Turret")
            {
                towerToBuild = waterTurret;
            }
            else if (nodeUI.selectedNode.towerObj.tag == "Missile")
            {
                towerToBuild = waterMissileLauncher;
            }
            else if (nodeUI.selectedNode.towerObj.tag == "AOE")
            {
                towerToBuild = waterAoeTower;
            }
        }
        else
        { 
            if (nodeUI.selectedNode.towerObj.tag == "Turret")
            {
                towerToBuild = upWaterTurret;
            } 
            else if (nodeUI.selectedNode.towerObj.tag == "Missile")
            {
                towerToBuild = upWaterMissileLauncher;
            }
            else if (nodeUI.selectedNode.towerObj.tag == "AOE")
            {
                towerToBuild = upWaterAoeTower;
            }
        }

        ChangeTowerElement(this.towerToBuild, this.playerGO, origTower);
        nodeUI.selectedNode.SetIsAddedElement(true);
    }

    public void UpgradeTower()
    {
        if (nodeUI.selectedNode.GetIsUpgraded()) {
            return;
        }

        if (!playerGO.GetComponent<InkManager>().hasEnoughInk(towerToBuild.GetComponent<Turret>().GetUpgradeCost())) {
            Instantiate(insufficientInkEffect, nodeUIGO.transform.position, Quaternion.identity);
            return;
        }

        GameObject origTower = towerToBuild;

        if (nodeUI.selectedNode.towerObj.tag == "Turret")
        {
            if (nodeUI.selectedNode.towerObj.GetComponent<Turret>().bulletPrefab.tag == "Basic")
            {
                towerToBuild = upTowerPrefab;
            }

            else if (nodeUI.selectedNode.towerObj.GetComponent<Turret>().bulletPrefab.tag == "Fire")
            {
                towerToBuild = upFireTurret;
            }

            else if (nodeUI.selectedNode.towerObj.GetComponent<Turret>().bulletPrefab.tag == "Ice")
            {
                towerToBuild = upIceTurret;
            }

            else if (nodeUI.selectedNode.towerObj.GetComponent<Turret>().bulletPrefab.tag == "Water")
            {
                towerToBuild = upWaterTurret;
            }
        }

        else if (nodeUI.selectedNode.towerObj.tag == "Missile")
        {
            if (nodeUI.selectedNode.towerObj.GetComponent<Turret>().bulletPrefab.tag == "Basic")
            {
                towerToBuild = upMissileLauncherPrefab;
            }

            else if (nodeUI.selectedNode.towerObj.GetComponent<Turret>().bulletPrefab.tag == "Fire")
            {
                towerToBuild = upFireMissileLauncher;
            }

            else if (nodeUI.selectedNode.towerObj.GetComponent<Turret>().bulletPrefab.tag == "Ice")
            {
                towerToBuild = upIceMissileLauncher;
            }

            else if (nodeUI.selectedNode.towerObj.GetComponent<Turret>().bulletPrefab.tag == "Water")
            {
                towerToBuild = upWaterMissileLauncher;
            }
        }

        else if (nodeUI.selectedNode.towerObj.tag == "AOE")
        {
            if (nodeUI.selectedNode.towerObj.GetComponent<Turret>().bulletPrefab.tag == "Basic")
            {
                towerToBuild = upAoeTower;
            }

            else if (nodeUI.selectedNode.towerObj.GetComponent<Turret>().bulletPrefab.tag == "Fire")
            {
                towerToBuild = upFireAoeTower;
            }

            else if (nodeUI.selectedNode.towerObj.GetComponent<Turret>().bulletPrefab.tag == "Ice")
            {
                towerToBuild = upIceAoeTower;
            }

            else if (nodeUI.selectedNode.towerObj.GetComponent<Turret>().bulletPrefab.tag == "Water")
            {
                towerToBuild = upWaterAoeTower;
            }
        }

        ChangeTowerElement(this.towerToBuild, this.playerGO, origTower);
        nodeUI.selectedNode.SetIsUpgraded(true);
    }

    

    public void DestroyTower()
    {
        nodeUI.selectedNode.DestroyTower();
        nodeUI.selectedNode.SetIsTowerBuilt(false);
        nodeUI.selectedNode.SetIsAddedElement(false);
        nodeUI.selectedNode.SetIsUpgraded(false);
        // Hide nodeUI
        DeselectNode();
    }
}
