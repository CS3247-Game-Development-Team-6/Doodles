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
    private GameObject currentTowerType;
    public GameObject standardTowerPrefab;
    public GameObject missileLauncherPrefab;
    public GameObject fireTurret;
    public GameObject iceTurret;
    public GameObject waterTurret;
    public GameObject fireMissileLauncher;
    public GameObject iceMissileLauncher;
    public GameObject waterMissileLauncher;

    private void Awake()
    {
        if (instance != null) 
        {
            Debug.Log("This should be a singleton! Only 1 should exist in a scene.");
            return;
        }
        instance = this;
        towerToBuild = standardTowerPrefab;
        currentTowerType = towerToBuild;
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
    }

    public void DeselectNode()
    { 
        selectedNode = null;
        nodeUI.Hide();
    }

    public void SetTowerToBuild(GameObject tower)
    {
        towerToBuild = tower; 
        DeselectNode();
    }

    public GameObject GetCurrentTowerType()
    {
        return currentTowerType;
    }

    public void SetCurrentTowerType(GameObject tower)
    {
        currentTowerType = tower;
    }

    // Change tower elements
    public void SwapTower(GameObject towerToBuild, GameObject playerGO)
    {
        float cost = towerToBuild.GetComponent<Turret>().GetSwapElementCost();
        Player player = playerGO.GetComponent<Player>();
        // Check if enough ink
        if (!player.hasEnoughInk(cost))
        {
            return;
        }

        // Get rid of original tower
        selectedNode.DestroyTower();

        // Build new tower with correct element
        selectedNode.SwapTower();

        player.ChangeInkAmount(-cost);

        selectedNode.SetIsAddedElement(true);

        // Reset towerToBuild
        if (towerToBuild.GetComponent<Turret>().name.Contains("Turret"))
            this.towerToBuild = standardTowerPrefab;
        else if (towerToBuild.GetComponent<Turret>().name.Contains("MissileLauncher"))
            this.towerToBuild = missileLauncherPrefab;

        // Hide nodeUI
        DeselectNode();
    }

    public void buildFireTurret()
    {

        // Check if already added element
        if (selectedNode.GetIsAddedElement())
        {
            return;
        }
        // Set respective towers
        if (currentTowerType == standardTowerPrefab)
        {
            towerToBuild = fireTurret;
        }
        else if (currentTowerType == missileLauncherPrefab)
        {
            towerToBuild = fireMissileLauncher;
        }

        SwapTower(this.towerToBuild, this.playerGO);
    }

    public void buildIceTurret()
    {
        // Check if already added element
        if (selectedNode.GetIsAddedElement())
        {
            return;
        }

        // Set respective towers
        if (currentTowerType == standardTowerPrefab)
        {
            towerToBuild = iceTurret;
        }
        else if (currentTowerType == missileLauncherPrefab)
        {
            towerToBuild = iceMissileLauncher;
        }

        SwapTower(this.towerToBuild, this.playerGO);
    }

    public void buildWaterTurret()
    {
        // Check if already added element
        if (selectedNode.GetIsAddedElement())
        {
            return;
        }

        // Set respective towers
        if (currentTowerType == standardTowerPrefab)
        {
            towerToBuild = waterTurret;
        }
        else if (currentTowerType == missileLauncherPrefab)
        {
            towerToBuild = waterMissileLauncher;
        }

        SwapTower(this.towerToBuild, this.playerGO);
    }

    

    public void DestroyTower()
    {
        selectedNode.DestroyTower();
        selectedNode.SetIsTowerBuilt(false);
        selectedNode.SetIsAddedElement(false);
        // Hide nodeUI
        DeselectNode();
    }
}
